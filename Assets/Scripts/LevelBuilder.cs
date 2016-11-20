using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

public class LevelBuilder : MonoBehaviour {
	public GameObject Level;

	public GameObject Crate;
	public GameObject Door;
	public GameObject Energizer;
	public GameObject Floor;
	public GameObject Lift;
	public GameObject Wall;

	public GameObject PlayerStart;
	public GameObject Waypoint;

	public float heightInterval = 20f;

	public List<int> defaultDroidTypes;

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
			{23, Crate},
			{24, Wall},
			{25, Floor},
			{26, Floor},
			{27, Wall},
			{28, Wall},
			{29, Crate},
			{30, Crate},
			{31, Floor},
		};

		markerMap = new Dictionary<int, GameObject>() {
			{33, Waypoint},
			{34, PlayerStart},
		};

		// Remove any old levels.
		while (transform.childCount > 0) {
			DestroyImmediate(transform.GetChild(0).gameObject);
		}

		// Build a new level for each map.
		List<XmlDocument> mapDocs = GetMapDocs();
		for (int i = 0; i < mapDocs.Count; i++) {
			BuildLevel(mapDocs[i], heightInterval * i);
		}

		// Build the nav mesh.
		UnityEditor.NavMeshBuilder.BuildNavMesh();

		// Connect lifts between levels.
		ConnectLifts();

		// Disable all levels.
		foreach (Transform level in transform) {
			level.gameObject.SetActive(false);
		}
	}

	void BuildLevel(XmlDocument xmlDoc, float yOffset) {
		// Load the Tiled map tag from the XML document.
		XmlNode mapNode = xmlDoc.SelectSingleNode("map");
		int width = int.Parse(mapNode.Attributes["width"].Value);
		int height = int.Parse(mapNode.Attributes["height"].Value);
		string name = mapNode.SelectSingleNode("properties/property[@name=\"Name\"]/@value").Value;

		// Create a new level.
		GameObject level = (GameObject)Instantiate(Level, transform);
		level.name = name;

		// Place tiles and objects from the map file.
		int[] tiles = getLayerTileData(xmlDoc, "tiles");
		int[] markers = getLayerTileData(xmlDoc, "markers");
		for (int z = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				int i = (height - z - 1) * width + x;
				PlaceTile(tiles[i], markers[i], level, x, z);
			}
		}

		// Set the list of droid types.
		DroidSpawner spawner = level.GetComponent<DroidSpawner>();
		foreach (XmlNode property in mapNode.SelectNodes("properties/property")) {
			if (property.Attributes["name"].Value.Contains("Droid")) {
				for (int i = 0; i < int.Parse(property.Attributes["value"].Value); i++) {
					spawner.droidTypes.Add(int.Parse(property.Attributes["name"].Value.Substring(5, 3)));
				}
			}
		}
		if (spawner.droidTypes.Count == 0) {
			spawner.droidTypes = defaultDroidTypes;
		}

		// Move the level up.
		level.transform.Translate(Vector3.up * yOffset);
	}

	public List<XmlDocument> GetMapDocs() {
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

	void PlaceTile(int tileType, int markerType, GameObject level, int x, int z) {
		if (tileMap.ContainsKey(tileType)) {
			GameObject tilePrefab = tileMap[tileType];
			GameObject tile = (GameObject)Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity);
			tile.transform.parent = level.transform;

			// Tile-specific options

			if (tilePrefab == Door && tileType == 2) {
				// Rotate door.
				tile.transform.FindChild("Cylinder").RotateAround(tile.transform.position + new Vector3(0, 0.5f, 0), Vector3.up, 90f);
			}

			if (tilePrefab == Lift) {
				// Set lift index, to link it to other lifts.
				if (markerType >= 41 && markerType <= 48) {
					tile.GetComponent<Lift>().liftIndex = markerType - 41;
				}
			}
		}

		if (markerMap.ContainsKey(markerType)) {
			GameObject objPrefab = markerMap[markerType];
			GameObject obj = (GameObject)Instantiate(objPrefab, new Vector3(x, 0.5f, z), Quaternion.identity);
			obj.transform.parent = level.transform;
		}
	}

	void ConnectLifts() {
		List<Lift> lifts = new List<Lift>(GameObject.FindGameObjectsWithTag("Lift").Select(lift => lift.GetComponent<Lift>()));
		foreach (Lift lift in lifts) {
			lift.otherLifts = lifts.FindAll(otherLift => otherLift != lift && otherLift.liftIndex == lift.liftIndex);
		}
	}
}
