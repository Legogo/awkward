using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class RoutineSortObjects : EditorWindow {
  
  [MenuItem ("Tools/Sort objects %#w")]
  static void init(){
    GameObject s = Selection.activeGameObject;

    if(s == null) return;

    //Debug.Log(s.transform.childCount+" objects to sort");
    order(s.transform);
  }

  static void order(Transform t){

    List<Transform> children = new List<Transform>();

    foreach(Transform child in t){ children.Add(child); }
    for (int i = 0; i < children.Count; i++) {
      children[i].parent = null;
    }

    children.Sort(CompareListByName);

    for (int i = 0; i < children.Count; i++) {
      children[i].parent = t;
    }

    //Debug.Log("Sorted "+children.Count+" objects");
  }

  private static int CompareListByName(Transform i1, Transform i2)
  {
    return i1.name.CompareTo(i2.name); 
  }
}