using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
	public float speed = 10f;
	public float rotateSpeed = 20f;
	Rigidbody rb;
	Quaternion targetRotation;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		targetRotation = transform.rotation;
	}
	
	void FixedUpdate ()
	{
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		if (h != 0f || v != 0f) {
			rb.AddForce (h * speed, 0f, v * speed);

			targetRotation = Quaternion.LookRotation (new Vector3 (h, 0f, v));
		}
		if (transform.rotation != targetRotation) {
			transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
		}
	}
}
