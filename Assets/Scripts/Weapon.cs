using UnityEngine;
using System.Collections.Generic;

public abstract class Weapon : MonoBehaviour {
	[HideInInspector]
	public GameObject origin;
	public int damage = 1;
	public float defaultCooldownTime = 0.5f;
	public bool destroyOnHit = true;

	protected List<Health> targetsHit = new List<Health>();

	public virtual void OnTriggerCollision(Collider other) {
		if (other.gameObject != origin) {
			// Optionally destroy if it hits any non-trigger collider.
			if (!other.isTrigger && destroyOnHit) {
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
