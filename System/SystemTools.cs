
using UnityEngine;
using System.Collections;

public class SystemTools : MonoBehaviour {

  void Update () {
    if(Input.GetKeyUp(KeyCode.Backspace)){
      Application.Quit();
    }

    if(Input.GetKeyUp(KeyCode.T)){
      Time.timeScale = (Time.timeScale < 1f) ? 1f : 0.01f;
    }
  }
  
}