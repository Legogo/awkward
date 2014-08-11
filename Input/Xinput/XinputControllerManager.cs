using UnityEngine;
using XInputDotNetPure;
using System.Collections;

/// <summary>
/// Xinput controller manager.
/// * Les controllers sont créer dans des GameObjects séparés "controller-[ID]" 
/// * La création des index se fait dans l'ordre. Si il manque 2 entre 1 et 3, 2 sera recréé avant 4
/// </summary>

public class XinputControllerManager : ControllerManager {

  override public void updateControllers(){
  
    Controller360 c = null;
    GameObject obj = null;
    for(int i = 0; i < MAX_CONTROLLER; i++){
      tempControllers[i] = null;
      obj = GameObject.Find("controller-"+i);
      GamePadState gamepadState = GamePad.GetState((PlayerIndex)i, GamePadDeadZone.Circular);

      if(gamepadState.IsConnected){
        //Debug.Log("xinput pad "+i+" is connected");
        if(obj == null){
          c = create(i);
        }else{
          c = obj.GetComponent<Controller360>();
        }
        tempControllers[i] = c;
      }
    }

    updateControllerArray();
    //Debug.Log("controller update, list is "+controllers.Length+" long");
  }

	override public bool anyPressedStart(){
		if(controllers.Length < 1)	return false;
		
		foreach(XinputController c in controllers){
			if(c != null){
				if(c.released[Controller360.START])	return true;
			}
		}
		return false;
	}
	
	override public bool anyPressedA(){
		if(controllers.Length < 1)	return false;
		
		foreach(XinputController c in controllers){
			if(c != null){
				//Debug.Log("checking "+c.name);
				if(c.released[Controller360.A])	return true;
			}
		}
		return false;
	}

  public Controller360 getControllerByXinputId(int id){
    //Debug.Log("getting xinput controller of id "+id);

    for(int i = 0; i < tempControllers.Length; i++){
      if(tempControllers[i] == null){
        //Debug.Log("listTempController (len:"+list.Count+") index:"+i+" is null"); 
        continue;
      }
      XinputController c = (XinputController)tempControllers[i];
      if(c == null){
        //Debug.Log("controller "+i+" is null");
        continue;
      }

      //Debug.Log("checking for xinput controller with id "+id+", this one is "+c.getXinputIndex());
      if(c.getXinputIndex() == id){
        //Debug.Log("found xinput of id "+c.getXinputIndex());
        return c;
      }
    }
    return null;
  }

  override protected void updateControllerArray(){
    for(int i = 0; i < tempControllers.Length; i++){
      controllers[i] = getControllerByXinputId(i);
    }
  }
	
	override public int countConnected(){
    Controller360[] controls = getControllers();
		int count = 0;
    for(int i = 0; i < controls.Length; i++){
      XinputController c = (XinputController)controls[i];
			if(c == null)	continue;
			if(c.isConnected())	count++;
		}
		return count;
	}

	override public Controller360 create(int index){
		XinputController controller = XinputController.add(index);
		//GamePadState state = GamePad.GetState((PlayerIndex)index);
		return controller;
	}

  static public XinputControllerManager xinputManager(){
    if(manager != null) return (XinputControllerManager)manager;
    GameObject obj = GameObject.Find("_input");
    if(obj == null) obj = new GameObject("_input");
    manager = obj.GetComponent<XinputControllerManager>();
    if(manager == null) manager = obj.AddComponent<XinputControllerManager>();
    return (XinputControllerManager)manager;
  }
}
