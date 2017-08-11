using UnityEngine;

public class DroidType : MonoBehaviour {
	public Weapon Disruptor;
	public Weapon LaserBeam;
	public Weapon LaserBeamDouble;
	public Weapon Sabre;

	int type = 1;

	public class DroidStats {
		public Weapon weapon = null;
		public float cooldownTime = 0;
	};

	public DroidStats GetDroidStats(int type) {
		DroidStats stats = new DroidStats();

		if (type == 200) {
			stats.weapon = LaserBeam;
			stats.cooldownTime = 1f;
		}
		else if (type == 300) {
			stats.weapon = LaserBeamDouble;
			stats.cooldownTime = 0.75f;
		}
		else if (type == 400) {
			stats.weapon = Sabre;
		}
		else if (type == 500) {
			stats.weapon = Disruptor;
		}

		return stats;
	}

	public int GetDroidType() {
		return type;
	}

	public void SetDroidType(int typeNumber) {
		type = typeNumber;

		GetComponentInChildren<Label>().SetLabel(type);

		GetComponent<Health>().SetMaxHealth(type / 100);

		DroidStats stats = GetDroidStats(type);
		DroidWeapon droidWeapon = GetComponent<DroidWeapon>();
		droidWeapon.weapon = stats.weapon;
		droidWeapon.cooldownTime = stats.cooldownTime;
	}
}
