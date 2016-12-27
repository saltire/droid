using UnityEngine;
using System.Collections.Generic;

public class Lights : MonoBehaviour {
	public float darkenTime = 1f;

	List<Renderer> blocks = new List<Renderer>();
	Color lightColor = Color.clear;
	float offTime;

	void Start() {
		foreach (Transform child in GetComponentsInChildren<Transform>()) {
			if (child.tag == "Solid") {
				Renderer block = child.GetComponent<Renderer>();
				blocks.Add(block);

				if (lightColor == Color.clear) {
					lightColor = block.material.GetColor("_EmissionColor");
				}
				DynamicGI.SetEmissive(block, lightColor);
			}
		}
	}

	public void CheckDroidCount() {
		int count = 0;
		foreach (Transform child in transform) {
			if (child.tag == "Droid" && child.GetComponent<Health>().IsAlive()) {
				count++;
			}
		}
		if (count == 0) {
			DimLights();
		}
	}

	public void DimLights() {
		offTime = Time.time + darkenTime;
	}

	void Update() {
		if (offTime > Time.time) {
			Color color = Color.Lerp(lightColor, Color.clear, (1 - offTime + Time.time) / darkenTime);

			foreach (Renderer block in blocks) {
				block.material.SetColor("_EmissionColor", color);
				DynamicGI.SetEmissive(block, color);
			}
		}
	}
}
