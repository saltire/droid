﻿using UnityEngine;
using System;

public class Map : MonoBehaviour {
	public GameObject playerCamera;
	public GameObject mapCamera;

	int currentLiftIndex;
	int currentLiftPosition;
	Transform currentLiftObject;
	Transform currentLevelObject;

	bool fireReleased = false;
	bool moveReleased = true;

	string[][] lifts = new string[][] {
		new string[] {"Observation", "Airlock", "Reactor", "Engineering", "Maintenance"},
		new string[] {"Upper Cargo", "Mid Cargo", "Lower Cargo"},
		new string[] {"Bridge", "Reactor"},
		new string[] {"Observation", "Bridge", "Research", "Stores", "Staterooms"},
		new string[] {"Staterooms", "Upper Cargo", "Mid Cargo", "Shuttle Bay"},
		new string[] {"Robo-stores"},
		new string[] {"Research", "Stores", "Staterooms", "Repairs", "Quarters"},
		new string[] {"Bridge", "Research"},
	};

	public void ShowMap(Lift lift) {
		// Pause game.
		Time.timeScale = 0;

		string levelName = lift.transform.parent.name;
		currentLiftIndex = lift.liftIndex;

		SetLiftPosition(Array.IndexOf(lifts[currentLiftIndex], levelName));

		// Light up current lift.
		if (currentLiftObject) {
			currentLiftObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
		}
		currentLiftObject = transform.Find("Lift " + (char)(65 + currentLiftIndex));
		currentLiftObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);

		// Switch cameras.
		playerCamera.SetActive(false);
		mapCamera.SetActive(true);

		fireReleased = false;
	}

	void Update() {
		if (!fireReleased && Input.GetAxisRaw("Fire2") == 0) {
			fireReleased = true;
		}
		if (!moveReleased && Input.GetAxis("Vertical") == 0) {
			moveReleased = true;
		}

		if (mapCamera.activeSelf) {
			if (moveReleased) {
				int direction = Math.Sign(Input.GetAxis("Vertical"));

				if (direction != 0) {
					moveReleased = false;

					if (direction > 0 && currentLiftPosition > 0) {
						SetLiftPosition(currentLiftPosition - 1);
					}
					else if (direction < 0 && currentLiftPosition < lifts[currentLiftIndex].Length - 1) {
						SetLiftPosition(currentLiftPosition + 1);
					}
				}
			}

			if (fireReleased && Input.GetAxisRaw("Fire2") > 0) {
				foreach (Transform level in GameObject.Find("LevelBuilder").transform) {
					if (level.name == lifts[currentLiftIndex][currentLiftPosition]) {
						foreach (Lift otherLift in level.GetComponentsInChildren<Lift>()) {
							if (otherLift.liftIndex == currentLiftIndex) {
								otherLift.TemporarilyDisable();
								level.gameObject.SetActive(true);

								GameObject player = GameObject.FindGameObjectWithTag("Player");
								player.GetComponent<Rigidbody>().velocity = Vector3.zero;
								player.transform.parent = level;
								player.transform.position = otherLift.transform.position + new Vector3(0, 0.5f, 0);
							}
						}

						foreach (Transform otherLevel in GameObject.Find("LevelBuilder").transform) {
							if (otherLevel != level) {
								otherLevel.gameObject.SetActive(false);
							}
						}
					}
				}

				// Unpause game.
				Time.timeScale = 1;

				// Switch cameras.
				playerCamera.SetActive(true);
				mapCamera.SetActive(false);
			}
		}
	}

	void SetLiftPosition(int liftPosition) {
		currentLiftPosition = liftPosition;
		string levelName = lifts[currentLiftIndex][currentLiftPosition];

		// Light up current level.
		if (currentLevelObject) {
			foreach (Transform child in currentLevelObject) {
				child.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
			}
		}
		currentLevelObject = transform.Find(levelName);
		foreach (Transform child in currentLevelObject) {
			child.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.blue);
		}
	}
}
