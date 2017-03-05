using UnityEngine;

public class Weapon : MonoBehaviour {
	[HideInInspector]
	public GameObject origin;
	public int damage = 1;
	public float defaultCooldownTime = 0.5f;
}
