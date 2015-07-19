using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{
	public Transform player;
	public float cameraSpeed = 10f;
	Vector3 targetCameraPosition;
	Vector3 cameraDistance;
	
	void Start ()
	{
		cameraDistance = transform.position - player.position;
	}

	// Use LateUpdate in case any other effects happen this frame.
	// Also enable rigidbody interpolation on the player to avoid jitter.
	void LateUpdate ()
	{
		targetCameraPosition = player.position + cameraDistance;
		transform.position = Vector3.Lerp (transform.position, targetCameraPosition, Time.deltaTime * cameraSpeed);
	}
}
