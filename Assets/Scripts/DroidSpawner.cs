using UnityEngine;
using System.Collections.Generic;

public class DroidSpawner : MonoBehaviour {
	public GameObject Droid;

	public List<int> droidTypes = new List<int>();

	void Start () {
		// Spawn enemies on empty waypoints in each level.
		List<GameObject> waypoints = new List<GameObject>();
		foreach (Transform child in transform) {
			if (child.tag == "Waypoint") {
				waypoints.Add(child.gameObject);
			}
		}
		List<int> placedPoints = new List<int>();

		foreach (int droidType in droidTypes) {
			if (placedPoints.Count == waypoints.Count) {
				continue;
			}

			int point;
			do {
				point = Random.Range(0, waypoints.Count);
			}
			while (placedPoints.Contains(point));
			placedPoints.Add(point);

			GameObject droid = (GameObject)Instantiate(Droid, waypoints[point].transform.position, Quaternion.identity);
			droid.transform.parent =  waypoints[point].transform.parent;
			droid.GetComponent<DroidType>().SetDroidType(droidType);
		}
	}
}
