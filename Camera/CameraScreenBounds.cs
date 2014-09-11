using UnityEngine;
using System.Collections;

/// <summary>
/// Camera screen bounds.
/// Solve bottom left / top right / center corner
/// </summary>

public class CameraScreenBounds : MonoBehaviour {
  
  public Vector3 bottomLeft;
  public Vector3 topRight;
  public Vector3 centerPoint;

  Camera cam;

  void Start(){
    cam = Camera.main;
    update();
  }
  
  void update(){
    //get camera z
    Vector3 pos = Vector3.zero;
    pos.z = cam.nearClipPlane;
    
    bottomLeft = cam.ScreenToWorldPoint(pos);
    
    pos.x = Screen.width;
    pos.y = Screen.height;
    
    topRight = cam.ScreenToWorldPoint(pos);

    centerPoint = bottomLeft + ((topRight - bottomLeft) * 0.5f);
  }
}