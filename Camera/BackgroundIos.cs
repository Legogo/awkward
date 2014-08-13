using UnityEngine;
using System.Collections;

public class BackgroundIos : MonoBehaviour {
  
  float ratio = 0f;
  
  void Awake () {
    updateRatio();
    Debug.Log(name+" >> ratio : "+ratio);
  }
  
  void updateRatio(){
    ratio = Camera.main.pixelWidth / Camera.main.pixelHeight;
    ratio = Mathf.FloorToInt(ratio * 100) / 100f;

    Debug.Log(name+" >> ratio : "+ratio);

    //320x480
    //transform.localScale = new Vector3(ratio * Camera.main.orthographicSize * 2f, 1f, 
    if(ratio == 0.66f){
      transform.localScale = new Vector3(ratio*10f,1f,12f);
    }else if(ratio == 0.75f){
      transform.localScale = new Vector3(ratio*10f,1f,10f);
    }
  }
  
  void OnDrawGizmos(){
    updateRatio();
  }
}
