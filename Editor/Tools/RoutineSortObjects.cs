using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class RoutineSortObjects : EditorWindow {
  
  [MenuItem ("Tools/Nodes tools")]
  static void init(){
    EditorWindow.GetWindow(typeof(ApplyPrefabRoutine));
  }
  
  void OnEnabled(){
    // called on window creation
  }
  
  void OnGUI(){
    /*
    GUILayout.Label("label");
    EditorGUILayout.LabelField("label left", "label right");
    EditorGUILayout.Separator();
    EditorGUILayout.ObjectField("Title", objectHandle, typeof(objectClassName), true);
    */
    if (GUILayout.Button("Re-order nodes")){
      reorder();
    }
  }
  
  void reorder(){
    GameObject select = Selection.activeGameObject;
    if(select == null)  return;
    
    if(select.name.IndexOf("_path") > -1){
      List<Transform> children = new List<Transform>();
      
      for (int i = 1; i < 50; i++) {
        foreach(Transform c in select.transform){
          if(c.name.IndexOf("_"+i) > -1){
            children.Add(c);
          }
        }
      }
      
      for (int i = 0; i < children.Count; i++) {
        children[i].transform.parent = null;
        children[i].transform.parent = select.transform;
      }
      //Debug.Log(children.Count);
      //children.ToArray().orOrder();
    }
  }
}
