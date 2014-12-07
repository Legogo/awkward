using UnityEngine;
using System.Collections;

public class ScreenPoints : MonoBehaviour {
  static public ScreenPoints manager;

  public Vector3 bottomLeft;
  public Vector3 topRight;
  public Vector3 centerPoint;

  Camera cam;
  
  void Awake() { manager = this; cam = Camera.main; }
  void Update(){ updatePoints(); }
  
  public void updatePoints(){
    if(cam == null) cam = Camera.main;
    
    if(cam.orthographic){
      bottomLeft = cam.transform.position - (cam.transform.right * cam.orthographicSize * cam.aspect) - (cam.transform.up * cam.orthographicSize);
      topRight = cam.transform.position - (-cam.transform.right * cam.orthographicSize * cam.aspect) + (cam.transform.up * cam.orthographicSize);
      centerPoint = cam.transform.position;
    }else{
      bottomLeft = cam.ScreenToWorldPoint(new Vector3(0,0,cam.farClipPlane));
      topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth,cam.pixelHeight,cam.farClipPlane));
      centerPoint = bottomLeft + ((topRight - bottomLeft) * 0.5f);
    }
    
  }
  
  public bool DEBUG_GIZMO = false;
  void OnDrawGizmos(){
    if(!DEBUG_GIZMO)  return;
    Gizmos.color = Color.green;
    
    updatePoints();
    Gizmos.color = Color.red;
    Gizmos.DrawSphere(bottomLeft, 1f);
    Gizmos.DrawSphere(centerPoint, 1f);
    Gizmos.DrawSphere(topRight, 1f);
    
  }

  public Vector3 getScreenWorldSize(){
    return new Vector3(
      topRight.x - bottomLeft.x,
      topRight.y - bottomLeft.y,
      topRight.z - bottomLeft.z
      );
  }

  static public bool outOfScreen(GameObject go){
    Vector3 position = go.transform.position;
    
    if(position.x < ScreenPoints.manager.bottomLeft.x || position.x > ScreenPoints.manager.topRight.x) return true;
    if(position.z < ScreenPoints.manager.bottomLeft.z || position.z > ScreenPoints.manager.topRight.z)  return true;
    
    return false;
  }
  
  static public Vector2 getOutsidePosition(){
    ScreenPoints.manager.updatePoints();
    
    Vector2 pos = Vector2.zero;
    if(Random.seed > 0.5) pos.x = ScreenPoints.manager.topRight.x + 5f;
    else pos.x = ScreenPoints.manager.bottomLeft.x - 5f;
    
    pos.y = Random.Range(-6f, 6f);
    return pos;
  }
  
  static public Vector2 getInsidePosition(){
    ScreenPoints.manager.updatePoints();
    
    Vector2 pos = Vector2.zero;
    pos.x = Random.Range(-10f, 10f);
    pos.y = Random.Range(-6f, 6f);
    return pos;
  }
  
}
