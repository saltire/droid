using UnityEngine;
using System.Collections.Generic;

public class Minigame : MonoBehaviour {
	public Transform source;
	public Color color = Color.yellow;

	List<PoweredComponent> startSegments = new List<PoweredComponent>();

	void Start () {
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
}
