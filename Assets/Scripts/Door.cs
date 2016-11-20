using UnityEngine;
using System.Collections.Generic;

public class Door : MonoBehaviour {
	public Transform door;
	public float doorSpeed = 4f;

	HashSet<Collider> droids = new HashSet<Collider>();
	Vector3 closedPosition;
	Vector3 openPosition;

	void Start() {
		// If there isn't a wall in the opening direction, rotate 180 degrees so the door opens the other way.
		if (Physics.OverlapSphere(door.position + door.transform.up, 0.4f)[0].transform.parent.name != "Wall(Clone)") {
			door.transform.Rotate(new Vector3(180, 0, 0));
		}

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

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Droid" || other.tag == "Player")  {
			droids.Add(other);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Droid" || other.tag == "Player")  {
			droids.Remove(other);
		}
	}
}
