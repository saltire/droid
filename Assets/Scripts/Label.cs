using UnityEngine;

public class Label : MonoBehaviour {
	public string text;

	public void SetLabel(int type) {
		text = type.ToString("D3");
		GetComponent<TextMesh>().text = text;
	}

	void LateUpdate() {
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 1, transform.rotation.eulerAngles.z);
	}
}
