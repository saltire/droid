using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class DroidSpawner : MonoBehaviour {
	public GameObject Player;
	public GameObject Droid;

	void Start() {
		// Place player on a random start point.
		GameObject[] startPoints = GameObject.FindGameObjectsWithTag("PlayerStart");
		GameObject startPoint = startPoints[Random.Range(0, startPoints.Length)];
		GameObject player = (GameObject)Instantiate(Player, startPoint.transform.position, Quaternion.identity);
		player.transform.parent = startPoint.transform.parent;

		// Place camera.
		GameObject.FindWithTag("MainCamera").GetComponent<PlayerCamera>().LookAtPlayer(player.transform);

		// Spawn enemies on empty waypoints in each level.
		foreach (XmlDocument xmlDoc in GetComponent<LevelBuilder>().GetMaps()) {
			XmlNode mapNode = xmlDoc.GetElementsByTagName("map")[0];
			string levelName = mapNode.SelectSingleNode("properties/property[@name=\"Name\"]/@value").Value;
			GameObject level = transform.Find(levelName).gameObject;

			List<GameObject> waypoints = new List<GameObject>();
			foreach (Transform child in level.transform) {
				if (child.tag == "Waypoint") {
					waypoints.Add(child.gameObject);
				}
			}
			List<int> placedPoints = new List<int>();

			foreach (int droidType in GetComponent<LevelBuilder>().GetDroidTypes(xmlDoc)) {
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
				droid.GetComponent<DroidType>().SetType(droidType);
			}
		}
	}
}
