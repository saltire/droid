using UnityEngine;

public class DetectPlayer : MonoBehaviour {
	void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			GetComponentInParent<DroidWeapon>().OnPlayerInRange(other);
		}
	}
}
