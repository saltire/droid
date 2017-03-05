using UnityEngine;

public class LaserBeam : Weapon {
	public float laserSpeed = 20f;

	void FixedUpdate() {
		transform.position += transform.forward * Time.deltaTime * laserSpeed;
	}
}
