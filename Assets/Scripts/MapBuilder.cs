using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

public class MapBuilder : MonoBehaviour
{
	public TextAsset mapFile;

	public GameObject Wall;
	public GameObject Floor;

	static int[] wallChars = {4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 28, 29};
	static int[] floorChars = {1, 2, 3, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 30};

	public void Build ()
	{
		XmlDocument xmlDoc = new XmlDocument ();
		xmlDoc.LoadXml (mapFile.text);

		XmlNode levelTag = xmlDoc.GetElementsByTagName ("layer") [0];
		int width = int.Parse (levelTag.Attributes ["width"].Value);
		int height = int.Parse (levelTag.Attributes ["height"].Value);

		XmlNode dataTag = xmlDoc.GetElementsByTagName ("data") [0];
		string[] tiles = dataTag.InnerText.Split (new string[] {","}, System.StringSplitOptions.None);

		Transform oldMap = transform.Find ("Map");
		if (oldMap != null) {
			DestroyImmediate (oldMap.gameObject);
		}

		GameObject map = new GameObject ("Map");
		map.transform.parent = transform;

		for (int i = 0; i < tiles.Length; i++) {
			int tileType = int.Parse (tiles [i]);
			if (tileType == 0) {
				continue;
			}

			tileType -= 1;

			int x = i % width;
			int z = height - 1 - (i / width);

			GameObject prefab = null;
			if (wallChars.Contains (tileType)) {
				prefab = Wall;
			} else if (floorChars.Contains (tileType)) {
				prefab = Floor;
			}

			if (prefab != null) {
				GameObject tile = (GameObject)Instantiate (prefab, new Vector3 (x, 0, z), Quaternion.identity);
				tile.transform.parent = map.transform;
			}
		}
	}
}
