using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Minigame : MonoBehaviour {
	public Transform source;
	public Color color = Color.yellow;

	public GameObject WireSegment;
	public GameObject WireSplitter;

	List<PoweredComponent> startSegments = new List<PoweredComponent>();

	Dictionary<string, GameObject> prefabs;

	class PatternComponent {
		public string prefab;
		public int x = 0;
		public int y = 0;
		public float xScale = 1;
	};

	List<List<PatternComponent>> patterns = new List<List<PatternComponent>>() {
		// Straight
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 4, xScale = 8}
		},
		// // Dead end
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 3, xScale = 6}
		},
		// Fork
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 2, y = 1, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 6, y = 0, xScale = 4},
			new PatternComponent {prefab = "WireSegment", x = 6, y = 2, xScale = 4},

			new PatternComponent {prefab = "WireSegment", x = 1, y = 0, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 1, y = 2, xScale = 2},
		},
		// Fork closer to the end
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 3, y = 1, xScale = 6},
			new PatternComponent {prefab = "WireSplitter", x = 6, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 7, y = 0, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 7, y = 2, xScale = 2},

			new PatternComponent {prefab = "WireSegment", x = 2, y = 0, xScale = 4},
			new PatternComponent {prefab = "WireSegment", x = 2, y = 2, xScale = 4},
		},
		// Fork with a dead end at the top
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 2, y = 1, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 5, y = 0, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 6, y = 2, xScale = 4},

			new PatternComponent {prefab = "WireSegment", x = 1, y = 0, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 1, y = 2, xScale = 2},
		},
		// Fork with a dead end at the bottom
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 2, y = 1, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 6, y = 0, xScale = 4},
			new PatternComponent {prefab = "WireSegment", x = 5, y = 2, xScale = 2},

			new PatternComponent {prefab = "WireSegment", x = 1, y = 0, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 1, y = 2, xScale = 2},
		},
		// Reverse fork
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 2, y = 0, xScale = 4},
			new PatternComponent {prefab = "WireSegment", x = 2, y = 2, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 6, y = 1, xScale = 4},

			new PatternComponent {prefab = "WireSegment", x = 1, y = 1, xScale = 2},
		},
		// Reverse fork closer to the end
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 3, y = 0, xScale = 6},
			new PatternComponent {prefab = "WireSegment", x = 3, y = 2, xScale = 6},
			new PatternComponent {prefab = "WireSplitter", x = 6, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 7, y = 1, xScale = 2},

			new PatternComponent {prefab = "WireSegment", x = 2, y = 1, xScale = 4},
		},
		// Ring, or a fork followed by a reverse fork
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 2, y = 1, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 5, y = 0, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 5, y = 2, xScale = 2},
			new PatternComponent {prefab = "WireSplitter", x = 6, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 7, y = 1, xScale = 2},

			new PatternComponent {prefab = "WireSegment", x = 1, y = 0, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 1, y = 2, xScale = 2},
		},
		// Reverse fork followed by a fork
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 2, y = 0, xScale = 4},
			new PatternComponent {prefab = "WireSegment", x = 2, y = 2, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 5, y = 1, xScale = 2},
			new PatternComponent {prefab = "WireSplitter", x = 6, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 7, y = 0, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 7, y = 2, xScale = 2},

			new PatternComponent {prefab = "WireSegment", x = 1, y = 1, xScale = 2},
		},
		// Reverse branching fork
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 2, y = 0, xScale = 4},
			new PatternComponent {prefab = "WireSegment", x = 2, y = 2, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 5, y = 1, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 3, y = 3, xScale = 6},
			new PatternComponent {prefab = "WireSplitter", x = 6, y = 2},
			new PatternComponent {prefab = "WireSegment", x = 7, y = 2, xScale = 2},

			new PatternComponent {prefab = "WireSegment", x = 1, y = 1, xScale = 2},
		},
		// Reverse fork with a branching fork below it
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 2, y = 0, xScale = 4},
			new PatternComponent {prefab = "WireSegment", x = 2, y = 2, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 6, y = 1, xScale = 4},

			new PatternComponent {prefab = "WireSegment", x = 2, y = 4, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 4},
			new PatternComponent {prefab = "WireSegment", x = 5, y = 3, xScale = 2},
			new PatternComponent {prefab = "WireSplitter", x = 6, y = 3},
			new PatternComponent {prefab = "WireSegment", x = 7, y = 2, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 7, y = 4, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 6, y = 5, xScale = 4},

			new PatternComponent {prefab = "WireSegment", x = 1, y = 1, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 1, y = 3, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 1, y = 5, xScale = 2},
		},
	};

	float offsetX = 19.5f;
	float offsetY = -26;
	float rowHeight = -12;
	float xScale = 15;

	void Start () {
		prefabs = new Dictionary<string, GameObject>() {
			{"WireSegment", WireSegment},
			{"WireSplitter", WireSplitter}
		};

		int row = 0;
		while (row < 12) {
			List<PatternComponent> pattern = patterns[Random.Range(0, patterns.Count)];
			int height = Mathf.Max(pattern.Select(component => component.y).ToArray()) + 1;
			if (row + height <= 12) {
				PlacePattern(pattern, row);
				row += height;
			}
		}

		Time.timeScale = 0;

		source.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);

		foreach (Collider other in Physics.OverlapBox(source.position + new Vector3(source.localScale.x / 2, 0, 0), new Vector3(0.5f, source.localScale.y / 2, 0.5f))) {
			if (other.transform != source && other.tag == "WireSegment") {
				startSegments.Add(other.GetComponent<PoweredComponent>());
			}
		}
	}

	void Update () {
		foreach (PoweredComponent startSegment in startSegments) {
			startSegment.TransmitPower();
		}
	}

	void PlacePattern(List<PatternComponent> pattern, int row) {
		foreach (PatternComponent pc in pattern) {
			GameObject comp = (GameObject)Instantiate(prefabs[pc.prefab], transform.position + new Vector3(offsetX, (pc.y + row) * rowHeight + offsetY, 0), Quaternion.identity);
			comp.transform.localScale = Vector3.Scale(comp.transform.localScale, new Vector3(pc.xScale, 1, 1));
			comp.transform.position += new Vector3(pc.x * xScale, 0, 0);
			comp.transform.parent = transform;
		}
	}
}
