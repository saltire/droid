using UnityEngine;

public class Explosion : MonoBehaviour {
	public float explosionTime = 1f;

	float elapsedTime = 0;

	Renderer exRenderer;
	Light exLight;

	float originalAlpha;
	float originalIntensity;

	void Start() {
		exRenderer = GetComponent<Renderer>();
		exLight = GetComponent<Light>();

		originalAlpha = exRenderer.material.color.a;
		originalIntensity = exLight.intensity;
	}

	void Update() {
		elapsedTime += Time.deltaTime;
		float lightness = 1 - elapsedTime / explosionTime;

		Color newColor = exRenderer.material.color;
		newColor.a = originalAlpha * lightness;
		exRenderer.material.color = newColor;

		exLight.intensity = originalIntensity * lightness;

		if (elapsedTime > explosionTime) {
			Destroy(gameObject);
		}
	}
}
