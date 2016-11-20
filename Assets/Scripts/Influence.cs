using UnityEngine;
using System.Collections.Generic;

public class Influence : MonoBehaviour {
	public float flashSpeed = 3;
	public Color flashColor = Color.red;

	List<Material> materials = new List<Material>();
	float holdTime = 0;

	void Start() {
		foreach (Renderer renderer in transform.FindChild("Body").GetComponentsInChildren<Renderer>()) {
			materials.Add(renderer.material);
		}
	}

	void Update() {
		if (holdTime > 0 && Input.GetAxis("Influence") == 0) {
			holdTime = 0;

			foreach (Material material in materials) {
				material.SetColor("_EmissionColor", Color.black);
			}
		}
		else if (Input.GetAxis("Influence") > 0) {
			holdTime += Time.deltaTime;
			float mult = Mathf.PingPong(holdTime * flashSpeed, 1);
			Color color = new Color(flashColor.r * mult, flashColor.g * mult, flashColor.b * mult);

			foreach (Material material in materials) {
				material.SetColor("_EmissionColor", color);
			}
		}
	}
}
