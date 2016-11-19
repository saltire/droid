using UnityEngine;
using System.Collections.Generic;

public class Lift : MonoBehaviour {
	public int liftIndex;
	public List<Lift> otherLifts;

	public float cooldownTime = 0.5f;
	float enableTime = 0f;
	bool fireReleased = true;

	void Update() {
		if (!fireReleased && Input.GetAxis("Fire2") == 0) {
			fireReleased = true;
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Player" && fireReleased && Input.GetAxis("Fire2") > 0 && Time.time >= enableTime) {
			fireReleased = false;

			GameObject.Find("Map").GetComponent<Map>().ShowMap(GetComponent<Lift>());
		}
	}

	public void TemporarilyDisable() {
		enableTime = Time.time + cooldownTime;
	}
}
