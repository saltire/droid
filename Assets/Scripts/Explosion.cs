using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
	public float explosionTime = 1f;

	float elapsedTime = 0;

	Renderer render;
	Color originalColor;
	Color finalColor;

	void Start ()
	{
		render = GetComponent<Renderer> ();
		originalColor = render.material.color;
		finalColor = render.material.color;
		finalColor.a = 0;
	}
	
	void Update ()
	{
		elapsedTime += Time.deltaTime;

		render.material.color = Color.Lerp (originalColor, finalColor, elapsedTime / explosionTime);

		if (elapsedTime > explosionTime) {
			Destroy (gameObject);
		}
	}
}
