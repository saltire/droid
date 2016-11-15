using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

public class MapBuilder : MonoBehaviour {
	public GameObject Crate;
	public GameObject Door;
	public GameObject Energizer;
	public GameObject Floor;
	public GameObject Lift;
	public GameObject Wall;

	public GameObject PlayerStart;
	public GameObject Waypoint;

	public float heightInterval = 20f;

	Dictionary<int, GameObject> tileMap;
	Dictionary<int, GameObject> markerMap;

	public void Build() {
		tileMap = new Dictionary<int, GameObject>() {
			{2, Door},
			{3, Door},
			{4, Lift},
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
			{21, Energizer},
			{22, Floor},
			{23, Floor},
			{24, Floor},
			{25, Floor},
			{26, Floor},
			{29, Crate},
			{30, Crate},
			{31, Floor},
		};

		markerMap = new Dictionary<int, GameObject>() {
			{33, Waypoint},
			{34, PlayerStart},
		};

		// Remove any old maps.
		while (transform.childCount > 0) {
			DestroyImmediate(transform.GetChild(0).gameObject);
		}

		// Build a new map for each level.
		List<XmlDocument> levels = GetLevels();
		for (int i = 0; i < levels.Count; i++) {
			BuildLevel(levels[i], heightInterval * i);
		}

		// Build the nav mesh.
		UnityEditor.NavMeshBuilder.BuildNavMesh();
	}

	void BuildLevel(XmlDocument xmlDoc, float yOffset) {
		// Load the Tiled map tag from the XML document.
		XmlNode mapNode = xmlDoc.GetElementsByTagName("map")[0];
		int width = int.Parse(mapNode.Attributes["width"].Value);
		int height = int.Parse(mapNode.Attributes["height"].Value);

		// Create a new map.
		GameObject map = new GameObject(mapNode.SelectSingleNode("properties/property[@name=\"Name\"]/@value").Value);
		map.transform.parent = transform;

		// Place tiles and objects from the map file.
		int[] tiles = getLayerTileData(xmlDoc, "tiles");
		int[] markers = getLayerTileData(xmlDoc, "markers");
		for (int z = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				int i = (height - z - 1) * width + x;
				PlaceTile(tiles[i], markers[i], map, x, z);
			}
		}

		// Move the map up.
		map.transform.Translate(Vector3.up * yOffset);
	}

	public List<XmlDocument> GetLevels() {
		string[] guids = AssetDatabase.FindAssets("", new string[] {"Assets/Maps"});

		return new List<XmlDocument>(guids.Select(guid => {
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(File.ReadAllText(AssetDatabase.GUIDToAssetPath(guid)));
			return xmlDoc;
		}));
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

	void PlaceTile(int tileType, int markerType, GameObject map, int x, int z) {
		if (tileMap.ContainsKey(tileType)) {
			GameObject tilePrefab = tileMap[tileType];
			GameObject tile = (GameObject)Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity);
			tile.transform.parent = map.transform;

			// Tile-specific options

			if (tilePrefab == Door && tileType == 2) {
				// Rotate door.
				tile.transform.FindChild("Cylinder").RotateAround(tile.transform.position + new Vector3(0, 0.5f, 0), Vector3.up, 90f);
			}

			if (tilePrefab == Lift) {
				// Set lift shaft, to link it to other lifts.
				if (markerType >= 41 && markerType <= 48) {
					tile.GetComponent<Lift>().shaft = markerType - 41;
				}
			}
		}

		if (markerMap.ContainsKey(markerType)) {
			GameObject objPrefab = markerMap[markerType];
			GameObject obj = (GameObject)Instantiate(objPrefab, new Vector3(x, 0.5f, z), Quaternion.identity);
			obj.transform.parent = map.transform;
		}
	}

	public List<int> GetDroidTypes(XmlDocument xmlDoc) {
		List<int> droidTypes = new List<int>();

		foreach (XmlNode property in xmlDoc.SelectNodes("map/properties/property")) {
			if (property.Attributes["name"].Value.Contains("Droid")) {
				for (int i = 0; i < int.Parse(property.Attributes["value"].Value); i++) {
					droidTypes.Add(int.Parse(property.Attributes["name"].Value.Substring(5, 3)));
				}
			}
		}

		return droidTypes;
	}
}
