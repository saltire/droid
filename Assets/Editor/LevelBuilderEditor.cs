using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelBuilder))]
public class LevelBuilderEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		LevelBuilder script = (LevelBuilder)target;

		if (GUILayout.Button("Build Level")) {
			script.Build();
		}
	}
}
