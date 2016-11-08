using UnityEngine;

public class Label : MonoBehaviour {
	public void SetLabel(string label) {
		GetComponent<TextMesh>().text = label;
	}

	void LateUpdate() {
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 1, transform.rotation.eulerAngles.z);
	}
}
