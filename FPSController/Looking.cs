using UnityEngine;
using System.Collections;

public class Looking : MonoBehaviour {
	
	MouseSensivity mouseSensiv;
	float verticalLimits = 85f; // degrees
	
	//Vitesse de la souris
	public float sensitivityX = 5f;
	public float sensitivityY = 5f;
	
	//Angle actuel de l'avatar
	float rotationX = 0f;
	float rotationY = 0f;
	
	public Transform horizontalPivot;
	public Transform verticalPivot;
	
  Vector3 horizPositionDefault;
  Quaternion horizRotationDefault;

  Vector3 verticPositionDefault;
  Quaternion verticRotationDefault;

	void Start () {
		mouseSensiv = (MouseSensivity)GameObject.FindObjectOfType(typeof(MouseSensivity));

		if(horizontalPivot == null)	enabled = false;
    else{
      horizPositionDefault = horizontalPivot.position;
      horizRotationDefault = horizontalPivot.rotation;
    }

		if(verticalPivot == null)	enabled = false;
    else{
      verticPositionDefault = verticalPivot.position;
      verticRotationDefault = verticalPivot.rotation;
    }
	}
	
  public void reset(){
    if(verticalPivot != null){
      verticalPivot.position = verticPositionDefault;
      verticalPivot.rotation = verticRotationDefault;
    }
    if(horizontalPivot != null){
      horizontalPivot.position = horizPositionDefault;
      horizontalPivot.rotation = horizRotationDefault;
    }
    rotationY = 0f;
    rotationX = 0f;
  }

	void Update() {
		if(!Screen.lockCursor){
      if(Input.GetMouseButtonDown(0)){
        Screen.lockCursor = true;
      }
      return;
    }
		
		// --- Mouse looking around
		float xvalue = Input.GetAxis("Mouse X");
		if(mouseSensiv != null)	xvalue *= mouseSensiv.factor;
		if(xvalue != 0){
			rotationX = xvalue * sensitivityX;
			rotationX = Mathf.Clamp(rotationX, -360,360);
			horizontalPivot.transform.Rotate(Vector3.up * rotationX);
		}
		
		float yvalue = Input.GetAxis("Mouse Y");
		if(mouseSensiv != null)	yvalue *= mouseSensiv.factor;
		if(yvalue != 0){
			rotationY += yvalue * sensitivityY;
			rotationY = Mathf.Clamp(rotationY, -verticalLimits,verticalLimits);
			Vector3 euler = verticalPivot.eulerAngles;
			euler.x = -rotationY;
			verticalPivot.eulerAngles = euler;
		}
	}

}
