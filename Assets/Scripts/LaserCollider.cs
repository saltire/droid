using UnityEngine;

public class LaserCollider : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		transform.parent.GetComponent<LaserBeam>().OnChildCollision(other);
	}
}
