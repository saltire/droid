using UnityEngine;
using System.Collections.Generic;

public class Lights : MonoBehaviour {
	public Color lightColor = new Color(0.1f, 0.5f, 0.2f);
	public Color darkColor = new Color(0.1f, 0.1f, 0.1f);
	public float darkenTime = 1f;

	List<GameObject> blocks = new List<GameObject>();
	float offTime;

	void Start() {
		foreach (Transform child in GetComponentsInChildren<Transform>()) {
			if (child.tag == "Solid") {
				blocks.Add(child.gameObject);
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
			Color color = Color.Lerp(lightColor, darkColor, (1 - offTime + Time.time) / darkenTime);

			foreach (GameObject block in blocks) {
				block.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
			}
		}
	}
}
