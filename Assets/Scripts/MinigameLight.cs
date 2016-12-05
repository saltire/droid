using UnityEngine;

public class MinigameLight : MonoBehaviour {
	public float flickerSpeed = 20;

	PoweredComponent[] sources = new PoweredComponent[2];
	Material material;

	void Start() {
		material = GetComponent<Renderer>().material;
	}

	public void FindSources() {
		Vector3 offsetX = new Vector3(transform.localScale.x / 2, 0, 0);
		Vector3 boxDims = new Vector3(0.5f, transform.localScale.y / 2, 0.5f);

		foreach (Collider other in Physics.OverlapBox(transform.position - offsetX, boxDims)) {
			PoweredComponent powered = other.GetComponent<PoweredComponent>();
			if (powered != null) {
				sources[0] = powered;
			}
		}
		foreach (Collider other in Physics.OverlapBox(transform.position + offsetX, boxDims)) {
			PoweredComponent powered = other.GetComponent<PoweredComponent>();
			if (powered != null) {
				sources[1] = powered;
			}
		}
	}

	void LateUpdate() {
		bool powered0 = sources[0] != null && sources[0].color != Color.clear;
		bool powered1 = sources[1] != null && sources[1].color != Color.clear;

		if (powered0 && powered1) {
			material.SetColor("_EmissionColor", Color.Lerp(sources[0].color, sources[1].color, Mathf.PingPong(Time.unscaledTime * flickerSpeed, 1)));
		}
		else if (powered0) {
			material.SetColor("_EmissionColor", sources[0].color);
		}
		else if (powered1) {
			material.SetColor("_EmissionColor", sources[1].color);
		}
		else {
			material.SetColor("_EmissionColor", Color.clear);
		}
	}
}
