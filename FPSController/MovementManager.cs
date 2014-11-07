using UnityEngine;
using System.Collections;

/// <summary>
/// Manager of starting position and fall exception
/// </summary>

public class MovementManager : MonoBehaviour {
	
	//Reseting
	Camera cam;
	Vector3 initPosition;
	Quaternion initOrientation;
	
	Movement move;
	
	public bool debug = false;
	
	void Start () {
		
		cam = GetComponentInChildren<Camera>();
		
		initPosition = transform.position;
		initOrientation = transform.rotation;
		
		move = GetComponent<Movement>();
	}
	
	public void reset(){
		transform.position = initPosition;
		transform.rotation = initOrientation;
		
		cam.transform.rotation = Quaternion.identity;
		
		//move.togglePhysic(true);
		enabled = true;
	}
	
	void Update () {
		checkFallAnimation();
	}
	
	void checkFallAnimation(){
		if(move.getFallingSpeed() < -40){
			if(ScreenShake.instance != null){
				if(!ScreenShake.instance.isOn()){
					ScreenShake.instance.launch(-1, 0.1f);
				}
				ScreenShake.instance.setWorse(0.05f);
			}
			
			if(move.getFallingSpeed() < -60){
				kill();
			}
		}
	}
	
	void kill(){
		enabled = false;
	}
	
	void OnGUI () {
		if(!debug) return;
		if(move != null)	GUI.Label(new Rect(0,0,300,300), "falling?"+move.getFallingSpeed());
	}
	
	void OnDrawGizmos(){
		Movement move = gameObject.GetComponent<Movement>();
		if(move == null)	return;
		
		if(move.isGrounded()){
			Gizmos.color = Color.green;
		}else{
			Gizmos.color = Color.red;
		}
		
		Vector3 feet = transform.position;
		Gizmos.DrawSphere(feet, 0.1f);
	}
}
