using UnityEngine;
using System.Collections.Generic;

public class Influence : MonoBehaviour {
	public float flashSpeed = 3;
	public Color flashColor = Color.red;

	Minigame minigame;
	List<Material> materials = new List<Material>();
	List<DroidType> droids = new List<DroidType>();
	DroidType hackedDroid;

	float holdTime = 0;
	int type = 1;

	void Start() {
		minigame = GameObject.Find("Minigame").GetComponent<Minigame>();
		foreach (Renderer renderer in transform.Find("Body").GetComponentsInChildren<Renderer>()) {
			materials.Add(renderer.material);
		}
	}

	void Update() {
		if (holdTime > 0 && Input.GetAxis("Influence") == 0) {
			holdTime = 0;

			foreach (Material material in materials) {
				material.SetColor("_EmissionColor", Color.black);
			}
		}
		else if (Input.GetAxis("Influence") > 0) {
			holdTime += Time.deltaTime;
			float mult = Mathf.PingPong(holdTime * flashSpeed, 1);
			Color color = new Color(flashColor.r * mult, flashColor.g * mult, flashColor.b * mult);

			foreach (Material material in materials) {
				material.SetColor("_EmissionColor", color);
			}
		}
	}

	void FixedUpdate() {
		if (holdTime > 0) {
			droids = droids.FindAll(droid => !droid.Equals(null));

			if (droids.Count > 0) {
				hackedDroid = droids[0];
				minigame.StartMinigame(type, hackedDroid.GetDroidType());
			}
		}
	}

	public void OnHackSuccess() {
		// Get the hacked droid's stats and assign them to the player.
		type = hackedDroid.GetDroidType();
		GetComponentInChildren<Label>().SetLabel(type);
		DroidType.DroidStats stats = hackedDroid.GetDroidStats(type);
		GetComponent<PlayerWeapon>().SetWeapon(stats.weapon, stats.cooldownTime);

		// Remove the hacked droid.
		Destroy(hackedDroid.gameObject);
		droids.Remove(hackedDroid);
		hackedDroid = null;
	}

	public void OnHackFailure() {
		Label label = GetComponentInChildren<Label>();

		if (type == 1) {
			// If at the lowest level, explode the player and the hacked droid.
			GetComponent<Health>().Kill();
			hackedDroid.GetComponent<Health>().Kill();
		}
		else {
			// If not, demote to the lowest level.
			type = 1;
			label.SetLabel(1);
			DroidType.DroidStats stats = hackedDroid.GetDroidStats(type);
			GetComponent<PlayerWeapon>().SetWeapon(stats.weapon, stats.cooldownTime);

			// Remove the hacked droid.
			Destroy(hackedDroid.gameObject);
		}

		droids.Remove(hackedDroid);
		hackedDroid = null;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Droid")  {
			droids.Add(collision.gameObject.GetComponent<DroidType>());
		}
	}

	void OnCollisionExit(Collision collision) {
		if (collision.gameObject.tag == "Droid")  {
			droids.Remove(collision.gameObject.GetComponent<DroidType>());
		}
	}
}
