using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RainbowFingerAsset : MonoBehaviour {
	
	//DEBUG STUFF
	static public List<RainbowFingerAsset> list; 
	
	RainbowFinger finger;
	TextMesh text;
	
	void Awake () {
    Transform child = transform.FindChild("fingerInfo");
    if(child != null){
      text = child.GetComponent<TextMesh>();
      text.text = "no finger";
    }
		
		toggleRender(false);
	}
	
	void Update () {
    //Debug.Log(finger.tostring());

		if(finger != null){
			
			Vector3 fingerPosition = finger.getWorldPosition();
      //Debug.Log(fingerPosition);
			transform.position = fingerPosition;
			
			if(text != null){
				string content = "#"+finger.fingerId;
				
				content += "\n(phase:"+finger.phase+")";
				content += "\n(x:"+(int)fingerPosition.x+",z:"+(int)fingerPosition.z+")";
				content += "\nmomentum:"+finger.getMomentum();
				content += "\ndelta:"+finger.deltaPosition;
				
				text.text = content;
			}
			
			switch(finger.phase){
				case TouchPhase.Began :
					toggleRender(true);
					break;
				case TouchPhase.Canceled :
					toggleRender(false);
					break;
			}
		}
	}
	
	void toggleRender(bool flag){
		renderer.enabled = flag;
		if(text != null)  text.renderer.enabled = flag;
	}
	
	public void assignFinger(RainbowFinger f){
		finger = f;
	}
	
	public bool available(){
		return !enabled;
	}
	
	static public RainbowFingerAsset getFingerAsset(){
		if(RainbowFingerAsset.list == null)	RainbowFingerAsset.list = new List<RainbowFingerAsset>();
		
		RainbowFingerAsset current = null;
		foreach(RainbowFingerAsset obj in RainbowFingerAsset.list){
			if(obj.available())	current = obj;
		}
		
		if(current == null) current = RainbowFingerAsset.newFingerAsset();
		
		return current;
	}
	
	static public RainbowFingerAsset newFingerAsset(){
		if(RainbowFingerAsset.list == null)	RainbowFingerAsset.list = new List<RainbowFingerAsset>();
		
    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    RainbowFingerAsset asset = sphere.AddComponent<RainbowFingerAsset>();

    RainbowFingerAsset.list.Add(asset);
    asset.name = "finger-"+RainbowFingerAsset.list.Count;
		
    return asset;
	}
}
