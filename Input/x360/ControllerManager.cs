using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 2012-12-23
 * Les ids des manettes correspondent à l'ordre de branchement
 * Il faut attendre un input de la manette pour être certains de l'id
 * 
 * http://forum.unity3d.com/threads/114993-Joystick-detection-and-direct-axis-detection
 * */

public class ControllerManager : MonoBehaviour {
	
	static public ControllerManager manager;
	static public bool XINPUT = true;

	public const int MAX_CONTROLLER = 4;
	public int connectedCount = 0;
	public EventManager eventManager;

  protected Controller360[] controllers;
  protected Controller360[] tempControllers; // temp var on update controller

	virtual protected void Awake(){
    manager = this;

    tempControllers = new Controller360[MAX_CONTROLLER];
    controllers = new Controller360[MAX_CONTROLLER];

		updateControllers ();
	}

	void Update () {	checkUpdate();	}

	void checkUpdate(){
		//quand le nombre de controller branché change !
		if(Input.GetJoystickNames().Length != connectedCount){
			updateControllers();
		}
	}
	
	virtual public void updateControllers(){
		connectedCount = Input.GetJoystickNames().Length;
    //Debug.Log("updating controllers, raw count is "+connectedCount);

    Controller360 c = null;
		for(int i = 0; i < MAX_CONTROLLER; i++){
			GameObject obj = GameObject.Find("controller-"+i);
      tempControllers[i] = null;

			if(i < connectedCount){
				
        if(obj == null){
					c = create(i);
					event__controllerPlugged(i);
				}else{
          c = obj.GetComponent<Controller360>();
        }

        tempControllers[i] = c;
			}else{
				if(obj != null){
					GameObject.DestroyImmediate(controllers[i].gameObject);
					event__controllerUnplugged(i);
				}
			}
		}
		
    //Debug.Log("controller update, list is "+list.Count+" long");
    updateControllerArray();
	}

  void debug__displayControllerContent(){
    for(int i = 0; i < controllers.Length; i++){
      if(controllers[i] == null)  Debug.Log("controller of index "+i+" is null");
      else Debug.Log("controller of index "+i+" has controllerId of "+controllers[i].getControllerId());
    }
  }

  /* assign les manettes dans les cases du manager */
  virtual protected void updateControllerArray(){
    // sur le manager de base les manettes sont rangées dans l'ordre
    // sous xinput les manettes sont dans les cases correspondantes aux ids

    for(int i = 0; i < tempControllers.Length; i++){
      if(i >= tempControllers.Length) controllers[i] = null;
      else controllers[i] = tempControllers[i];
    }

  }

	public void event__controllerPlugged(int index){
		ConsoleBase.add("plug "+index);
		if(eventManager != null)	eventManager.controllersUpdated("PLUGGED controller of index "+index);
	}
	public void event__controllerUnplugged(int index){
    ConsoleBase.add("unplug "+index);
		if(eventManager != null)	eventManager.controllersUpdated("UNPLUGGED controller of index "+index);
	}

	virtual public Controller360[] getControllers(){
		updateControllers();
		return controllers;
	}
  virtual public Controller360 getController(int idx){
    updateControllers();
    return controllers[idx];
  }

	virtual public bool anyPressedSkip(){
		if(countConnected() < 1)	return false;

		for(int i = 0; i < controllers.Length; i++){
			Controller360 c = controllers[i];
			if(c == null) continue;
			//ConsoleSurround.add("checking "+c.name);
			if(c.released[Controller360.START])	return true;
			if(c.released[Controller360.A])	return true;
		}
		return false;
	}

	virtual public bool anyPressedBack(){
		if(Input.GetKeyUp(KeyCode.Escape))	return true;
		if(controllers.Length < 1)	return false;
		for(int i = 0; i < controllers.Length; i++){ if(controllers[i] == null) continue; if(controllers[i].released[Controller360.BACK])	return true; }
		return false;
	}

	virtual public bool anyPressedStart(){
		if(controllers.Length < 1)	return false;
		for(int i = 0; i < controllers.Length; i++){ if(controllers[i] != null){ if(controllers[i].released[Controller360.START])	return true; } }
		return false;
	}
	
	virtual public bool anyPressedA(){
		if(controllers.Length < 1)	return false;
		
		foreach(Controller360 c in controllers){
			if(c != null){
				//Debug.Log("checking "+c.name);
				if(c.released[Controller360.A])	return true;
			}
		}
		return false;
	}

	// create new controller
	virtual public Controller360 create(int id){
		Controller360 control = null;
		if(GameObject.Find("controller-"+id) == null){
			GameObject obj = new GameObject("controller-"+id);
			control = obj.AddComponent<Controller360>();
		}
		//Debug.Log("Created controller "+id);
		return control;
	}

	virtual public int countConnected(){
		int count = 0;
		for(int i = 0; i < controllers.Length; i++){
			Controller360 c = controllers[i];
			if(c == null)	continue;
			if(c.isConnected())	count++;
		}
		return count;
	}

	virtual public int countRefInArray(){
		int count = 0;
    //Debug.Log("checking ref in array, controllers array length is : "+controllers.Length);
		for(int i = 0; i < controllers.Length; i++){
			Controller360 c = controllers[i];
			if(c == null){
        //Debug.Log(i+" is null");
        continue;
      }
			count++;
		}
		return count;
	}
  
  static public ControllerManager create(){
    if(manager != null) return (ControllerManager)manager;
    GameObject obj = GameObject.Find("_input");
    if(obj == null) obj = new GameObject("_input");
    manager = obj.GetComponent<ControllerManager>();
    if(manager == null) manager = obj.AddComponent<ControllerManager>();
    return (ControllerManager)manager;
  }


}
