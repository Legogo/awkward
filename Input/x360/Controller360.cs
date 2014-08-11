using UnityEngine;
using System.Collections;

/*
 * L'index id d'un controller débranché/rebranché peut avoir changé si il est pas sur le même port USB
 * http://wiki.unity3d.com/index.php?title=Xbox360Controller pour le mapping des axis
 * */

public class Controller360 : MonoBehaviour {
	
	public const int LB = 0;
	public const int RB = 1;
	public const int A = 2;
	public const int B = 3;
	public const int X = 4;
	public const int Y = 5;
	public const int START = 6;
	public const int BACK = 7;
	
	public const int QTY_KEYS = 8; // +1 de l'id de la dernière touche
	public string[] map = new string[QTY_KEYS]; // all axis names
	
	//all sticks
	public float[] leftStick = new float[2]; // x, y
	public float[] rightStick = new float[2]; // x, y
	public Vector3 leftStickVector = Vector3.zero; // x, z
	public Vector3 rightStickVector = Vector3.zero; // x, z
  public float[] dPad = new float[2];
  public Vector3 dPadVector = Vector3.zero;

	protected Vector3 previousRightStickVector = Vector3.zero;

	//triggers
	public float leftTrigger = 0f;
	public float rightTrigger = 0f;
	
	//all buttons
	public bool[] pressed = new bool[QTY_KEYS]; // event pressed
	public bool[] released = new bool[QTY_KEYS]; // event released
	public bool[] state = new bool[QTY_KEYS]; // currently pressed ?
	
	public int controllerIndex = 0;
	protected int controllerId = -1;
	
	public bool drawDebug = false;
	
	virtual protected void Start(){
		reset();
	}
  
  virtual protected void Update () {
    if(!isConnected()){
      checkForId();
      return;
    }

    updateControllerInfo();
  }

  virtual public int getControllerId(){ return controllerId; }

  virtual public bool isConnected(){
    if(controllerId > -1) return true;
    return false;
  }

  protected void updateControllerInfo(){
    updateSticks();
    updateButtons();
    updateTriggers();
    updateDpad();
  }

	public void reset(){
		
		//use name to find team auto
		if(name.IndexOf('-') > -1){
			string[] split = name.Split('-');
			controllerIndex = int.Parse(split[1]);
		}

		enabled = true;
		gameObject.SetActive(true);
		//Debug.Log("controller "+controllerIndex+" reseted");
	}
	
	public void kill(){
		controllerId = -1;
		controllerIndex = -1;
		
		enabled = false;
		gameObject.SetActive(false);
	}
	
	void checkForId(){
		//Debug.Log(name+" is checking for slot");
		for(int i = 1; i <= 4; i++){
			
			//move the stick to assign
			if(pressedAnyButton(i)){
				
				if(slotAvailable(i)){
					//Debug.Log("("+name+") "+i+" is moving and is available");
					
					//ID FOUND EVENT
					controllerId = i;
					//joystickName = Input.GetJoystickNames()[i];
					
          event__controllerReady();
					return;
				}
				
			}
		}
	}

  protected void event__controllerReady(){
    updateAxisString();
  }
  protected void event__controllerUnplugged(){
    //...
    GameObject.DestroyImmediate(gameObject);
  }
	protected void event__controllerPlugged(){
    //...
  }

	// check si la manette qui donne un retour est déjà assignée
	bool slotAvailable(int controllerId){
    Controller360[] controls = ControllerManager.manager.getControllers();
    for(int i = 0; i < controls.Length; i++){
      if(controls[i] != null){
        if(controls[i].controllerId == controllerId)	return false;
			}
		}
		return true;
	}
	
	void updateAxisString(){
		map[LB] = "LB_"+controllerId;
		map[RB] = "RB_"+controllerId;
		map[A] = "A_"+controllerId;
		map[B] = "B_"+controllerId;
		map[X] = "X_"+controllerId;
		map[Y] = "Y_"+controllerId;
		map[START] = "Start_"+controllerId;
		map[BACK] = "Back_"+controllerId;
	}
	
	bool pressedAnyButton(int index){
		if(Input.GetAxis("L_XAxis_"+index) != 0 || Input.GetAxis("L_YAxis_"+index) != 0)	return true;
		if(Input.GetAxis("R_XAxis_"+index) != 0 || Input.GetAxis("R_YAxis_"+index) != 0)	return true;
		if(Input.GetAxis("A_"+index) > 0f)	return true;
		if(Input.GetAxis("B_"+index) > 0f)	return true;
		if(Input.GetAxis("X_"+index) > 0f)	return true;
		if(Input.GetAxis("Start_"+index) > 0f)	return true;
		if(Input.GetAxis("Back_"+index) > 0f)	return true;
		return false;
	}

  virtual protected void updateDpad(){
  	/*
    dPad[0] = Input.GetAxis("DPad_XAxis"+controllerId);
    dPad[1] = Input.GetAxis("DPad_YAxis"+controllerId);
    dPadVector.x = dPad[0];
    dPadVector.z = dPad[1];
    */
  }
	
	virtual protected void updateSticks(){
		previousRightStickVector = rightStickVector;
		leftStick[0] = Input.GetAxis("L_XAxis_"+controllerId);
		leftStickVector.x = leftStick[0];
		leftStick[1] = Input.GetAxis("L_YAxis_"+controllerId);
		leftStickVector.z = leftStick[1];

		rightStick[0] = Input.GetAxis("R_XAxis_"+controllerId);
		rightStickVector.x = rightStick[0];
		rightStick[1] = Input.GetAxis("R_YAxis_"+controllerId);
		rightStickVector.z = rightStick[1];
	}

	virtual public float rightStickAngle(){
		if(rightStickVector.magnitude <= 0f)	return 0f;
		return Vector3.Angle(rightStickVector, previousRightStickVector);
	}
	
	virtual protected void updateTriggers(){
		/*
		float val = Input.GetAxis("Triggers_"+controllerId);
		leftTrigger = 0f;
		rightTrigger = 0f;
		if(val > 0)	leftTrigger = val;
		else if(val < 0)	rightTrigger = -val;
		*/

		leftTrigger = Input.GetAxis("TriggersL_"+controllerId);
		rightTrigger = Input.GetAxis("TriggersR_"+controllerId);
	}
	
	virtual protected void updateButtons(){
		for(int i = 0; i < map.Length; i++){
			bool current = (Input.GetAxis(map[i]) > 0f);
			if(pressed[i])	pressed[i] = false;
			if(released[i])	released[i] = false;
			
			if(current && !state[i]){
				//Debug.Log(name+", "+map[i]+" PRESSED");
				pressed[i] = true;
			}else if(state[i] && !current){
				//Debug.Log(name+", "+map[i]+" RELEASED");
				released[i] = true;
			}
			state[i] = current;
		}
	}
	
	public float getLeftStickStrenght(){
		return leftStickVector.magnitude;
	}

	virtual protected void OnGUI(){
		if(!drawDebug) return;
		
		string content = name+" -- ControllerID="+controllerId+" -- Rdy ? "+isConnected();
		if(isConnected()){
			content += "\nA="+state[A]+","+pressed[A]+","+released[A];
			content += "\nB="+state[B]+","+pressed[B]+","+released[B];
			content += "\nSTART="+state[START]+","+pressed[START]+","+released[START];
		}else{
			content += "\nNo id assigned yet, controller is not ready";
		}
		
		GUI.Label(new Rect(controllerIndex * 200f, 100f, 200f, 200f), content);
	}
	
}
