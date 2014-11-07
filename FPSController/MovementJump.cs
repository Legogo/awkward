
using UnityEngine;
using System.Collections;

public class MovementJump : MonoBehaviour {

  Movement move;

  [HideInInspector]public float jumpBoost = 0f; // current jump strenght
  [HideInInspector]public float jumpBoostStep = 10f; // reduction speed
  
  public float jumpSpeed = 10f;
  [HideInInspector]public Transform lastOrientation; // keep orientation during jump

  void Awake(){
    move = GetComponent<Movement>();
    GameObject obj = new GameObject("player-jump-orient");
    lastOrientation = obj.transform;
  }

  void Update(){
    update__grounded();
    update__jumpBoost();
  }

  // each frame on the ground
  protected void update__grounded(){
    if(!move.isGrounded()) return;
    lastOrientation.rotation = transform.rotation; // keep last valid orientation for no air control !
    if (InputKeys.key_space)  event__jump(); //jump ?
  }

  protected void update__jumpBoost(){
    jumpBoost -= Time.deltaTime * jumpBoostStep;

  }

  public void resetJump(){
    jumpBoost = 0f;
  }

  public void event__jump(){
    //if(DEBUG_DRAW)  Debug.Log("jump");
    if(!move.isGrounded()) return;
    
    // jump boost power based on forward running speed
    jumpBoost = move.getSpeed() * 0.5f;
    
    //inject jump speed
    move.setFallingSpeed(jumpSpeed);
  }


}