using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * MANAGER
 * to add somewhere in scene
 * 
 * this framework only works with an orthographic camera !
 * */

public class RainbowInputManager : MonoBehaviour {
	
	List<RainbowInputObject> objects = new List<RainbowInputObject>();
	List<RainbowFinger> fingers = new List<RainbowFinger>();
	
	//PC STUFF
	Vector3 lastPosition = Vector3.zero;
	[HideInInspector]public int touchCount = 0; // read only
	
	void Awake(){
		int qtyFingers = 11;
		if(!isMobile()) qtyFingers = 1;

		//create all 11 touches
		for(int i = 0; i < qtyFingers; i++){
			fingers.Add(new RainbowFinger());
		}
	}
	
	public void reset(){
		foreach(RainbowFinger finger in fingers){
			finger.phase = TouchPhase.Ended;
		}
		
		foreach(RainbowInputObject obj in objects){
			obj.unassignFinger();
		}
		
		//Debug.Log("Input has "+objects.Count+" interactive objects");
	}
	
	public void registerObject(RainbowInputObject obj){
		objects.Add(obj);
		//Debug.Log("Input registered "+obj.name);
	}
	
	public void unregisterObject(RainbowInputObject obj){
		objects.Remove(obj);
	}
	
	void Update () {
		generateFingers();
		solveFingers();
	}

  bool isMobile(){
    if(Application.platform == RuntimePlatform.Android) return true;
    else if(Application.platform == RuntimePlatform.IPhonePlayer) return true;
    return false;
  }
	
	void generateFingers(){
		
		RainbowFinger finger;
		
    if(isMobile()){
			//ON IPHONE
			int i = 0;
			foreach(Touch t in Input.touches){
				finger = fingers[i];
				
				finger.fingerId = t.fingerId;
				finger.deltaPosition = t.deltaPosition;
				finger.position = t.position;
				finger.phase = t.phase;
				
				if(finger.phase == TouchPhase.Began){
					finger.startPosition = finger.position;
				}
				
				i++;
			}
			
			touchCount = Input.touchCount;
		}else{
			//ON PC
			finger = fingers[0];
			
			if(Input.GetMouseButton(0)){
				
				finger.fingerId = 0;
				
				finger.position = Input.mousePosition; // set new position
				if(lastPosition.magnitude == 0)	lastPosition = finger.position; // keep old position
				
				finger.deltaPosition = finger.position - lastPosition; // solve delta with old position
				lastPosition = finger.position; // remove old position for next frame
				
				//next position
				if(finger.phase == TouchPhase.Ended){
					finger.phase = TouchPhase.Began;
					finger.startPosition = finger.position;
				}else if(finger.deltaPosition.magnitude > 0){
					finger.phase = TouchPhase.Moved;
					finger.addMomentum(finger.deltaPosition.magnitude);
				}else if(finger.deltaPosition.magnitude == 0){
					finger.phase = TouchPhase.Stationary;
					finger.resetMomentum();
				}
				
				touchCount = 1;
			}else{
				finger.fingerId = -1;
				finger.phase = TouchPhase.Ended;
				lastPosition = Vector3.zero;
				
				touchCount = 0;
			}
		}
		
	}
	
	void solveFingers(){
		
		if(objects == null)	return;
		if(objects.Count <= 0)	return;
		
		foreach(RainbowInputObject obj in objects){
			if(touchCount <= 0)	obj.unassignFinger();
			else obj.assignFinger(touch(obj));
		}
		
	}
	
	/* Retourne le premier doigt qui se trouve au dessus */
	RainbowFinger touch(RainbowInputObject w){
		foreach(RainbowFinger finger in fingers){
			if(finger.phase == TouchPhase.Began && isOver(finger, w)){
				return finger;
			}
		}
		
		return null;
	}
	
  public RainbowFinger getFinger(int index){
    return fingers[index];
  }

	public bool isOver(RainbowFinger f, RainbowInputObject w){
    Vector3 fPosition = f.getWorldPosition();
    Vector3 wPosition = w.transform.position;
    fPosition.z = 0f;
    wPosition.z = 0f;

    if(w.collider != null){
      wPosition = w.collider.bounds.center;
      fPosition.z = wPosition.z;
      //Debug.Log(wPosition+","+fPosition);
      if(w.collider.bounds.Contains(fPosition)) return true;
    }else{
      float distance = Vector3.Distance(fPosition, wPosition);
      //Debug.Log("w:"+w.name+", distance:"+distance+"\nfWorldPosition:"+fPosition+" - wPos:"+wPosition);
      if(distance < w.radius) return true;
    }
    return false;
	}

  public bool DEBUG = false;
  void OnGUI(){
    if(!DEBUG) return;
    string content = "";
    content = "touchCount : "+touchCount;
    content += "\nfingers ? "+fingers.Count;
    for(int i = 0; i < fingers.Count; i++){
      content += "\nfinger("+i+") : "+fingers[i].startPosition+" , "+fingers[i].position;
      content += "\nfromStart : "+fingers[i].getVectorFromStart();
    }
    GUI.TextArea(new Rect(10,10,300,100), content);
  }
  
  static public RainbowInputManager manager;
  static public RainbowInputManager get(){
    if(manager != null) return (RainbowInputManager)manager;
    GameObject obj = GameObject.Find("_input");
    if(obj == null) obj = new GameObject("_input");
    manager = obj.GetComponent<RainbowInputManager>();
    if(manager == null) manager = obj.AddComponent<RainbowInputManager>();
    return (RainbowInputManager)manager;
  }
}
