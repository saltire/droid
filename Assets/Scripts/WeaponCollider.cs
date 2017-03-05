using UnityEngine;

public class WeaponCollider : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		transform.parent.GetComponent<Weapon>().OnTriggerEnter(other);
	}

	void OnTriggerExit(Collider other) {
		transform.parent.GetComponent<Weapon>().OnTriggerExit(other);
	}
}
