using UnityEngine;
using System.Collections.Generic;

public class Minigame : MonoBehaviour {
	public Transform source;
	public Color color = Color.yellow;

	public GameObject WireSegment;
	public GameObject WireSplitter;

	List<PoweredComponent> startSegments = new List<PoweredComponent>();

	class PatternComponent {
		public GameObject prefab;
		public float x = 0;
		public float y = 0;
		public float xScale = 1;
	};

	Dictionary<string, List<PatternComponent>> patterns;

	float offsetX = 19.5f;
	float offsetY = -26;
	float rowHeight = -12;
	float xScale = 15;

	void Start () {
		patterns = new Dictionary<string, List<PatternComponent>>() {
			{"Straight", new List<PatternComponent>() {
				new PatternComponent {prefab = WireSegment, x = 4, xScale = 8}
			}},
			{"Fork", new List<PatternComponent>() {
				new PatternComponent {prefab = WireSegment, x = 2, y = 1, xScale = 4},
				new PatternComponent {prefab = WireSplitter, x = 4, y = 1},
				new PatternComponent {prefab = WireSegment, x = 6, y = 0, xScale = 4},
				new PatternComponent {prefab = WireSegment, x = 6, y = 2, xScale = 4},
				new PatternComponent {prefab = WireSegment, x = 1, y = 0, xScale = 2},
				new PatternComponent {prefab = WireSegment, x = 1, y = 2, xScale = 2},
			}},
			{"Reverse Fork", new List<PatternComponent>() {
				new PatternComponent {prefab = WireSegment, x = 6, y = 1, xScale = 4},
				new PatternComponent {prefab = WireSplitter, x = 4, y = 1},
				new PatternComponent {prefab = WireSegment, x = 2, y = 0, xScale = 4},
				new PatternComponent {prefab = WireSegment, x = 2, y = 2, xScale = 4},
				new PatternComponent {prefab = WireSegment, x = 1, y = 1, xScale = 2},
			}},
			{"Dead End", new List<PatternComponent>() {
				new PatternComponent {prefab = WireSegment, x = 3, xScale = 6}
			}},
		};

		PlacePattern(patterns["Straight"], 0);
		PlacePattern(patterns["Fork"], 1);
		PlacePattern(patterns["Reverse Fork"], 4);
		PlacePattern(patterns["Dead End"], 7);
		PlacePattern(patterns["Reverse Fork"], 8);
		PlacePattern(patterns["Straight"], 11);

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
			GameObject comp = (GameObject)Instantiate(pc.prefab, transform.position + new Vector3(offsetX, (pc.y + row) * rowHeight + offsetY, 0), Quaternion.identity);
			comp.transform.localScale = Vector3.Scale(comp.transform.localScale, new Vector3(pc.xScale, 1, 1));
			comp.transform.position += new Vector3(pc.x * xScale, 0, 0);
			comp.transform.parent = transform;
		}
	}
}
