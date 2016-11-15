using UnityEngine;

public class Lift : MonoBehaviour {
	public float cooldownTime = .5f;
	float lastFireTime = 0f;
	bool fireReleased = true;
	GameObject travellingPlayer = null;

	void FixedUpdate() {
		if (!fireReleased && Input.GetAxis("Fire2") == 0) {
			fireReleased = true;
		}

		if (travellingPlayer) {

		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Player" && fireReleased && !travellingPlayer && Input.GetAxis("Fire2") > 0 && Time.time > lastFireTime + cooldownTime) {
			lastFireTime = Time.time;
			fireReleased = false;
			travellingPlayer = other.gameObject;
		}
	}
}
