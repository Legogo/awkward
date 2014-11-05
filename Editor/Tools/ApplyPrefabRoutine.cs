using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Apply prefab routine.
/// http://docs.unity3d.com/ScriptReference/MenuItem.html
/// http://forum.unity3d.com/threads/apply-prefab-script.140705/
/// 
/// permet d'apply tout les prefabs enfant de l'objet selectionné
/// </summary>

public class ApplyPrefabRoutine : EditorWindow {
  
  [MenuItem ("Tools/Apply child prefab(s) %#a")]
  static void init(){
    GameObject s = Selection.activeGameObject;
    if(s == null) return;

    if(s.transform.childCount <= 0){

      //seulement l'objet selectionné
      apply(s);

    }else {

      //les enfants de l'objet selectionné
      foreach(Transform t in s.transform){
        apply(t.gameObject);
      }

    }
  }

  static protected void apply(GameObject obj){
    Object o = PrefabUtility.GetPrefabParent(obj);
    
    if(o != null){
      PrefabUtility.ReplacePrefab(obj, o, ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
    }

    Debug.Log("[EDTIOR-TOOLS] applied prefab "+obj.name);
  }
}