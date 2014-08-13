using UnityEngine;
using System.Collections;

public class CameraIos : MonoBehaviour {

  float ratio = 0f;

  void Awake () {
    updateRatio();
    Debug.Log(name+" >> ratio : "+ratio);
  }

  void updateRatio(){
    ratio = Camera.main.pixelWidth / Camera.main.pixelHeight;
    ratio = Mathf.FloorToInt(ratio * 100) / 100f;

    //float orthoSize = 5f;
    if(ratio == 0.56f){ // 640x1136 (IP5)
      Camera.main.orthographicSize = 5.9f;
    }else if(ratio == 0.66f){ // 640x960 (IP4), 320x480 (IP3)
      Camera.main.orthographicSize = 5f;
    }else if(ratio == 0.75f){ // 768x1024 (IPAD,IPAD+)
      Camera.main.orthographicSize = 5f;
    }

  }

  void OnDrawGizmos(){
    updateRatio();
  }
}
