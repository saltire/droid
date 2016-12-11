﻿using UnityEngine;
using System;
using System.Collections.Generic;

public class Minigame : MonoBehaviour {
	public Camera minigameCamera;
	public GameObject MinigameSide;

	public Color player1Color = Color.yellow;
	public Color player2Color = Color.magenta;
	public int basePulsers = 3;

	public float warmupLength = 1;
	public float gameLength = 8;
	public float cooldownLength = 1;
	public float gameoverLength = 1;
	public float pulserLength = 4;
	public float flickerSpeed = 20;

	float startTime;
	int player1Pulsers;
	int player2Pulsers;
	bool cooldownDone = false;
	bool gameoverDone = false;

	MinigameSide[] sides = new MinigameSide[2];
	MinigameLight[] lights;
	Material topLightMaterial;
	List<PoweredComponent> startSegments;
	List<PoweredComponent> pulsers;
	List<Transform> timers = new List<Transform>();

	void Start() {
		lights = GetComponentsInChildren<MinigameLight>();
		topLightMaterial = transform.Find("TopLight").GetComponent<Renderer>().material;
		foreach (Transform child in transform) {
			if (child.name == "Timer") {
				timers.Add(child);
			}
		}
	}

	public void StartMinigame(int player1type, int player2type) {
		Time.timeScale = 0;
		startTime = Time.unscaledTime;
		player1Pulsers = basePulsers + player1type / 100;
		player2Pulsers = basePulsers + player2type / 100;
		cooldownDone = false;
		gameoverDone = false;

		// Instantiate a side of the game for each player.
		sides[0] = Instantiate(MinigameSide, transform.position, Quaternion.identity).GetComponent<MinigameSide>();
		sides[0].transform.parent = transform;

		sides[1] = Instantiate(MinigameSide, transform.position, Quaternion.identity).GetComponent<MinigameSide>();
		sides[1].transform.parent = transform;
		sides[1].transform.localScale = new Vector3(-1, 1, 1);

		// Build patterns on each side of the game, and store refs to the starting wire segments and pulsers.
		startSegments = new List<PoweredComponent>();
		startSegments.AddRange(sides[0].Build(player1Color, player1Pulsers, false));
		startSegments.AddRange(sides[1].Build(player2Color, player2Pulsers, true));

		pulsers = new List<PoweredComponent>(GetComponentsInChildren<PoweredComponent>()).FindAll(powered => powered.tag == "Pulser");

		// Initialize the timers.
		foreach (Transform timer in timers) {
			timer.localScale = new Vector3(0, 1, 1);
		}

		// Reset each light, and create references to connecting wires.
		foreach (MinigameLight light in lights) {
			light.FindSources();
			light.Reset();
		}
		topLightMaterial.SetColor("_EmissionColor", Color.clear);

		minigameCamera.enabled = true;
	}

	void Update() {
		if (!minigameCamera.enabled) {
			return;
		}

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
			UpdatePowerGrid();
		}
		else if (gameTime <= (warmupLength + gameLength + cooldownLength)) {
			if (!cooldownDone) {
				// Remove the timers.
				foreach (Transform timer in timers) {
					timer.localScale = new Vector3(0, 1, 1);
				}

				// Remove the current pulsers.
				foreach (MinigameSide side in sides) {
					PoweredComponent currentPulser = side.GetCurrentPulser();
					if (currentPulser != null) {
						Destroy(currentPulser.gameObject);
					}
				}

				cooldownDone = true;
			}

			// Continue to update power components and lights.
			UpdatePowerGrid();
		}
		else if (gameTime <= (warmupLength + gameLength + cooldownLength + gameoverLength)) {
			if (!gameoverDone) {
				// Remove all activated pulsers in the order they were placed, updating the power grid after each.
				List<PoweredComponent> activePulsers = pulsers.FindAll(powered => powered != null && powered.IsPowered());
				activePulsers.Sort((a, b) => Math.Sign(a.GetTimeRemaining() - b.GetTimeRemaining()));
				foreach (PoweredComponent pulser in activePulsers) {
					DestroyImmediate(pulser.gameObject);
					UpdatePowerGrid();
				}

				gameoverDone = true;
			}
		}
		else {
			// Reset camera and timescale.
			minigameCamera.enabled = false;
			Time.timeScale = 1;
			foreach (MinigameSide side in sides) {
				DestroyImmediate(side.gameObject);
			}

			// Take action depending on the minigame's outcome.
			Influence influence = GameObject.FindGameObjectWithTag("Player").GetComponent<Influence>();
			Color finalColor = topLightMaterial.GetColor("_EmissionColor");
			if (finalColor == player1Color) {
				influence.OnHackSuccess();
			}
			else if (finalColor == player2Color) {
				influence.OnHackFailure();
			}
			else {
				StartMinigame(player1Pulsers, player2Pulsers);
			}
		}
	}

	void UpdatePowerGrid() {
		// Update power components.
		foreach (PoweredComponent startSegment in startSegments) {
			startSegment.TransmitPower();
		}

		// Update the color and get the powered state of each light.
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

		// Update the top light.
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
