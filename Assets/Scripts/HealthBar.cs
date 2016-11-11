using UnityEngine;

public class HealthBar : MonoBehaviour {
	public GameObject tick;
	public float width = 0.5f;
	public float zOffset = -0.2f;

	float spacing;

	void Start() {
		spacing = width / (GetComponentInParent<Health>().maxHealth - 1);
	}

	void LateUpdate() {
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 1, transform.rotation.eulerAngles.z);
	}

	public void UpdateHealth(int health) {
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}

		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 1, transform.rotation.eulerAngles.z);
		Vector3 firstTickPosition = transform.position - new Vector3(width / 2, 0, 0);

		for (int i = 0; i < health; i++) {
			GameObject tickInstance = (GameObject)Instantiate(tick, firstTickPosition + new Vector3(spacing * i, 0, zOffset), Quaternion.identity);
			tickInstance.transform.parent = transform;
		}
	}
}
