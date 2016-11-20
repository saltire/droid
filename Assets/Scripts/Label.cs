using UnityEngine;

public class Label : MonoBehaviour {
	public void SetLabel(int type) {
		GetComponent<TextMesh>().text = type.ToString("D3");
	}

	void LateUpdate() {
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 1, transform.rotation.eulerAngles.z);
	}
}
