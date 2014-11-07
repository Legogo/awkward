using UnityEngine;
using System.Collections;

public class MovementCrouch : MonoBehaviour {
  
	public Transform verticalRef;

	float heightDefault_vertical = 0f;
	float heightDefault_controller = 0f;

  BoxCollider box;

	void Start () {
    box = GetComponent<BoxCollider>();

		if(verticalRef == null){
			Debug.LogError("MCrouch >> Need vertical ref to work");
			enabled = false;
			return;
		}

		heightDefault_vertical = verticalRef.localPosition.y;
    heightDefault_controller = box.bounds.extents.y;
    //Debug.Log("Crouch >> vertical height : "+heightDefault_vertical+", controller height : "+heightDefault_controller);
	}

	void Update () {
    //resize la box de colision
		setControllerHeight((isCrouching()) ? heightDefault_controller * 0.5f : heightDefault_controller);

    //déplace la camera
		setRefHeight((isCrouching()) ? heightDefault_vertical * 0.5f : heightDefault_vertical);
	}
	
	void setControllerHeight(float height){
    Vector3 temp = box.bounds.extents;
    temp.y = height;
    box.size = temp;
	}

	void setRefHeight(float height){
		// progressive
		Vector3 pos = verticalRef.localPosition;
		pos.y = Mathf.MoveTowards(pos.y, height, Time.deltaTime * 5f);
		verticalRef.localPosition = pos;
	}
	public bool isCrouching(){ return InputKeys.key_e; }
}
