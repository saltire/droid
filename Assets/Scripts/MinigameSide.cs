using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MinigameSide : MonoBehaviour {
	public Color color = Color.yellow;

	public GameObject WireDeadEnd;
	public GameObject WireSegment;
	public GameObject WireSplitter;

	float offsetX = -132.5f;
	float offsetY = -39;
	float rowHeight = -11;
	float xScale = 15;

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
			new PatternComponent {prefab = "WireSegment", x = 4, xScale = 8},
		},
		// // Dead end
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 3, xScale = 6},
			new PatternComponent {prefab = "WireDeadEnd", x = 6, y = 0},
		},
		// Fork
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 2, y = 1, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 6, y = 0, xScale = 4},
			new PatternComponent {prefab = "WireSegment", x = 6, y = 2, xScale = 4},

			new PatternComponent {prefab = "WireSegment", x = 1, y = 0, xScale = 2},
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 0},
			new PatternComponent {prefab = "WireSegment", x = 1, y = 2, xScale = 2},
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 2},
		},
		// Fork closer to the end
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 3, y = 1, xScale = 6},
			new PatternComponent {prefab = "WireSplitter", x = 6, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 7, y = 0, xScale = 2},
			new PatternComponent {prefab = "WireSegment", x = 7, y = 2, xScale = 2},

			new PatternComponent {prefab = "WireSegment", x = 2, y = 0, xScale = 4},
			new PatternComponent {prefab = "WireDeadEnd", x = 4, y = 0},
			new PatternComponent {prefab = "WireSegment", x = 2, y = 2, xScale = 4},
			new PatternComponent {prefab = "WireDeadEnd", x = 4, y = 2},
		},
		// Fork with a dead end at the top
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 2, y = 1, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 5, y = 0, xScale = 2},
			new PatternComponent {prefab = "WireDeadEnd", x = 6, y = 0},
			new PatternComponent {prefab = "WireSegment", x = 6, y = 2, xScale = 4},

			new PatternComponent {prefab = "WireSegment", x = 1, y = 0, xScale = 2},
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 0},
			new PatternComponent {prefab = "WireSegment", x = 1, y = 2, xScale = 2},
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 2},
		},
		// Fork with a dead end at the bottom
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 2, y = 1, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 6, y = 0, xScale = 4},
			new PatternComponent {prefab = "WireSegment", x = 5, y = 2, xScale = 2},
			new PatternComponent {prefab = "WireDeadEnd", x = 6, y = 2},

			new PatternComponent {prefab = "WireSegment", x = 1, y = 0, xScale = 2},
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 0},
			new PatternComponent {prefab = "WireSegment", x = 1, y = 2, xScale = 2},
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 2},
		},
		// Reverse fork
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 2, y = 0, xScale = 4},
			new PatternComponent {prefab = "WireSegment", x = 2, y = 2, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 6, y = 1, xScale = 4},

			new PatternComponent {prefab = "WireSegment", x = 1, y = 1, xScale = 2},
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 1},
		},
		// Reverse fork closer to the end
		new List<PatternComponent>() {
			new PatternComponent {prefab = "WireSegment", x = 3, y = 0, xScale = 6},
			new PatternComponent {prefab = "WireSegment", x = 3, y = 2, xScale = 6},
			new PatternComponent {prefab = "WireSplitter", x = 6, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 7, y = 1, xScale = 2},

			new PatternComponent {prefab = "WireSegment", x = 2, y = 1, xScale = 4},
			new PatternComponent {prefab = "WireDeadEnd", x = 4, y = 1},
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
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 0},
			new PatternComponent {prefab = "WireSegment", x = 1, y = 2, xScale = 2},
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 2},
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
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 1},
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
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 1},
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
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 1, y = 3, xScale = 2},
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 3},
			new PatternComponent {prefab = "WireSegment", x = 1, y = 5, xScale = 2},
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 5},
		},
	};

	public void Build() {
		prefabs = new Dictionary<string, GameObject>() {
			{"WireDeadEnd", WireDeadEnd},
			{"WireSegment", WireSegment},
			{"WireSplitter", WireSplitter}
		};

		// Place patterns.
		int row = 0;
		while (row < 12) {
			List<PatternComponent> pattern = patterns[Random.Range(0, patterns.Count)];
			int height = Mathf.Max(pattern.Select(component => component.y).ToArray()) + 1;
			if (row + height <= 12) {
				PlacePattern(pattern, row);
				row += height;
			}
		}

		// Set color of power source and (testing) pulsers.
		foreach (Transform child in transform) {
			if (child.name == "Source") {
				child.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
			}
			if (child.tag == "Pulser") {
				child.GetComponent<PoweredComponent>().color = color;
			}
		}
	}

	void PlacePattern(List<PatternComponent> pattern, int row) {
		foreach (PatternComponent pc in pattern) {
			GameObject comp = (GameObject)Instantiate(prefabs[pc.prefab], transform.position, Quaternion.identity);
			comp.transform.parent = transform;
			comp.transform.localPosition += new Vector3(pc.x * xScale + offsetX, (pc.y + row) * rowHeight + offsetY, 0);
			comp.transform.localScale = Vector3.Scale(comp.transform.localScale, new Vector3(pc.xScale, 1, 1));
		}
	}
}
