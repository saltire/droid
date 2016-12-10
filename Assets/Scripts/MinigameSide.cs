﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class MinigameSide : MonoBehaviour {
	public Color color = Color.yellow;
	public float pulserCount = 5;
	public bool computerPlayer = false;

	List<PoweredComponent> startSegments = new List<PoweredComponent>();
	Stack<PoweredComponent> unusedPulsers = new Stack<PoweredComponent>();
	PoweredComponent currentPulser;

	float offsetX = -132.5f;
	float offsetY = -38;
	float rowHeight = -11;
	float xScale = 15;

	float pulserOffsetX = -123;
	float pulserSpacing = 13;
	float pulserLength;

	bool fireReleased = false;
	bool moveReleased = true;

	public List<PoweredComponent> Build() {
		pulserLength = GetComponentInParent<Minigame>().pulserLength;

		Dictionary<string, GameObject> prefabs = GetComponentInParent<MinigameBuilder>().GetPrefabs();

		// Place patterns.
		int row = 0;
		while (row < 12) {
			MinigameBuilder.PatternComponent[] pattern = MinigameBuilder.patterns[UnityEngine.Random.Range(0, MinigameBuilder.patterns.Length)];
			int height = Mathf.Max(pattern.Select(component => component.y).ToArray()) + 1;
			if (row + height <= 12) {
				foreach (MinigameBuilder.PatternComponent pc in pattern) {
					GameObject comp = (GameObject)Instantiate(prefabs[pc.prefab], transform.position, Quaternion.identity);
					comp.transform.parent = transform;
					comp.transform.localPosition += new Vector3(pc.x * xScale + offsetX, (pc.y + row) * rowHeight + offsetY, 0);
					comp.transform.localScale = Vector3.Scale(comp.transform.localScale, new Vector3(pc.xScale, 1, 1));
				}
				row += height;
			}
		}

		// Set color of power source and (testing) pulsers.
		foreach (Transform child in transform) {
			if (child.name == "Source") {
				child.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);

				// Build a list of wire segments connected to each power source.
				foreach (Collider other in Physics.OverlapBox(child.position, new Vector3(child.localScale.x / 2 + 0.5f, child.localScale.y / 2, 0.5f))) {
					PoweredComponent powered = other.GetComponent<PoweredComponent>();
					if (powered != null && other.tag == "WireSegment") {
						startSegments.Add(powered);
					}
				}
				startSegments.Sort((a, b) => Math.Sign(a.transform.position.y - b.transform.position.y));
			}

			// Pulsers placed for testing only.
			if (child.tag == "Pulser") {
				child.GetComponent<PoweredComponent>().color = color;
			}
		}

		// Add pulsers.
		for (int i = 0; i < pulserCount; i++) {
			GameObject pulser = (GameObject)Instantiate(prefabs["Pulser"], transform.position, Quaternion.Euler(0, 0, -90));
			pulser.transform.parent = transform;
			pulser.transform.localPosition += new Vector3(pulserOffsetX + pulserSpacing * i, offsetY - (rowHeight * 2), 0);
			unusedPulsers.Push(pulser.GetComponent<PoweredComponent>());
		}

		return startSegments;
	}

	public void DoAction() {
		if (currentPulser == null) {
			NextPulser();
		}

		if (computerPlayer) {
			DoAIAction();
		}
		else {
			DoPlayerAction();
		}
	}

	void DoAIAction() {

	}

	void DoPlayerAction() {
		if (!fireReleased && Input.GetAxisRaw("Use") == 0) {
			fireReleased = true;
		}
		if (!moveReleased && Input.GetAxisRaw("Vertical") == 0) {
			moveReleased = true;
		}

		if (moveReleased) {
			int direction = Math.Sign(Input.GetAxisRaw("Vertical"));

			if (direction != 0 && currentPulser != null) {
				moveReleased = false;

				// Get a list of open segments, and the current segment that the pulser is on.
				List<PoweredComponent> openSegments = startSegments.FindAll(segment => segment.GetAdjacentComponents(false).FindAll(adj => adj.tag == "Pulser" && adj.IsPowered()).Count == 0);
				int wireIndex = -1;
				foreach (Collider other in Physics.OverlapSphere(currentPulser.GetComponent<CapsuleCollider>().transform.position, 1)) {
					PoweredComponent powered = other.GetComponent<PoweredComponent>();
					if (other.tag == "WireSegment" && powered != null) {
						wireIndex = openSegments.IndexOf(powered);
					}
				}

				// Get the segment to move to, and move the pulser.
				int nextWireIndex = (direction > 0 ? wireIndex + 1 : Math.Max(wireIndex, 0) + openSegments.Count - 1) % openSegments.Count;
				currentPulser.transform.localPosition = new Vector3(currentPulser.transform.localPosition.x, openSegments[nextWireIndex].transform.localPosition.y, 0);
			}
		}

		if (fireReleased && Input.GetAxisRaw("Use") > 0) {
			if (currentPulser != null) {
				// Activate the current pulser and get the next one.
				currentPulser.ActivatePulser(color, pulserLength);
				NextPulser();
				fireReleased = false;
			}
		}
	}

	void NextPulser() {
		if (unusedPulsers.Count == 0) {
			currentPulser = null;
			return;
		}

		currentPulser = unusedPulsers.Pop();
		currentPulser.transform.localPosition = new Vector3(pulserOffsetX, offsetY - rowHeight, 0);
	}
}
