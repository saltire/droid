using UnityEngine;

public class DroidWeapon : MonoBehaviour {
	public Weapon weapon;
	public float cooldownTime = 0.75f;

	float enableTime = 0f;

	public void OnPlayerInRange(Collider player) {
		if (weapon && (Time.time > enableTime)) {
			Vector3 playerDir = player.transform.position - transform.position;
			RaycastHit hitInfo;
			if (Physics.Raycast(transform.position, playerDir, out hitInfo, 10f) && hitInfo.collider.tag == "Player") {
				enableTime = Time.time + cooldownTime;

				Weapon weaponInstance = (Weapon)Instantiate(weapon, transform.position, Quaternion.LookRotation(playerDir));
				weaponInstance.origin = gameObject;
			}
		}
	}
}
