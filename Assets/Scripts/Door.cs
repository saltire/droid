using UnityEngine;
using System.Collections.Generic;

public class Door : MonoBehaviour {
	public Transform door;
	public float doorSpeed = 4f;

	HashSet<Collider> droids = new HashSet<Collider>();
	Vector3 closedPosition;
	Vector3 openPosition;

	void Start() {
		closedPosition = door.position;
		openPosition = door.position + door.transform.up;
	}

	void FixedUpdate() {
		droids.RemoveWhere(droid => droid.Equals(null));

		if (droids.Count > 0 && transform.position != openPosition)  {
			door.position = Vector3.Lerp(door.position, openPosition, Time.deltaTime * doorSpeed);
		}
		else if (droids.Count == 0 && transform.position != closedPosition)  {
			door.position = Vector3.Lerp(door.position, closedPosition, Time.deltaTime * doorSpeed);
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Droid")  {
			droids.Add(other);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Droid")  {
			droids.Remove(other);
		}
	}
}
