using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapBuilder))]
public class MapBuilderEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		MapBuilder script = (MapBuilder)target;

		if (GUILayout.Button("Build Level")) {
			script.Build();
		}
	}
}
