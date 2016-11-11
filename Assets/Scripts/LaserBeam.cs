using UnityEngine;

public class LaserBeam : Weapon {
	public float laserSpeed = 20f;
	public GameObject explosion;

	void FixedUpdate() {
		transform.position += transform.up * Time.deltaTime * laserSpeed;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Solid") {
			Destroy(gameObject);
		}
		else if ((other.tag == "Droid" || other.tag == "Player") && other.gameObject != origin) {
			Destroy(gameObject);
			Destroy(other.gameObject);
			Instantiate(explosion, other.transform.position, Quaternion.identity);
		}
	}
}
