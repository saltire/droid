using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PoweredComponent : MonoBehaviour {
	public Color color = Color.clear;

	void LateUpdate () {
		GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
	}

	public void TransmitPower() {
		List<PoweredComponent> sources = GetAdjacentComponents(transform, false);
		color = (sources.Count > 0 && sources[0].color != Color.clear && sources.All(source => source.color == sources[0].color)) ? sources[0].color : Color.clear;

		foreach (PoweredComponent dest in GetAdjacentComponents(transform, true)) {
			dest.TransmitPower();
		}
	}

	List<PoweredComponent> GetAdjacentComponents(Transform trans, bool forward) {
		List<PoweredComponent> adjacents = new List<PoweredComponent>();
		foreach (Collider other in Physics.OverlapBox(trans.position + new Vector3(trans.localScale.x / 2 * (forward ? 1 : -1), 0, 0), new Vector3(0.5f, trans.localScale.y / 2, 0.5f))) {
			PoweredComponent powered = other.GetComponent<PoweredComponent>();
			if (other.transform != trans && powered != null) {
				adjacents.Add(powered);
			}
		}
		return adjacents;
	}
}
