using System.Collections.Generic;
using UnityEngine;

public class MinigameBuilder : MonoBehaviour {
	public GameObject Pulser;
	public GameObject WireDeadEnd;
	public GameObject WireSegment;
	public GameObject WireSplitter;

	public class PatternComponent {
		public string prefab;
		public int x = 0;
		public int y = 0;
		public float xScale = 1;
	};

	public static PatternComponent[][] patterns = new PatternComponent[][] {
		// Straight
		new PatternComponent[] {
			new PatternComponent {prefab = "WireSegment", x = 4, xScale = 8},
		},
		// // Dead end
		new PatternComponent[] {
			new PatternComponent {prefab = "WireSegment", x = 3, xScale = 6},
			new PatternComponent {prefab = "WireDeadEnd", x = 6, y = 0},
		},
		// Fork
		new PatternComponent[] {
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
		new PatternComponent[] {
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
		new PatternComponent[] {
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
		new PatternComponent[] {
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
		new PatternComponent[] {
			new PatternComponent {prefab = "WireSegment", x = 2, y = 0, xScale = 4},
			new PatternComponent {prefab = "WireSegment", x = 2, y = 2, xScale = 4},
			new PatternComponent {prefab = "WireSplitter", x = 4, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 6, y = 1, xScale = 4},

			new PatternComponent {prefab = "WireSegment", x = 1, y = 1, xScale = 2},
			new PatternComponent {prefab = "WireDeadEnd", x = 2, y = 1},
		},
		// Reverse fork closer to the end
		new PatternComponent[] {
			new PatternComponent {prefab = "WireSegment", x = 3, y = 0, xScale = 6},
			new PatternComponent {prefab = "WireSegment", x = 3, y = 2, xScale = 6},
			new PatternComponent {prefab = "WireSplitter", x = 6, y = 1},
			new PatternComponent {prefab = "WireSegment", x = 7, y = 1, xScale = 2},

			new PatternComponent {prefab = "WireSegment", x = 2, y = 1, xScale = 4},
			new PatternComponent {prefab = "WireDeadEnd", x = 4, y = 1},
		},
		// Ring, or a fork followed by a reverse fork
		new PatternComponent[] {
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
		new PatternComponent[] {
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
		new PatternComponent[] {
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
		new PatternComponent[] {
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

	public Dictionary<string, GameObject> GetPrefabs() {
		return new Dictionary<string, GameObject>() {
			{"Pulser", Pulser},
			{"WireDeadEnd", WireDeadEnd},
			{"WireSegment", WireSegment},
			{"WireSplitter", WireSplitter}
		};
	}
}
