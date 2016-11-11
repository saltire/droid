using UnityEngine;

public class DroidWeapon : MonoBehaviour {
	public Weapon weapon;
	public float cooldownTime = .5f;
	float lastFireTime = 0f;

	public void OnPlayerInRange(Collider player) {
		if (weapon && (Time.time > lastFireTime + cooldownTime)) {
			Vector3 playerDir = player.transform.position - transform.position;
			RaycastHit hitInfo;
			if (Physics.Raycast(transform.position, playerDir, out hitInfo, 10f) && hitInfo.collider.tag == "Player") {
				lastFireTime = Time.time;

				Weapon weaponInstance = (Weapon)Instantiate(weapon, transform.position, Quaternion.LookRotation(playerDir) * Quaternion.Euler(90, 0, 0));
				weaponInstance.origin = gameObject;
			}
		}
	}
}
