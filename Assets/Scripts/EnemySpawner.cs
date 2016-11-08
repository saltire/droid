using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
	public GameObject Droid;

	void Start() {
		// Spawn enemies on empty waypoints.

		GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
		List<int> placedPoints = new List<int>();

		foreach (int droidType in GetComponent<MapBuilder>().GetDroidTypes()) {
			if (placedPoints.Count == waypoints.Length) {
				continue;
			}

			int point;
			do {
				point = Random.Range(0, waypoints.Length - 1);
			}
			while (placedPoints.Contains(point));
			placedPoints.Add(point);

			GameObject droid = (GameObject)Instantiate(Droid, waypoints [point].transform.position, Quaternion.identity);
			droid.GetComponent<DroidType>().SetType(droidType);
		}
	}
}
