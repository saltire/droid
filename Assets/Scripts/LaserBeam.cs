using UnityEngine;
using System.Collections.Generic;

public class LaserBeam : Weapon {
	public float laserSpeed = 20f;

	List<Health> targetsHit = new List<Health>();

	void FixedUpdate() {
		transform.position += transform.forward * Time.deltaTime * laserSpeed;
	}

	public void OnChildCollision(Collider other) {
		if (other.gameObject != origin) {
			// Destroy the laser beam if it hits any non-trigger collider.
			if (!other.isTrigger) {
				Destroy(gameObject);
			}

			Health target = other.GetComponent<Health>();
			// Deal damage to each target once only.
			if (target != null && !targetsHit.Contains(target)) {
				targetsHit.Add(target);
				target.Damage(damage);
			}
		}
	}
}
