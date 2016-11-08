using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

public class MapBuilder : MonoBehaviour {
	public DefaultAsset tmxFile;
	GameObject map;
	Dictionary<int, GameObject> tileMap;

	public GameObject Crate;
	public GameObject Door;
	public GameObject Floor;
	public GameObject Wall;

	public GameObject Waypoint;
	public GameObject Player;

	public void Build() {
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

		XmlDocument xmlDoc = GetXmlDocument();
		XmlNode mapNode = xmlDoc.GetElementsByTagName("map")[0];
		int width = int.Parse(mapNode.Attributes["width"].Value);
		int height = int.Parse(mapNode.Attributes["height"].Value);

		// Remove any old maps and create a new one.

		Transform oldMap = transform.Find("Map");
		if (oldMap != null) {
			DestroyImmediate(oldMap.gameObject);
		}
		map = new GameObject("Map");
		map.transform.parent = transform;

		// Place tiles from the map file.

		int[] tiles = getLayerTileData(xmlDoc, "tiles");
		int[] markers = getLayerTileData(xmlDoc, "markers");

		for (int z = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				int i = (height - z - 1) * width + x;

				if (tiles [i] != 0) {
					PlaceTile(x, z, tiles [i]);
				}
				if (markers [i] == 33) {
					PlaceGameObject(Waypoint, x, z);
				}
				if (markers [i] == 34) {
					GameObject player = PlaceGameObject(Player, x, z);
					GameObject.FindWithTag("MainCamera").GetComponent<PlayerCamera>().player = player.transform;
				}
			}
		}

		// Build the nav mesh.

		UnityEditor.NavMeshBuilder.BuildNavMesh();
	}

	XmlDocument GetXmlDocument() {
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(File.ReadAllText(AssetDatabase.GetAssetPath(tmxFile)));
		return xmlDoc;
	}

	int[] getLayerTileData(XmlDocument xmlDoc, string layerName) {
		int[] tilesInt = {};
		foreach (XmlNode layer in xmlDoc.GetElementsByTagName("layer")) {
			if (layer.Attributes["name"].Value == layerName) {
				foreach (XmlNode child in layer.ChildNodes) {
					if (child.Name == "data") {
						string[] tilesStr = child.InnerText.Split(new string[] {","}, System.StringSplitOptions.None);
						tilesInt = tilesStr.Select(t => int.Parse(t)).ToArray();
					}
				}
			}
		}
		return tilesInt;
	}

	void PlaceTile(int x, int z, int tileType) {
		if (tileMap.ContainsKey(tileType)) {
			GameObject prefab = tileMap[tileType];
			GameObject tile = (GameObject)Instantiate(prefab, new Vector3(x, 0, z), Quaternion.identity);
			tile.transform.parent = map.transform;

			// Tile-specific options
			if (prefab == Door && tileType == 2) {
				tile.transform.FindChild("Cylinder").RotateAround(tile.transform.position + new Vector3(0, .5f, 0), Vector3.up, 90f);
			}
		}
	}

	GameObject PlaceGameObject (GameObject prefab, int x, int z) {
		GameObject obj = (GameObject)Instantiate(prefab, new Vector3(x, 0.5f, z), Quaternion.identity);
		obj.transform.parent = map.transform;
		return obj;
	}

	public List<int> GetDroidTypes() {
		List<int> droidTypes = new List<int>();

		foreach (XmlNode property in GetXmlDocument().GetElementsByTagName("properties")[0].ChildNodes) {
			if (property.Attributes["name"].Value.Substring(0, 5) == "Droid") {
				for (int i = 0; i < int.Parse(property.Attributes["value"].Value); i++) {
					droidTypes.Add(int.Parse(property.Attributes["name"].Value.Substring(5, 3)));
				}
			}
		}

		return droidTypes;
	}
}
