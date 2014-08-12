using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 2013-09-19 - 0.2
 * fix rename class
 * add clickable object class
 * add debug info
 * 
 * 2012-11-26 - 0.1
 * # Il faut parametrer la LAYER et mettre les objets interactifs dans cette layer
 * */

public class MouseSelectionManager : MonoBehaviour {
	
	static public Collider currentCollider = null;
	
	public GameObject rayOrigin;
	Vector3 mousePos;
	RaycastHit hit;
	
	public float rayDistance = 20; // distance d'interactivité (de la camera)
	public LayerMask layer; // doit etre mise en public et ref dans l'editeur ...
	
  public bool RAY_FROM_CENTER = false;
	public bool SHOW_DEBUG_RAY = false;
	public bool SHOW_DEBUG = false;
	
	void Awake(){
		manager = this;
		hit = new RaycastHit();
    layer = LayerMask.NameToLayer("Everything");
	}
	
	void Start () {
		MouseSelectionManager.currentCollider = null;
		//mousePos.x = Screen.width * 0.5f;
		//mousePos.y = Screen.height * 0.5f;	
	}
	
	void Update () {
		//mousePos = Input.mousePosition;
		//mousePos.z = -100;
		
		Collider collided = null;
		Color debugColor = Color.red;
		
		if(rayOrigin != null){
			
			//raycast based on gameobject in world (position / forward)
			
			if (Physics.Raycast(rayOrigin.transform.position, rayOrigin.transform.forward, out hit, rayDistance, layer)) {
				debugColor = Color.green;
				collided = hit.collider;
			}
			if(SHOW_DEBUG_RAY)	Debug.DrawLine(
				rayOrigin.transform.position, 
				rayOrigin.transform.position + rayOrigin.transform.forward * rayDistance, debugColor);
		}else{
			
			//raycast based on mouse position in screen
			
      if(RAY_FROM_CENTER){
        mousePos.x = Screen.width * 0.5f;
        mousePos.y = Screen.height * 0.5f;
      }else{
        mousePos = Input.mousePosition;
      }
			
			//mousePos.z = -Camera.main.transform.position.z;
			//mousePos = Camera.main.ScreenToWorldPoint(mousePos);
			//Debug.DrawLine(Vector3.zero, mousePos, Color.red);
			
			Ray ray = Camera.main.ScreenPointToRay(mousePos);
			if (Physics.Raycast(ray, out hit, rayDistance, layer)) {
				debugColor = Color.green;
				collided = hit.collider;
			}
			
			if(SHOW_DEBUG_RAY)	Debug.DrawLine(
				ray.origin, 
				ray.origin + ray.direction * rayDistance, debugColor);
		}
		
		//if(collided != null)	Debug.Log("[MOUSE SELECTION] hover : "+collided.name);
		MouseSelectionManager.currentCollider = collided;
	}
	
	void OnGUI(){
		if(!SHOW_DEBUG)	return;
		string content = "[MOUSE SELECTION]";
		if(currentCollider == null) content += "\nNo collider";
		else{
			content += "\ncurrentCollider = "+currentCollider;
			if(currentCollider.transform.parent != null)	content += "\nparent : "+currentCollider.transform.parent.name;
		}
		GUI.TextArea(new Rect(10,10,400,50), content);
	}
	
	static public bool underMouse(Collider objCollider){
		
		if(MouseSelectionManager.currentCollider == null)	return false;
		if(MouseSelectionManager.currentCollider == objCollider)	return true;
		
		return false;
	}

  static public MouseSelectionManager manager;
  static public MouseSelectionManager get(){
    if(manager != null) return (MouseSelectionManager)manager;
    GameObject obj = GameObject.Find("_input");
    if(obj == null) obj = new GameObject("_input");
    manager = obj.GetComponent<MouseSelectionManager>();
    if(manager == null) manager = obj.AddComponent<MouseSelectionManager>();
    return (MouseSelectionManager)manager;
  }
}
