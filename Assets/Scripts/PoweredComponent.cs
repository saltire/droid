﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PoweredComponent : MonoBehaviour {
	public Color color;

	float expiryTime = 0;

	public bool IsPowered() {
		return color != Color.clear;
	}

	public void TransmitPower() {
		List<PoweredComponent> sources = GetAdjacentComponents(false);
		color = (sources.Count > 0 && sources[0].IsPowered() && sources.All(source => source.color == sources[0].color)) ? sources[0].color : Color.clear;

		foreach (PoweredComponent dest in GetAdjacentComponents(true)) {
			dest.TransmitPower();
		}
	}

	Collider[] GetAdjacentColliders(bool forward) {
		return Physics.OverlapBox(transform.position + new Vector3(transform.localScale.x / 2 * (forward ? 1 : -1), 0, 0), new Vector3(0.5f, transform.localScale.y / 2, 0.5f));
	}

	public List<PoweredComponent> GetAdjacentComponents(bool forward) {
		List<PoweredComponent> adjacents = new List<PoweredComponent>();
		foreach (Collider other in GetAdjacentColliders(forward)) {
			PoweredComponent powered = other.GetComponent<PoweredComponent>();
			if (powered != this && powered != null) {
				adjacents.Add(powered);
			}
		}
		return adjacents;
	}

	public HashSet<MinigameLight> GetConnectedLights() {
		HashSet<MinigameLight> lights = new HashSet<MinigameLight>();

		foreach (Collider other in GetAdjacentColliders(true)) {
			MinigameLight light = other.GetComponent<MinigameLight>();
			if (light != null) {
				lights.Add(light);
			}
			else {
				PoweredComponent powered = other.GetComponent<PoweredComponent>();
				if (powered != this && powered != null) {
					lights.UnionWith(powered.GetConnectedLights());
				}
			}
		}
		return lights;
	}

	public void ActivatePulser(Color newColor, float length) {
		color = newColor;
		expiryTime = Time.unscaledTime + length;
	}

	public float GetTimeRemaining() {
		return expiryTime - Time.unscaledTime;
	}

	void Update() {
		if (expiryTime > 0 && expiryTime <= Time.unscaledTime) {
			Destroy(gameObject);
		}
	}

	void LateUpdate () {
		GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
	}
}
