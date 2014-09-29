using UnityEngine;
using System.Collections;

public class MouseLock : MonoBehaviour {
	
  public bool lockedAtStart = false;

	void Start(){
    //can't lock in editor
    if(Application.isEditor)  return;

    if(lockedAtStart) toggle(true);
	}
	
	void Update () {
		
    //capture mouse
		if(!Screen.lockCursor && Input.GetMouseButtonUp(0)){
			toggle(false);
		}
		
    //release mouse
		if(Input.GetKeyUp(KeyCode.Escape)){
			toggle(true);
		}
	}
	
	void toggle(bool flag){
		//Debug.Log("toggle "+flag);
		Screen.lockCursor = !flag;
		//Screen.showCursor = flag;
	}

  static public void lockCursor(){
    Screen.lockCursor = true;
  }
  static public void unlockCursor(){
    Screen.lockCursor = false;
  }
}
