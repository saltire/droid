using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PoweredComponent : MonoBehaviour {
	public Color color;

	public void TransmitPower() {
		List<PoweredComponent> sources = GetAdjacentComponents(false);
		color = (sources.Count > 0 && sources[0].color != Color.clear && sources.All(source => source.color == sources[0].color)) ? sources[0].color : Color.clear;

		foreach (PoweredComponent dest in GetAdjacentComponents(true)) {
			dest.TransmitPower();
		}
	}

	public List<PoweredComponent> GetAdjacentComponents(bool forward) {
		List<PoweredComponent> adjacents = new List<PoweredComponent>();
		foreach (Collider other in Physics.OverlapBox(transform.position + new Vector3(transform.localScale.x / 2 * (forward ? 1 : -1), 0, 0), new Vector3(0.5f, transform.localScale.y / 2, 0.5f))) {
			PoweredComponent powered = other.GetComponent<PoweredComponent>();
			if (powered != this && powered != null) {
				adjacents.Add(powered);
			}
		}
		return adjacents;
	}

	void LateUpdate () {
		GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
	}
}
