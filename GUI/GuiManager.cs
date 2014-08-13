using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * v0.1 2012-11-19
 * */

public class GuiManager : MonoBehaviour {
	
	static public List<GuiObject> list;
	
	static public void addObject(GuiObject obj){
		list.Add(obj);
		//Debug.Log(obj.name+" added");
	}
	
	static public void init(bool resources){
		list = new List<GuiObject>();
		
		Object[] resourceList;
		
		if(resources){
			resourceList = Resources.LoadAll("Prefabs/GUI");
			
			for(int i = 0; i < resourceList.Length; i++){
				GameObject obj = (GameObject)resourceList[i];
				GameObject instance = (GameObject)GameObject.Instantiate(obj);
				instance.name = obj.name;
				list.Add(instance.GetComponent<GuiObject>());
			}
			
		}else{
			resourceList = GameObject.FindObjectsOfType(typeof(GuiObject));
			
			for(int i = 0; i < resourceList.Length; i++){
				list.Add((GuiObject)resourceList[i]);
			}
		}
		
		/*
		Camera cam = GameObject.Find("cam_gui").GetComponent<Camera>();
		cam.clearFlags = CameraClearFlags.Depth;
		cam.clearFlags = CameraClearFlags.Nothing;
		*/
		
		//Debug.Log("GUI > "+objects.Count+" obj");
	}
	
	static public void resetAll(){
		foreach(GuiObject obj in list){
			obj.reset();
		}
	}
	
	static public GuiObject getObject(string path){
		foreach(GuiObject obj in list){
			if(obj.name == path){
				return obj;
			}
		}
		
		Debug.LogWarning("Couldn't find "+path);
		return null;
	}
	
	static public void call(string path){
		GuiObject obj = getObject(path);
		if(obj != null)	obj.call();
	}
	
	static public void callGuiTranslate(string path){
		GuiObject obj = getObject(path);
		obj.callTranslate();
	}
	
	static public void kill(string path){
		GuiObject obj = getObject(path);
		if(obj != null)	obj.kill();
	}
	
	static public void setGuiText(string path, string content){
		foreach(GuiObject obj in list){
			if(obj.name == path){
				TextMesh text = (TextMesh)obj.transform.GetComponentInChildren<TextMesh>();
				if(text != null){
					text.text = content;
					return;
				}
			}
		}
		
		Debug.LogWarning("Couldn't find "+path+" in GUI");
	}
	
}
