using UnityEngine;
using System.Collections.Generic;

public class Lift : MonoBehaviour {
	public int shaft;
	public List<Lift> otherLifts;

	public float cooldownTime = 0.5f;
	float enableTime = 0f;
	bool fireReleased = true;

	void FixedUpdate() {
		if (!fireReleased && Input.GetAxis("Fire2") == 0) {
			fireReleased = true;
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Player" && fireReleased && Input.GetAxis("Fire2") > 0 && Time.time >= enableTime) {
			fireReleased = false;

			if (otherLifts.Count > 0) {
				Lift otherLift = otherLifts[Random.Range(0, otherLifts.Count)];
				otherLift.TemporarilyDisable();
				TemporarilyDisable();

				otherLift.transform.parent.gameObject.SetActive(true);
				other.GetComponent<Rigidbody>().velocity = Vector3.zero;
				other.transform.parent = otherLift.transform.parent;
				other.transform.position = otherLift.transform.position + new Vector3(0, 0.5f, 0);
				transform.parent.gameObject.SetActive(false);
			}
		}
	}

	public void TemporarilyDisable() {
		enableTime = Time.time + cooldownTime;
	}
}
