using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
	public Transform door;
	public float doorSpeed = 4f;

	int droidsInRange = 0;
	Vector3 closedPosition;
	Vector3 openPosition;

	void Start ()
	{
		closedPosition = door.position;
		openPosition = door.position + door.transform.up;
	}

	void FixedUpdate ()
	{
		if (droidsInRange > 0 && transform.position != openPosition) {
			door.position = Vector3.Lerp (door.position, openPosition, Time.deltaTime * doorSpeed);
		} else if (droidsInRange == 0 && transform.position != closedPosition) {
			door.position = Vector3.Lerp (door.position, closedPosition, Time.deltaTime * doorSpeed);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Droid") {
			droidsInRange++;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Droid") {
			droidsInRange--;
		}
	}
}
