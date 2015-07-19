using UnityEngine;
using System.Collections;

public class Laser : Weapon
{
	public float laserSpeed = 20f;
	public GameObject explosion;

	void FixedUpdate ()
	{
		transform.position += transform.up * Time.deltaTime * laserSpeed;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Solid") {
			Destroy (gameObject);
		} else if (other.tag == "Droid" && other.gameObject != origin) {
			Instantiate (explosion, other.transform.position, Quaternion.identity);
			Destroy (other.gameObject);
		}
	}
}
