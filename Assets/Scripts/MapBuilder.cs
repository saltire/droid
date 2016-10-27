using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

public class MapBuilder : MonoBehaviour
{
	public TextAsset mapFile;
	XmlDocument xmlDoc;

	GameObject map;

	public GameObject Crate;
	public GameObject Door;
	public GameObject Floor;
	public GameObject Wall;

	Dictionary<int, GameObject> tileMap;

	public GameObject Waypoint;
	
	public void Build () {
		tileMap = new Dictionary<int, GameObject>() {
			{2, Door},
			{3, Door},
			{4, Floor},
			{5, Wall},
			{6, Wall},
			{7, Wall},
			{8, Wall},
			{9, Wall},
			{10, Wall},
			{11, Wall},
			{12, Wall},
			{13, Wall},
			{14, Wall},
			{15, Wall},
			{16, Wall},
			{17, Floor},
			{18, Floor},
			{19, Floor},
			{20, Floor},
			{21, Floor},
			{22, Floor},
			{23, Floor},
			{24, Floor},
			{25, Floor},
			{26, Floor},
			{29, Crate},
			{30, Crate},
			{31, Floor},
		};

		// Load the Tiled map file.

		xmlDoc = new XmlDocument ();
		xmlDoc.LoadXml (mapFile.text);

		XmlNode mapNode = xmlDoc.GetElementsByTagName ("map") [0];
		int width = int.Parse (mapNode.Attributes ["width"].Value);
		int height = int.Parse (mapNode.Attributes ["height"].Value);

		// Remove any old maps and create a new one.

		Transform oldMap = transform.Find ("Map");
		if (oldMap != null) {
			DestroyImmediate (oldMap.gameObject);
		}
		map = new GameObject ("Map");
		map.transform.parent = transform;

		// Place tiles from the map file.

		int[] tiles = getLayerTileData ("tiles");
		int[] waypoints = getLayerTileData ("waypoints");

		for (int z = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				int i = (height - z - 1) * width + x;

				if (tiles [i] != 0) {
					PlaceTile (x, z, tiles [i]);
				}
				if (waypoints [i] != 0) {
					PlaceWaypoint (x, z);
				}
			}
		}

		// Build the nav mesh.

		UnityEditor.NavMeshBuilder.BuildNavMesh();
	}

	int[] getLayerTileData (string layerName) {
		int[] tilesInt = {};
		foreach (XmlNode layer in xmlDoc.GetElementsByTagName ("layer")) {
			if (layer.Attributes ["name"].Value == layerName) {
				foreach (XmlNode child in layer.ChildNodes) {
					if (child.Name == "data") {
						string[] tilesStr = child.InnerText.Split (new string[] {","}, System.StringSplitOptions.None);
						tilesInt = tilesStr.Select (t => int.Parse (t)).ToArray ();
					}
				}
			}
		}
		return tilesInt;
	}

	void PlaceTile (int x, int z, int tileType) {
		if (tileMap.ContainsKey(tileType)) {
			GameObject prefab = tileMap[tileType];
			GameObject tile = (GameObject)Instantiate (prefab, new Vector3 (x, 0, z), Quaternion.identity);
			tile.transform.parent = map.transform;
			
			// Tile-specific options
			if (prefab == Door && tileType == 2) {
				tile.transform.FindChild ("Cylinder").RotateAround (tile.transform.position + new Vector3 (0, .5f, 0), Vector3.up, 90f);
			}
		}
	}

	void PlaceWaypoint (int x, int z) {
		GameObject waypoint = (GameObject)Instantiate (Waypoint, new Vector3 (x, 0, z), Quaternion.identity);
		waypoint.transform.parent = map.transform;
	}
}
