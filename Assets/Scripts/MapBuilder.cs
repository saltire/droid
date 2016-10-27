using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

public class MapBuilder : MonoBehaviour
{
	public TextAsset mapFile;
	XmlDocument xmlDoc;

	GameObject map;

	public GameObject Door;
	public GameObject Floor;
	public GameObject Wall;
	static int[] doorChars = {2, 3};
	static int[] floorChars = {4, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 31};
	static int[] wallChars = {5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 29, 30};

	public GameObject Waypoint;
	
	int[] getLayerTileData (string layerName)
	{
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

	public void Build ()
	{
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

		UnityEditor.NavMeshBuilder.BuildNavMesh();
	}

	void PlaceTile (int x, int z, int tileType)
	{
		GameObject prefab = null;
		if (doorChars.Contains (tileType)) {
			prefab = Door;
		} else if (floorChars.Contains (tileType)) {
			prefab = Floor;
		} else if (wallChars.Contains (tileType)) {
			prefab = Wall;
		}
		
		if (prefab != null) {
			GameObject tile = (GameObject)Instantiate (prefab, new Vector3 (x, 0, z), Quaternion.identity);
			tile.transform.parent = map.transform;
			
			// Tile-specific options
			if (prefab == Door && tileType == 2) {
				tile.transform.FindChild ("Cylinder").RotateAround (tile.transform.position + new Vector3 (0, .5f, 0), Vector3.up, 90f);
			}
		}
	}

	void PlaceWaypoint (int x, int z)
	{
		GameObject waypoint = (GameObject)Instantiate (Waypoint, new Vector3 (x, 0, z), Quaternion.identity);
		waypoint.transform.parent = map.transform;
	}
}
