using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(FollowPath))]
public class FollowPathEditor : Editor {
  
  override public void OnInspectorGUI () {
    DrawDefaultInspector();

    if(Application.isPlaying) return;
    //setObjectOnAverage();
    setObjectOnCenter();
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

  /* function doesn't work if object is in hierarchy */
  void drawHandle(){
    FollowPath obj = (FollowPath)target;
    if(obj.path == null)  return; // do nothing if no path

    for(int i = 0; i < obj.path.Length; i++){
      obj.path[i] = Handles.PositionHandle(obj.getNodePosition(i), Quaternion.identity) - obj.transform.position;
      //obj.path[i] = EditorGUILayout.Vector3Field ("node#"+i, obj.path[i]);
    }

  }

  Transform getAgent(){
    FollowPath obj = (FollowPath)target;
    Transform agent = obj.agent;
    return agent;
  }

  void setObjectOnCenter(){
    FollowPath obj = (FollowPath)target;
    getAgent().transform.position = obj.transform.position;
  }

  void setObjectOnAverage(){
    Transform agent = getAgent();
    if(agent != null) agent.transform.position = getPathAverage();
  }

  Vector3 getPathAverage(){
    FollowPath obj = (FollowPath)target;
    if(obj.path == null) return Vector3.zero;
    
    //mettre l'objet au centre des points
    Vector3 average = Vector3.zero;
    if(obj.path.Length < 1) return Vector3.zero;
    for (int i = 0; i < obj.path.Length; i++) {
      Vector3 pt = obj.getNodePosition(i);
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