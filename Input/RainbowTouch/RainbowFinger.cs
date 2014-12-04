using UnityEngine;
using System.Collections;

/*
 * Structure qui imite la struct de la classe Touch
 * Coordz are in screen pixels
 * */

public class RainbowFinger {
	
	public TouchPhase phase;
	public Vector3 deltaPosition = Vector3.zero;
	public Vector3 position = Vector3.zero; // position (X,Y) dans l'écran
	public int fingerId = -1;
	
	float[] momentum = new float[5]{0f,0f,0f,0f,0f}; // permet de récup une valeur de magnitude sur les 5 dernières frames
	
	public Vector3 startPosition = Vector3.zero;
	
  public bool DEBUG = false;
	RainbowFingerAsset asset;
	
	public RainbowFinger(){
		
    if(DEBUG){
			asset = RainbowFingerAsset.getFingerAsset();
			asset.assignFinger(this);
		}
		
		phase = TouchPhase.Canceled;
	}
	
	public void resetMomentum(){
		for(int i = 0; i < momentum.Length; i++){
			momentum[i] = 0f;
		}
    deltaPosition = Vector3.zero;
	}

	public void addMomentum(float delta){
		//shift
		for(int i = momentum.Length - 1; i > 0; i--){
			momentum[i] = momentum[i - 1];
		}
		
		momentum[0] = delta;
	}
	public float getMomentum(){
		float total = 0f;
		for(int i = 0; i < momentum.Length; i++){
			total += momentum[i];
		}
		return total;
	}
	
	public string tostring(){
		return "id:"+fingerId+" position:"+position+" state:"+phase+" dir(Start):"+getDirFromStart()+", momentum:"+getMomentum();
	}
	
	public Vector3 getWorldPosition(){
    position.z = Camera.main.nearClipPlane;
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
    //Debug.Log(position+","+worldPos);

		worldPos.z = 0f;
		return worldPos;
	}
	
  public Vector3 getVectorFromStart(){
    position.z = 0f;
    startPosition.z = 0f;
    return (position - startPosition);
  }

	public Vector3 getDirFromStart(){
		position.z = 0f;
		startPosition.z = 0f;
		return (position - startPosition).normalized;
	}
}
