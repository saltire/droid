using UnityEngine;

public class DroidType : MonoBehaviour {
	public Weapon smallWeapon;

	int type = 1;

	public void SetType(int typeNumber) {
		type = typeNumber;

		GetComponentInChildren<Label>().SetLabel(type.ToString("D3"));

		GetComponent<Health>().SetMaxHealth(type / 100);

		if (type >= 200) {
			GetComponent<DroidWeapon>().weapon = smallWeapon;
		}
	}
}
