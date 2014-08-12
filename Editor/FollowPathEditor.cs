using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(FollowPath))]
public class FollowPathEditor : Editor {
  
  override public void OnInspectorGUI () {
    DrawDefaultInspector();

    if(Application.isPlaying) return;
    setObjectOnAverage();
  }
  
  void OnSceneGUI(){
    if(Application.isPlaying) return;
    
    //Debug.Log(obj.name);
    
    Transform agent = getAgent();
    if(agent == null) return;
    
    Handles.color = Color.magenta;
    drawHandle();
    if (GUI.changed)  EditorUtility.SetDirty (target);
  }

  void drawHandle(){
    FollowPath obj = (FollowPath)target;
    if(obj.path == null)  return; // do nothing if no path

    for(int i = 0; i < obj.path.Length; i++){
      obj.path[i] = Handles.PositionHandle(obj.path[i], Quaternion.identity);
      //obj.path[i] = EditorGUILayout.Vector3Field ("node#"+i, obj.path[i]);
    }

  }

  Transform getAgent(){
    FollowPath obj = (FollowPath)target;
    Transform agent = obj.agent;
    if(agent == null) agent = obj.transform;
    if(obj.path == null)  return null; // do nothing if no path

    return agent;
  }

  void setObjectOnAverage(){
    FollowPath obj = (FollowPath)target;
    Transform agent = getAgent();

    agent.transform.position = getPathAverage();
    obj.transform.position = getPathAverage();
  }

  Vector3 getPathAverage(){
    FollowPath obj = (FollowPath)target;
    if(obj.path == null) return Vector3.zero;
    
    //mettre l'objet au centre des points
    Vector3 average = Vector3.zero;
    foreach(Vector3 pt in obj.path){
      average.x += pt.x;
      average.y += pt.y;
      average.z += pt.z;
    }
    average.x /= obj.path.Length;
    average.y /= obj.path.Length;
    average.z /= obj.path.Length;

    return average;
  }
}