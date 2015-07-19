using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
	public float laserSpeed = 20f;

	void FixedUpdate ()
	{
		transform.position += transform.up * Time.deltaTime * laserSpeed;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Solid") {
			Destroy (gameObject);
		}
	}
}
