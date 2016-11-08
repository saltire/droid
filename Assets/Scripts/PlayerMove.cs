using UnityEngine;

public class PlayerMove : MonoBehaviour {
	public float speed = 10f;
	public float rotateSpeed = 30f;
	Rigidbody rb;
	Quaternion targetRotation;

	void Start() {
		rb = GetComponent<Rigidbody>();
		targetRotation = transform.rotation;
	}

	void FixedUpdate() {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		Vector3 inputDirection = new Vector3(h, 0f, v);

		if (inputDirection != Vector3.zero) {
			rb.AddForce(inputDirection * speed);
			targetRotation = Quaternion.LookRotation(inputDirection);
		}
		if (transform.rotation != targetRotation) {
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
		}
	}
}
