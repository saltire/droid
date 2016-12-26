using UnityEngine;
using UnityEditor;
using UnityEditor.AI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

public class LevelBuilder : MonoBehaviour {
	public GameObject Level;

	public float heightInterval = 20f;

	public List<int> defaultDroidTypes;

	class TileData {
		public GameObject prefab;
		public int rotation;
	}

	Dictionary<int, TileData> tileMap;

	public void Build() {
		tileMap = GetTileSet("Assets/Maps/tiles.tsx");

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
		NavMeshBuilder.BuildNavMesh();

		// Connect lifts between levels.
		ConnectLifts();

		// Disable all levels.
		foreach (Transform level in transform) {
			level.gameObject.SetActive(false);
		}
	}

	Dictionary<int, TileData> GetTileSet(string path) {
		// Get a dictionary of tile prefabs, indexed by name.
		Dictionary<string, GameObject> tiles = new Dictionary<string, GameObject>();
		foreach (string guid in AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/Tiles"})) {
			GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(GameObject));
			tiles.Add(prefab.name, prefab);
		}

		// Read the tileset file and return a map of tile prefabs, indexed by their tile id.
		Dictionary<int, TileData> tileMap = new Dictionary<int, TileData>();
		XmlDocument tileset = new XmlDocument();
		tileset.LoadXml(File.ReadAllText(path));
		foreach (XmlNode tile in tileset.GetElementsByTagName("tile")) {
			XmlNode rotation = tile.SelectSingleNode("properties/property[@name=\"rotation\"]/@value");
			tileMap.Add(int.Parse(tile.Attributes["id"].Value), new TileData() {
				prefab = tiles[tile.SelectSingleNode("properties/property[@name=\"name\"]/@value").Value],
				rotation = rotation == null ? 0 : int.Parse(rotation.Value),
			});
		}
		return tileMap;
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
		return new List<XmlDocument>(AssetDatabase.FindAssets("", new string[] {"Assets/Maps"})
			.Select(guid => AssetDatabase.GUIDToAssetPath(guid))
			.Where(path => path.Contains(".tmx"))
			.Select(path => {
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(File.ReadAllText(path));
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
		// Tile numbers in map files have an offset from tile ids in tileset files.
		int offset = 1;

		// Instantiate prefabs from the tile layer.
		if (tileMap.ContainsKey(tileType - offset)) {
			TileData tileData = tileMap[tileType - offset];
			GameObject tile = (GameObject)Instantiate(tileData.prefab, new Vector3(x, 0, z), Quaternion.identity);
			tile.transform.parent = level.transform;

			if (tileData.rotation > 0) {
				tile.transform.RotateAround(tile.transform.position, Vector3.up, 90f);
			}

			// Tile-specific options

			if (tileData.prefab.name == "Lift") {
				// Set lift index, to link it to other lifts.
				if (markerType >= 41 && markerType <= 48) {
					tile.GetComponent<Lift>().liftIndex = markerType - 41;
				}
			}
		}

		// Instantiate prefabs from the marker layer.
		if (tileMap.ContainsKey(markerType - offset)) {
			TileData tileData = tileMap[markerType - offset];
			GameObject tile = (GameObject)Instantiate(tileData.prefab, new Vector3(x, 0, z), Quaternion.identity);
			tile.transform.parent = level.transform;
		}
	}

	void ConnectLifts() {
		List<Lift> lifts = new List<Lift>(GameObject.FindGameObjectsWithTag("Lift").Select(lift => lift.GetComponent<Lift>()));
		foreach (Lift lift in lifts) {
			lift.otherLifts = lifts.FindAll(otherLift => otherLift != lift && otherLift.liftIndex == lift.liftIndex);
		}
	}
}
