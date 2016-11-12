using UnityEngine;

public class Health : MonoBehaviour {
	public int maxHealth = 5;
	public GameObject explosion;

	int health;
	HealthBar healthBar;

	void Start() {
		SetMaxHealth(maxHealth);
	}

	public void SetMaxHealth(int newMaxHealth) {
		maxHealth = newMaxHealth;
		health = maxHealth;

		healthBar = GetComponentInChildren<HealthBar>();
		if (healthBar) {
			healthBar.UpdateHealth(health);
		}
	}

	public void Heal(int healing) {
		if (health < maxHealth) {
			health = Mathf.Min(health + healing, maxHealth);

			if (healthBar) {
				healthBar.UpdateHealth(health);
			}
		}
	}

	public void Damage(int damage) {
		health -= damage;

		if (healthBar) {
			healthBar.UpdateHealth(health);
		}

		if (health <= 0) {
			Destroy(gameObject);
			Instantiate(explosion, transform.position, Quaternion.identity);
		}
	}
}
