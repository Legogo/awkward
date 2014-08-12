
using UnityEngine;
using System.Collections;

public class SystemTools : MonoBehaviour {

  void Update () {
    if(Input.GetKeyUp(KeyCode.Delete)){
      Application.Quit();
    }
  }
  
}