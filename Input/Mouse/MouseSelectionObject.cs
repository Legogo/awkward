using UnityEngine;
using System.Collections;

/*
 * 2013-09-19 - 0.2
 * add will trigger hover event
 * add will trigger on click event
 * fix set to be herited from
 * */

public class MouseSelectionObject : MonoBehaviour {
	
	Collider refCollider;
	
	[HideInInspector]public bool clickState = false;
	[HideInInspector]public bool hoverState = false;
  [HideInInspector]public bool onClickOverObject = false; // was above the object when click happened

	virtual protected void Awake(){
    MouseSelectionManager.get();
		//Debug.Log(gameObject.layer+","+MouseSelectionManager.manager.layer.value);
		//if(gameObject.layer != MouseSelectionManager.manager.layer)	Debug.LogWarning("[MOUSE SELECTION] object "+name+" don't have same layer has Mouse manager");
		refCollider = collider;
		if(refCollider == null)	refCollider = transform.GetComponentInChildren<Collider>();
		if(refCollider == null){
			Debug.LogWarning("[MOUSE SELECTION] couldn't find any collider for "+name);
			enabled = false;
			return;
		}
	}
	
	void Start(){
		//Debug.Log("(re)start mouse selection object of "+name);
		resetState();
	}
	
	public void resetState(){
		clickState = hoverState = false;
	}
	
	virtual protected void Update () {

    //release
    if(!Input.GetMouseButton(0) && clickState){
      //Debug.Log(MouseSelectionManager.currentCollider.name);
      if(MouseSelectionManager.currentCollider == refCollider){
        event_releaseOnObject();
      }
      event_release();
    }
		
		if(MouseSelectionManager.currentCollider == refCollider){
			event_hover();
			if(Input.GetMouseButtonDown(0) && !clickState)	event_click();
		}else{ // hover un autre au aucun
      if(hoverState){
        event_out();
      }
    }
		
	}

  virtual public void event_releaseOnObject(){
    //Debug.Log(name + " mouse release on object");
  }

  virtual public void event_release(){
    //Debug.Log(name + " mouse release");
    clickState = false;
  }

  //on click ON OBJECT
  virtual public void event_click(){
    //Debug.Log(name + " mouse click");
    clickState = true;
  }

	virtual public void event_out(){
    //Debug.Log(name + " mouse is out");
    hoverState = false;
  }
  // chaque frame
	virtual public void event_hover(){
    //Debug.Log(name + " mouse is hover");
    //Debug.Log(name+" hovered");
		hoverState = true;
	}
	
	
}
