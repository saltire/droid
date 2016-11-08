using UnityEngine;

public class DroidType : MonoBehaviour {
	int type = 1;

	public void SetType(int typeNumber) {
		type = typeNumber;

		GetComponentInChildren<Label>().SetLabel(type.ToString("D3"));
	}
}
