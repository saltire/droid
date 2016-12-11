using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class MinigameSide : MonoBehaviour {
	public Color color = Color.yellow;
	public int pulserCount = 5;
	public bool computerPlayer = false;
	public float playerMoveCooldown = 0.1f;
	public float aiActionCooldown = 0.1f;

	List<PoweredComponent> startSegments;
	Stack<PoweredComponent> unusedPulsers;
	PoweredComponent currentPulser;

	float offsetX = -132.5f;
	float offsetY = -38;
	float rowHeight = -11;
	float xScale = 15;

	float pulserOffsetX = -127;
	float pulserSpacing = 9.5f;
	float pulserLength;

	bool fireReleased = false;
	float playerMoveCooldownTime = 0;
	float aiActionCooldownTime = 0;

	public List<PoweredComponent> Build(Color newColor, int newPulserCount, bool newComputerPlayer) {
		color = newColor;
		pulserCount = newPulserCount;
		computerPlayer = newComputerPlayer;

		pulserLength = GetComponentInParent<Minigame>().pulserLength;

		fireReleased = false;
		playerMoveCooldownTime = 0;
		aiActionCooldownTime = 0;

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

		foreach (Transform child in transform) {
			if (child.name == "Source") {
				child.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);

				// Build a sorted list of wire segments connected to the power source.
				startSegments = new List<PoweredComponent>();
				foreach (Collider other in Physics.OverlapBox(child.position, new Vector3(child.localScale.x / 2 + 0.5f, child.localScale.y / 2, 0.5f))) {
					PoweredComponent powered = other.GetComponent<PoweredComponent>();
					if (powered != null && other.tag == "WireSegment") {
						startSegments.Add(powered);
					}
				}
				startSegments.Sort((a, b) => Math.Sign(a.transform.position.y - b.transform.position.y));
			}
		}

		// Add pulsers.
		unusedPulsers = new Stack<PoweredComponent>();
		for (int i = 0; i < pulserCount; i++) {
			GameObject pulser = (GameObject)Instantiate(prefabs["Pulser"], transform.position, Quaternion.identity);
			pulser.transform.parent = transform;
			pulser.transform.localRotation = Quaternion.Euler(0, 0, -90);
			pulser.transform.localPosition += new Vector3(pulserOffsetX + pulserSpacing * i, offsetY - (rowHeight * 2), 0);
			unusedPulsers.Push(pulser.GetComponent<PoweredComponent>());
		}
		currentPulser = null;

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
		if (currentPulser == null || aiActionCooldownTime > Time.unscaledTime) {
			return;
		}

		// Find the number of lights connected to the current pulser, and what color they are.
		int connectedLights = 0;
		int enemyConnectedLights = 0;
		PoweredComponent currentSegment = GetCurrentSegment();
		if (currentSegment != null) {
			HashSet<MinigameLight> lights = currentSegment.GetConnectedLights();
			connectedLights = lights.Count;
			enemyConnectedLights = lights.Where(light => light.GetColor() != color).Count();
		}

		// Create a weighted pool of potential actions.
		Dictionary<string, int> actions = new Dictionary<string, int>() {
			{"MoveUp", 1},
			{"MoveDown", 15},
			{"UsePulser", 1 + connectedLights + enemyConnectedLights * 20},
			{"Wait", 15 - pulserCount}
		};
		List<string> actionPool = new List<string>();
		foreach (KeyValuePair<string, int> act in actions) {
			for (int i = 0; i < act.Value; i++) {
				actionPool.Add(act.Key);
			}
		}

		// Pick an action and execute it.
		string action = actionPool[UnityEngine.Random.Range(0, actionPool.Count)];
		if (action == "MoveUp") {
			MovePulser(1);
		}
		else if (action == "MoveDown") {
			MovePulser(-1);
		}
		else if (action == "UsePulser") {
			UsePulser();
		}

		aiActionCooldownTime = Time.unscaledTime + aiActionCooldown;
	}

	void DoPlayerAction() {
		if (currentPulser == null) {
			return;
		}

		if (playerMoveCooldownTime < Time.unscaledTime) {
			int direction = Math.Sign(Input.GetAxisRaw("Vertical"));

			if (direction != 0) {
				MovePulser(direction);
				playerMoveCooldownTime = Time.unscaledTime + playerMoveCooldown;
			}
		}

		if (!fireReleased && Input.GetAxisRaw("Use") == 0) {
			fireReleased = true;
		}
		if (fireReleased && Input.GetAxisRaw("Use") > 0) {
			if (GetCurrentSegment() != null) {
				UsePulser();
				fireReleased = false;
			}
		}
	}

	PoweredComponent GetCurrentSegment() {
		if (currentPulser != null) {
			foreach (Collider other in Physics.OverlapSphere(transform.position + new Vector3(offsetX * transform.localScale.x, currentPulser.transform.localPosition.y, 0), 1)) {
				PoweredComponent powered = other.GetComponent<PoweredComponent>();
				if (other.tag == "WireSegment" && powered != null) {
					return powered;
				}
			}
		}
		return null;
	}

	void MovePulser(int direction) {
		if (direction != 0 && currentPulser != null) {
			// Get a list of open segments, and the current segment that the pulser is on.
			// List<PoweredComponent> openSegments = startSegments.FindAll(segment => segment.GetAdjacentComponents(false).FindAll(adj => adj.tag == "Pulser" && adj.IsPowered()).Count == 0);
			int wireIndex = startSegments.IndexOf(GetCurrentSegment());

			// Get the segment to move to, and move the pulser.
			int nextWireIndex = (direction > 0 ? wireIndex + 1 : Math.Max(wireIndex, 0) + startSegments.Count - 1) % startSegments.Count;
			currentPulser.transform.localPosition = new Vector3(currentPulser.transform.localPosition.x, startSegments[nextWireIndex].transform.localPosition.y, 0);
		}
	}

	void UsePulser() {
		PoweredComponent currentSegment = GetCurrentSegment();
		// Check that the current segment does not already have an active pulser.
		if (currentPulser != null && currentSegment != null && currentSegment.GetAdjacentComponents(false).FindAll(adj => adj.tag == "Pulser" && adj.IsPowered()).Count == 0) {
			// Activate the current pulser and get the next one.
			currentPulser.ActivatePulser(color, pulserLength);
			currentPulser.transform.localPosition += new Vector3(pulserSpacing, 0, 0);
			NextPulser();
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

	public PoweredComponent GetCurrentPulser() {
		return currentPulser;
	}
}
