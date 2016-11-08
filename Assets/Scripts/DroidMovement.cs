﻿using UnityEngine;

public class DroidMovement : MonoBehaviour {
	NavMeshAgent agent;
	GameObject[] waypoints;
	int currentPoint;
	float waitTime = 2f;
	float timeWaited = 0f;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
		waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
	}

	void Update() {
		if (currentPoint == -1) {
			// Keep waiting at the current position, then set a destination.
			timeWaited += Time.deltaTime;
			if (timeWaited >= waitTime) {
				timeWaited = 0;
				PickDestination();
			}
		}
		else {
			// Check if we are at the current destination, and if so set a new one.
			if ((transform.position - agent.destination).magnitude < 0.8f) {
				PickDestination();
			}
		}
	}

	void PickDestination() {
		// Pick a waypoint at random, or -1 to wait in position.
		int point = currentPoint;
		while (point == currentPoint) {
			point = Random.Range(-1, waypoints.Length - 1);
		}
		currentPoint = point;

		// Set destination.
		if (currentPoint > -1) {
			agent.SetDestination(waypoints[currentPoint].transform.position);
		}
	}
}
