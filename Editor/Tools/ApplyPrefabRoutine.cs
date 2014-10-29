using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ApplyPrefabRoutine : EditorWindow {

  [MenuItem ("Tools/ApplyPrefab #a")]
  static void init(){
    GameObject s = Selection.activeGameObject;
    if(s == null) return;
    apply(s);

    if(s.transform.childCount > 0){
      foreach(Transform t in s.transform){
        apply(t.gameObject);
      }
    }
  }

  //http://forum.unity3d.com/threads/apply-prefab-script.140705/
  static void apply(GameObject obj){
    Object o = PrefabUtility.GetPrefabParent(obj);
    Debug.Log(o);

    if(o != null){
      PrefabUtility.ReplacePrefab(obj, o, ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
    }

    Debug.Log("[TOOLS] applied prefab "+obj.name);
  }
}