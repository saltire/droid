using UnityEngine;
using System.Collections.Generic;

public class Energizer : MonoBehaviour {
	public float tickHealTime = 0.75f;

	Dictionary<Collider, float> healTimes = new Dictionary<Collider, float>();

	void OnTriggerStay(Collider other) {
		if (other.tag == "Droid" || other.tag == "Player")  {
			if (!healTimes.ContainsKey(other)) {
				healTimes[other] = tickHealTime;
			}
			else {
				healTimes[other] -= Time.deltaTime;
				if (healTimes[other] <= 0) {
					other.GetComponent<Health>().Heal(1);
					healTimes[other] += tickHealTime;
				}
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Droid" || other.tag == "Player")  {
			healTimes.Remove(other);
		}
	}
}
