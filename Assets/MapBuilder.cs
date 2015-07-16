using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

public class MapBuilder : MonoBehaviour
{
	public TextAsset mapFile;

	public Transform Wall;
	public Transform Floor;

	static int[] wallChars = {4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 28, 29};
	static int[] floorChars = {1, 2, 3, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 30};

	void Start ()
	{
		XmlDocument xmlDoc = new XmlDocument ();
		xmlDoc.LoadXml (mapFile.text);

		XmlNode levelTag = xmlDoc.GetElementsByTagName ("layer") [0];
		int width = int.Parse (levelTag.Attributes ["width"].Value);
		int height = int.Parse (levelTag.Attributes ["height"].Value);

		XmlNode dataTag = xmlDoc.GetElementsByTagName ("data") [0];
		string[] tiles = dataTag.InnerText.Split (new string[] {","}, System.StringSplitOptions.None);

		for (int i = 0; i < tiles.Length; i++) {
			int tile = int.Parse (tiles [i]);
			int x = i % width;
			int z = height - 1 - (i / width);

			if (wallChars.Contains (tile)) {
				Instantiate (Wall, new Vector3 (x, 0, z), Quaternion.identity);
			} else if (floorChars.Contains (tile)) {
				Instantiate (Floor, new Vector3 (x, 0, z), Quaternion.identity);
			}
		}
	}
	
	void Update ()
	{
	
	}
}
