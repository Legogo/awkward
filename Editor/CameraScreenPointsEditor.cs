using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CameraScreenPoints))]
public class CameraScreenPointsEditor : Editor {
	
	override public void OnInspectorGUI () {
		DrawDefaultInspector();

		if (GUILayout.Button("Update camera")){
      CameraScreenPoints t = (CameraScreenPoints)target;
      t.updateCamera();
		}
	}
}