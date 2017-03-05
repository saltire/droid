using UnityEngine;

public class WeaponCollider : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		transform.parent.GetComponent<Weapon>().OnTriggerCollision(other);
	}
}
