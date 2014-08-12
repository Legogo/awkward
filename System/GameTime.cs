using UnityEngine;
using System.Collections;

public class GameTime : MonoBehaviour {

	public float timeScale = 1f;
  public float timeScaleDebug = 1f; // will be toggled by KeyCode.T
	public float elapsedTime = 0f;
  public static float deltaTime = 0f; // equivalent to Time.deltaTime;
	
	void Update () {
		deltaTime = Time.deltaTime * timeScale;

		if(Input.GetKeyDown(KeyCode.KeypadPlus)){
			timeScale += 0.5f;
		}
		if(Input.GetKeyDown(KeyCode.KeypadMinus)){
			timeScale -= 0.5f;
		}
    if(Input.GetKeyUp(KeyCode.T)){
      timeScale = (timeScale == 1f) ? timeScaleDebug : 1f;
    }
		
		elapsedTime += Time.deltaTime * timeScale;
		elapsedTime = Mathf.Max(0, elapsedTime);
	}

  static public GameTime manager;
  static public GameTime init(){
    if(manager != null) return manager;
    GameObject obj = GameObject.Find("_system");
    if(obj == null) obj = new GameObject("_system");
    manager = obj.GetComponent<GameTime>();
    if(manager == null) manager = obj.AddComponent<GameTime>();
    return (GameTime)manager;
  }

}
