using UnityEngine;

public class Sabre : Weapon {
	public float duration = 0.25f;
	public float windupArcLength = 180f;
	public float swingArcLength = 360f;

	float elapsed = 0f;
	float startAngle;
	float direction;
	float totalArcLength;
	Transform cylinder;
	Transform sphere;
	Vector3 initialScale;
	TrailRenderer trail;

	void Start() {
		transform.parent = origin.transform;

		direction = transform.rotation.eulerAngles.y > 180 ? -1 : 1;
		startAngle = transform.rotation.eulerAngles.y - windupArcLength * direction;
		totalArcLength = swingArcLength + windupArcLength * 2;

		cylinder = transform.FindChild("Cylinder");
		sphere = transform.FindChild("Sphere");
		initialScale = cylinder.localScale;
		trail = GetComponentInChildren<TrailRenderer>();

		transform.rotation = Quaternion.Euler(0, startAngle, 0);
		SetLength(0);
	}

	void Update() {
		elapsed += Time.deltaTime;
		if (elapsed > duration) {
			Destroy(gameObject);
		}

		float angle = elapsed / duration * totalArcLength;
		transform.rotation = Quaternion.Euler(0, startAngle + angle * direction, 0);

		float length = Mathf.Min(angle, totalArcLength - angle, windupArcLength) / windupArcLength;
		SetLength(length);
	}

	void SetLength(float length) {
		cylinder.localScale = new Vector3(initialScale.x, initialScale.y * length, initialScale.z);
		cylinder.localPosition = Vector3.forward * initialScale.y * length;
		sphere.localPosition = Vector3.forward * initialScale.y * length * 2;
		// trail.transform.localPosition = Vector3.forward * initialScale.y * length;
		trail.widthMultiplier = initialScale.y * length * 2;
	}
}
