using UnityEngine;

public class PlayerWeapon : MonoBehaviour {
	public Weapon weapon;

	public float cooldownTime = .5f;
	float enableTime = 0f;
	bool fireReleased = true;

	void FixedUpdate() {
		if (!fireReleased && Input.GetAxis("Fire1") == 0) {
			fireReleased = true;
		}

		if (fireReleased && Input.GetAxis("Fire1") > 0 && Time.time > enableTime) {
			enableTime = Time.time + cooldownTime;
			fireReleased = false;

			Weapon weaponInstance = (Weapon)Instantiate(weapon, transform.position, transform.rotation * Quaternion.Euler(90, 0, 0));
			weaponInstance.origin = gameObject;
		}
	}
}
