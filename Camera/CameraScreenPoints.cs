using UnityEngine;
using System.Collections;

public class CameraScreenPoints : MonoBehaviour {
  
  public Vector2 screenSize = Vector2.zero;
  public Vector2 pixelSize = Vector2.zero;
  
  public Vector3 center = Vector3.zero;
  public Vector3 bottomRight = Vector3.zero;
  public Vector3 topLeft = Vector3.zero;
  public Rect bounds = new Rect();

  public bool scaleScreen = false;

  void Start(){
    updateCamera();
  }

  void updatePosition(){
    Vector2 worldCenter = Vector2.zero;
    float ratio = pixelSize.x / pixelSize.y; // screen ratio => 1.775, 1.5, 1.33333, etc
    worldCenter.x = Camera.main.orthographicSize * ratio; worldCenter.y = -Camera.main.orthographicSize;
    transform.position = new Vector3(worldCenter.x, -worldCenter.y, 0f);
  }
  
  void updateOrtho(){

    screenSize.x = Screen.width;
    screenSize.y = Screen.height;

    if(!scaleScreen){
      pixelSize.x = Camera.main.pixelWidth;
      pixelSize.y = Camera.main.pixelHeight;
    }

    Camera.main.orthographicSize = pixelSize.y * 0.5f * (1f / 100f);
  }

  public void updateCamera(){
    updateOrtho();
    updatePosition();
    
    center.x = Camera.main.transform.position.x;
    center.y = Camera.main.transform.position.y;
    bottomRight.x = center.x * 2f;
    bottomRight.y = -center.y * 2f;
    topLeft.x = -center.x * 2f;
    topLeft.y = center.y * 2f;

    bounds.width = bottomRight.x;
    bounds.height = bottomRight.y;
  }

  void Update(){
    updateCamera();
  }

  void OnDrawGizmos(){
    float size = 0.1f;
    Gizmos.color = Color.cyan;
    Gizmos.DrawSphere(center, size);
    Gizmos.DrawSphere(bottomRight, size);
    Gizmos.DrawSphere(topLeft, size);
  }
  
  public Rect getBounds(){ return bounds; }
}