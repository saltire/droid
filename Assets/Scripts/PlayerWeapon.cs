using UnityEngine;

public class PlayerWeapon : MonoBehaviour {
	public Weapon defaultWeapon;

	Weapon weapon;
	float cooldownTime;

	float enableTime = 0f;
	bool fireReleased = true;

	void Start() {
		SetWeapon(defaultWeapon, defaultWeapon.defaultCooldownTime);
	}

	public void SetWeapon(Weapon newWeapon, float newCooldownTime) {
		weapon = newWeapon != null ? newWeapon : defaultWeapon;
		cooldownTime = newCooldownTime > 0 ? newCooldownTime : defaultWeapon.defaultCooldownTime;
	}

	void Update() {
		if (!fireReleased && Input.GetAxis("Fire") == 0) {
			fireReleased = true;
		}

		if (fireReleased && Input.GetAxis("Fire") > 0 && Time.time > enableTime) {
			enableTime = Time.time + cooldownTime;
			fireReleased = false;

			Weapon weaponInstance = (Weapon)Instantiate(weapon, transform.position, transform.rotation);
			weaponInstance.origin = gameObject;
		}
	}
}
