using UnityEngine;

public class DroidType : MonoBehaviour {
	public Weapon smallWeapon;
	public Weapon largeWeapon;

	int type = 1;

	public void SetType(int typeNumber) {
		type = typeNumber;

		GetComponentInChildren<Label>().SetLabel(type.ToString("D3"));

		GetComponent<Health>().SetMaxHealth(type / 100);

		DroidWeapon droidWeapon = GetComponent<DroidWeapon>();
		if (type == 200) {
			droidWeapon.weapon = smallWeapon;
			droidWeapon.cooldownTime = 1.5f;
		}
		else if (type == 300) {
			droidWeapon.weapon = largeWeapon;
			droidWeapon.cooldownTime = 0.75f;
		}
	}
}
