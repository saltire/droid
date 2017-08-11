using UnityEngine;
using System.Collections.Generic;

public class Disruptor : Weapon {
	public float effectLength = 0.25f;
	public float flashLength = 0.05f;

	float killTime;
	float nextFlashTime;
	Material mat;

	void Start() {
		killTime = Time.time + effectLength;
		nextFlashTime = Time.time + flashLength;
		mat = GetComponent<Renderer>().material;
	}

	void Update() {
		if (Time.time >= killTime) {
			Destroy(gameObject);
		}

		if (Time.time >= nextFlashTime) {
			nextFlashTime += flashLength;
			mat.SetColor("_EmissionColor", mat.GetColor("_EmissionColor") == Color.black ? Color.white : Color.black);
		}
	}
}
