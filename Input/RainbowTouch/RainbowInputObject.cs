using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 2012-12-19
 * Classe permettant de gérer la layer de touch avec n doigts sur l'objet parent au script
 * Il faut set le radius !
 * L'objet interactif dans la scène possède un script qui hérite de celui là
 * */

public class RainbowInputObject : MonoBehaviour {
	
	public float radius = 0f; // to set in inspector
	
	List<RainbowFinger> fingers = new List<RainbowFinger>();
	[HideInInspector]public int countFingers = 0; // read only
	
	virtual protected void Start(){
    RainbowInputManager.get().registerObject(this);
	}
	
	public void assignFinger(RainbowFinger finger){
		if(finger == null)	return;
		
		fingers.Add(finger);
		updateCount();
		
		touchEvent();
		//Debug.Log(name+" has finger "+finger.fingerId);
	}
	
	public void unassignFinger(){
		if(fingers.Count <= 0)	return;
		
		//Debug.Log(fingers[0].tostring());
		
		releaseEvent(); // avant de virer les finger, sinon pas de stats !
		
		fingers.Clear();
		updateCount();
		
		//Debug.Log(name+" lost fingers");
	}
	
	public float getMomentum(){
		float total = 0f;
		foreach(RainbowFinger f in fingers){
			total += f.getMomentum();
		}
		return total;
	}
	
	public Vector3 getDelta(){
		return fingers[0].deltaPosition;
	}
	
	protected void updateCount(){
		countFingers = fingers.Count;
	}
	
  public RainbowFinger getFinger(){
    if(fingers.Count <= 0)  return null;
    return fingers[0];
  }

	public bool hasFinger(){
		return (fingers.Count > 0);
	}
	
	/* Distance entre le point d'impact du doigt et le point de release */
	public Vector3 getOffset(){
		return fingers[0].position - fingers[0].startPosition;
	}
	
	virtual public void touchEvent(){
		// ...
	}
	
	virtual public void releaseEvent(){
		// ...
	}
}
