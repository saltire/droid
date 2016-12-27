using UnityEngine;
using System.Collections.Generic;

public class LaserBeam : Weapon {
	public float laserSpeed = 20f;
	public int damage = 1;

	List<Health> targetsHit = new List<Health>();

	void FixedUpdate() {
		transform.position += transform.forward * Time.deltaTime * laserSpeed;
	}

	public void OnChildCollision(Collider other) {
		if (other.gameObject != origin) {
			if (!other.isTrigger) {
				Destroy(gameObject);
			}

			if ((other.tag == "Droid" || other.tag == "Player")) {
				Health target = other.GetComponent<Health>();
				if (!targetsHit.Contains(target)) {
					targetsHit.Add(target);
					target.Damage(damage);
				}
			}
		}
	}
}
