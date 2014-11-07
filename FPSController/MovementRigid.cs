using UnityEngine;
using System.Collections;

/// <summary>
/// Movement rigid
/// 
/// Essayer de trouver une alternative au CharacterController
/// http://forum.unity3d.com/threads/111842-Optimized-alternatives-to-character-controller-physics
/// http://docs.unity3d.com/Documentation/ScriptReference/Physics.Raycast.html
/// 
/// </summary>

public class MovementRigid : Movement {
	
  protected Rigidbody controller;
	
  override protected void Awake(){
    base.Awake();
    controller = rigidbody;
    feetGap = 0f;
	}

  override protected void update__position(){
    solveDirection(); //update new movement direction
    
    // Apply a force that attempts to reach our target velocity
    Vector3 velocityChange = (applyDirection - controller.velocity);
    velocityChange.x = Mathf.Clamp(velocityChange.x, -speedCapSolved.x, speedCapSolved.x);
    velocityChange.z = Mathf.Clamp(velocityChange.z, -speedCapSolved.z, speedCapSolved.z);
    velocityChange.y = 0;
    controller.AddForce(velocityChange, ForceMode.VelocityChange);
	}
  
  void OnCollisionEnter(){
    controller.velocity = Vector3.zero;
  }
  
  override protected void update__gravity(){
    //don't use script gravity
  }

  override public float getFallingSpeed(){
    return controller.velocity.y;
  }
  override public void setFallingSpeed(float val){
    Vector3 v = controller.velocity;
    v.y = val;
    controller.velocity = v;
  }

  override protected Vector3 getPosition(){ return controller.position; }
  override protected void setPosition(Vector3 p){
    //controller.MovePosition(p);
  }

  override protected string debug__getString(){
    string content = "";
    content += (isFalling() ? "falling" : (isJumping() ? "jumping" : "grounded"));
    content += "\nvelocity : "+controller.velocity;
    content += "\nmoveDirectin : "+moveDirection;
    return content;
  }

}
