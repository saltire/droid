using UnityEngine;
using System;

public class Minigame : MonoBehaviour {
	public float flickerSpeed = 20;

	MinigameSide[] sides;
	MinigameLight[] lights;
	Material topLightMaterial;

	void Start() {
		// Time.timeScale = 0;

		sides = GetComponentsInChildren<MinigameSide>();
		lights = GetComponentsInChildren<MinigameLight>();
		topLightMaterial = transform.Find("TopLight").GetComponent<Renderer>().material;

		// Build patterns on each side of the game.
		foreach (MinigameSide side in sides) {
			side.Build();
		}

		// For each light, create references to connecting wires.
		foreach (MinigameLight light in lights) {
			light.FindSources();
		}
	}

	void Update() {
		// Update the powered components on each side.
		foreach (MinigameSide side in sides) {
			side.UpdatePowerComponents();
		}

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
