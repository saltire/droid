using UnityEngine;

public class Minigame : MonoBehaviour {
	void Start() {
		// Time.timeScale = 0;

		// Build patterns on each side of the game.
		foreach (MinigameSide side in GetComponentsInChildren<MinigameSide>()) {
			side.Build();
		}

		// For each light, create references to connecting wires.
		foreach (MinigameLight light in GetComponentsInChildren<MinigameLight>()) {
			light.FindSources();
		}
	}
}
