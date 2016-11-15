using UnityEngine;

public class PlayerCamera : MonoBehaviour {
	public float cameraSpeed = 10f;
	Vector3 targetCameraPosition;
	Vector3 cameraDistance;

	Transform player;
	public float distance = 15f;
	public float angle = 80f;

	public void LookAtPlayer(Transform playerTransform) {
		player = playerTransform;

		cameraDistance = Quaternion.Euler(angle, 0, 0) * -Vector3.forward * distance;
		transform.position = player.position + cameraDistance;
		transform.LookAt(player);
	}

	// Use LateUpdate in case any other effects happen this frame.
	// Also enable rigidbody interpolation on the player to avoid jitter.
	void LateUpdate() {
		if (player) {
			targetCameraPosition = player.position + cameraDistance;
			transform.position = Vector3.Lerp(transform.position, targetCameraPosition, Time.deltaTime * cameraSpeed);
		}
	}
}
