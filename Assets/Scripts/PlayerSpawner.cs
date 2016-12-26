using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour {
	public GameObject Player;
	public GameObject Droid;

	void Start() {
		// Pick a random start point and enable its level.
		List<Transform> startPoints = new List<Transform>();
		foreach (Transform level in transform) {
			foreach (Transform obj in level) {
				if (obj.tag == "PlayerStart") {
					startPoints.Add(obj);
				}
			}
		}
		Transform startPoint = startPoints[Random.Range(0, startPoints.Count)];
		startPoint.parent.gameObject.SetActive(true);

		// Place the player on the start point.
		GameObject player = (GameObject)Instantiate(Player, startPoint.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
		player.transform.parent = startPoint.parent;

		// Place camera.
		GameObject.FindWithTag("MainCamera").GetComponent<PlayerCamera>().LookAtPlayer(player.transform);
	}
}
