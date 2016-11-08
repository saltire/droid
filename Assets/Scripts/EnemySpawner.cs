using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
	public GameObject Enemy;
	public int enemyCount = 1;

	void Start() {
		GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
		enemyCount = Mathf.Min(enemyCount, waypoints.Length);

		// Spawn enemies on empty waypoints.
		List<int> placedPoints = new List<int>();
		while (placedPoints.Count < enemyCount)  {
			int point = Random.Range(0, waypoints.Length - 1);
			if (!placedPoints.Contains(point)) {
				placedPoints.Add(point);
				Instantiate(Enemy, waypoints [point].transform.position, Quaternion.identity);
			}
		}
	}
}
