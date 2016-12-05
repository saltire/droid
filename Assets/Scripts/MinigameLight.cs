using UnityEngine;

public class MinigameLight : MonoBehaviour {
	PoweredComponent[] sources = new PoweredComponent[2];
	Material material;

	int colorState = 0;
	float flickerSpeed;

	void Start() {
		material = GetComponent<Renderer>().material;
		flickerSpeed = GetComponentInParent<Minigame>().flickerSpeed;
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

	public int UpdateColorState() {
		bool[] powered = new bool[2] {
			sources[0] != null && sources[0].color != Color.clear,
			sources[1] != null && sources[1].color != Color.clear,
		};

		if (powered[0] && powered[1]) {
			material.SetColor("_EmissionColor", Color.Lerp(sources[0].color, sources[1].color, Mathf.PingPong(Time.unscaledTime * flickerSpeed, 1)));
			colorState = 3;
		}
		else if (powered[0]) {
			material.SetColor("_EmissionColor", sources[0].color);
			colorState = 1;
		}
		else if (powered[1]) {
			material.SetColor("_EmissionColor", sources[1].color);
			colorState = 2;
		}

		return colorState;
	}
}
