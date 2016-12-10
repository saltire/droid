using UnityEngine;
using System;
using System.Collections.Generic;

public class Minigame : MonoBehaviour {
	public float warmupLength = 1;
	public float gameLength = 8;
	public float cooldownLength = 1;
	public float pulserLength = 4;
	public float flickerSpeed = 20;

	float startTime;

	List<Transform> timers = new List<Transform>();
	List<PoweredComponent> startSegments = new List<PoweredComponent>();
	MinigameSide[] sides;
	MinigameLight[] lights;
	Material topLightMaterial;

	void Start() {
		Time.timeScale = 0;
		startTime = Time.unscaledTime;

		sides = GetComponentsInChildren<MinigameSide>();
		lights = GetComponentsInChildren<MinigameLight>();
		topLightMaterial = transform.Find("TopLight").GetComponent<Renderer>().material;

		// Build patterns on each side of the game, and store refs to the starting wire segments.
		foreach (MinigameSide side in sides) {
			var sideSegments = side.Build();
			startSegments.AddRange(sideSegments);
		}

		// Initialize the timers.
		foreach (Transform child in transform) {
			if (child.name == "Timer") {
				timers.Add(child);
				child.localScale = new Vector3(0, 1, 1);
			}
		}

		// For each light, create references to connecting wires.
		foreach (MinigameLight light in lights) {
			light.FindSources();
		}
	}

	void Update() {
		float gameTime = Time.unscaledTime - startTime;

		if (gameTime <= warmupLength) {
			// Expand the timers.
			foreach (Transform timer in timers) {
				timer.localScale = new Vector3(gameTime / warmupLength, 1, 1);
			}
		}
		else if (gameTime <= (warmupLength + gameLength)) {
			// Shrink the timers.
			foreach (Transform timer in timers) {
				timer.localScale = new Vector3((warmupLength + gameLength - gameTime) / gameLength, 1, 1);
			}

			// Execute turn for each player.
			foreach (MinigameSide side in sides) {
				side.DoAction();
			}

			// Update power components and lights.
			UpdatePowerComponents();
			UpdateLights();
		}
		else if (gameTime <= (warmupLength + gameLength + cooldownLength)) {
			// Remove the timers.
			foreach (Transform timer in timers) {
				timer.localScale = new Vector3(0, 1, 1);
			}

			// Remove all activated pulsers in the order they were placed, updating the power grid after each.
			List<PoweredComponent> pulsers = new List<PoweredComponent>(GetComponentsInChildren<PoweredComponent>()).FindAll(powered => powered.tag == "Pulser" && powered.IsPowered());
			pulsers.Sort((a, b) => Math.Sign(a.GetTimeRemaining() - b.GetTimeRemaining()));
			foreach (PoweredComponent pulser in pulsers) {
				DestroyImmediate(pulser.gameObject);
				UpdatePowerComponents();
				UpdateLights();
			}
		}
	}

	void UpdatePowerComponents() {
		foreach (PoweredComponent startSegment in startSegments) {
			startSegment.TransmitPower();
		}
	}

	void UpdateLights() {
		// Update the color and get the powered state of each light, then update the top light.
		int solidScore = 0;
		int flickerCount = 0;
		foreach (MinigameLight light in lights) {
			int colorState = light.UpdateColorState();

			if (colorState == 1) {
				solidScore -= 1;
			}
			if (colorState == 2) {
				solidScore += 1;
			}
			if (colorState == 3) {
				flickerCount += 1;
			}
		}

		// Possible scores if all flickering lights go to player 1 or player 2, respectively.
		int[] flickerScores = new int[] {
			Math.Sign(solidScore - flickerCount),
			Math.Sign(solidScore + flickerCount),
		};

		if (flickerScores[0] == flickerScores[1]) {
			// Only one possible outcome regardless of who gets the flickering lights.
			topLightMaterial.SetColor("_EmissionColor", GetColorFromScore(solidScore));
		}
		else {
			// Two possible outcomes; flicker top light between the two.
			topLightMaterial.SetColor("_EmissionColor", Color.Lerp(GetColorFromScore(flickerScores[0]), GetColorFromScore(flickerScores[1]), Mathf.PingPong(Time.unscaledTime * flickerSpeed, 1)));
		}
	}

	Color GetColorFromScore(int score) {
		return score < 0 ? sides[0].color : (score > 0 ? sides[1].color : Color.clear);
	}
}
