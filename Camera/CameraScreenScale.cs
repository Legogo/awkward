using UnityEngine;
using System.Collections;

public class CameraScreenScale : MonoBehaviour {

  Camera cam;

	void Update () {}

  void updateOrthoSize(){
    if(cam == null) cam = Camera.main;

    float h = cam.pixelHeight;
    if(h <= 480)  cam.orthographicSize = 2.5f;
    else if(h > 480 && h < 2048)  cam.orthographicSize = 5f;
    else if(h >= 2048)  cam.orthographicSize = 10f;
  }

  void OnDrawGizmos(){
    updateOrthoSize();
  }

}
