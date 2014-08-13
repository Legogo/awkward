using UnityEngine;
using System.Collections;

public class RelativePosition : MonoBehaviour {

  public RelativePosition parent;

  [HideInInspector]public Transform t;
  public Vector2 screenPosition;

  int IPAD = 0;
  int IP5 = 1;
  public Vector2[] screenPositionCorrections;
  //public Vector2 screenSizeRef;

  Vector2 relativePosition = Vector2.zero;
  Vector2 pixelPosition = Vector2.zero;
  Vector3 worldPosition = Vector3.zero;

  int frames = 5;

  void Update(){
    frames--;
    updatePosition();
    if(frames < 0) enabled = false;
  }

  void updateRef(){
    t = transform;
  }


  void updatePosition(){
    updateRef();
    getPosition(); // update relative (+based on parent positioning)

    pixelPosition.x = Camera.main.pixelWidth * relativePosition.x;
    pixelPosition.y = Camera.main.pixelHeight * relativePosition.y;

    Vector2 diff = getDiff();
    pixelPosition.x += diff.x * Camera.main.pixelWidth;
    pixelPosition.y += diff.y * Camera.main.pixelHeight;
    
    //normal alignement
    worldPosition = Camera.main.ScreenToWorldPoint(pixelPosition);
    worldPosition.z = transform.position.z;
    transform.position = worldPosition;
  }

  Vector2 getDiff(){
    if(screenPositionCorrections != null){
      if(screenPositionCorrections.Length > 0){
        float ratio = Mathf.FloorToInt((Camera.main.pixelWidth / Camera.main.pixelHeight) * 100f) / 100f;
        //Debug.Log(ratio);
        if(DeviceInfo.isIP5() && screenPositionCorrections.Length > IP5){
          return screenPositionCorrections[IP5];
        }else if(ratio == 0.75f && screenPositionCorrections.Length > IPAD){
          return screenPositionCorrections[IPAD];
        }
      }
    }
    return Vector2.zero;
  }

  Vector2 getPosition(){
    if(parent != null){
      Vector2 par = parent.getPosition();
      relativePosition.x = screenPosition.x + par.x;
      relativePosition.y = screenPosition.y + par.y;
    }else{
      relativePosition.x = screenPosition.x;
      relativePosition.y = screenPosition.y;
    }

    Vector2 diff = getDiff();
    relativePosition.x += diff.x;
    relativePosition.y += diff.y;

    return relativePosition;
  }

  void OnDrawGizmos(){
    if(!enabled)  return;
    updatePosition();
  }
  
}
