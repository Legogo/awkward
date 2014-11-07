using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

  protected Transform t;

  // x,y,z strafe / fall / forward (0 is null)
  public Vector3 speedCapNormal = new Vector3(3f, 0f, 3f);
  public Vector3 speedCapSprint = new Vector3(5f, 0f, 8f);
  protected Vector3 speedCapSolved = Vector3.zero; // overrided by normal or sprint speed

  public Vector3 speedStepNormal = new Vector3(10f,0f,10f);
  public Vector3 speedStepSprint = new Vector3(20f,0f,20f);
  protected Vector3 speedStepSolved = Vector3.zero; // overrided by normal or sprint speed

  public float gravity = 0.5f;

  protected float crouchBoost = 0f;
  protected float crouchBoostStep = 20f;
  
  protected Vector3 applyDirection = Vector3.zero;
  protected Transform worldHandle;
  protected Transform vertical;

  protected float colliderGap = 0f; // [AWAKE] distance entre le pivot du joueur et son collider
  protected float feetGap = 0.1f; // permet que le collider ne touche pas le sol (empeche déplacement du rigidbody)
  protected float radius = 0f;
  protected Vector3 nextPosition;

  protected Vector3 frixion = Vector3.zero; // val qui augmente pour que la frixion ai de plus en plus d'effet
  protected float frixionStep = 3f;

  [HideInInspector]public float additionnalSpeed = 0f;

  protected Vector3 moveDirection = Vector3.zero;
  protected bool grounded = false;
  protected bool crouch = false;

  protected float rayLength = 20f;
  protected RaycastHit hit;

  protected MovementJump moveJump;
  protected MovementCrouch moveCrouch;

  [HideInInspector]public CollisionFlags flags;
  public bool USE_INERTIA = true;
  public bool DEBUG_DRAW = false;

  virtual protected void Awake(){
    t = transform;
    vertical = t.Find("vertical");

    moveCrouch = t.GetComponent<MovementCrouch>();
    moveJump = t.GetComponent<MovementJump>();

    colliderGap = feetGap + (collider.bounds.center.y - t.position.y);

    if(!USE_INERTIA) frixionStep = 100f;
  }

  public void reset(){
    grounded = false;
    crouch = false;
    frixion = Vector3.zero;
    moveDirection = Vector3.zero;
  }
  
  void FixedUpdate() {
    update__move(); // horizontal movement
    update__fall(); // vertical movement
    update__position();
    update__checkFall(); // checks after vertical
  }

  protected void update__move(){
    solveSpeed(); // speed limit varies based on player input (walk/run)

    if(!USE_INERTIA){
      moveDirection.x = 0f; moveDirection.z = 0f;
      if(InputKeys.key_left) moveDirection.x = -speedCapSolved.x;
      else if(InputKeys.key_right) moveDirection.x = speedCapSolved.x;

      if(InputKeys.key_up) moveDirection.z = speedCapSolved.z;
      else if(InputKeys.key_down) moveDirection.z = -speedCapSolved.z;

      return;
    }

    //strafe
    float horizontal = 0f;
    if(InputKeys.key_left)  horizontal = -speedStepSolved.x;
    else if(InputKeys.key_right)  horizontal = speedStepSolved.x;
    
    if(horizontal != 0f){ // si le joueur a appuyé sur qq chose
      if(Mathf.Sign(horizontal) != Mathf.Sign(moveDirection.x)) horizontal *= speedStepSolved.x; // repartir plus vite dans l'autre sens
      frixion.x = 0f;
    }else{ // si le joueur appuie sur rien

      if(moveDirection.x != 0f) frixion.x += Time.deltaTime * frixionStep;
      else frixion.x = 0f;

      moveDirection.x = Mathf.MoveTowards(moveDirection.x, 0f, frixion.x); // apply frixion
    }

    //move front/back
    float vertical = 0f;
    if(InputKeys.key_up)  vertical = speedStepSolved.z;
    else if(InputKeys.key_down) vertical = -speedStepSolved.z;

    if(vertical != 0f){
      if(Mathf.Sign(vertical) != Mathf.Sign(moveDirection.z)) vertical *= speedStepSolved.z;
      frixion.z = 0f;
    }else{

      if(moveDirection.z != 0f) frixion.z += Time.deltaTime * frixionStep;
      else frixion.z = 0f;

      moveDirection.z = Mathf.MoveTowards(moveDirection.z, 0f, frixion.z);
    }
    
    //increase speed
    moveDirection.x += horizontal * Time.deltaTime;
    moveDirection.z += vertical * Time.deltaTime;
    moveDirection.y = getFallingSpeed();
    //Debug.Log(moveDirection);

    //cap speed
    if(!grounded){
      crouchBoost = 0f;
    }
    
    //clamp next move
    if(Mathf.Abs(moveDirection.x) > speedCapSolved.x) moveDirection.x = Mathf.Sign(moveDirection.x) * speedCapSolved.x;
    if(Mathf.Abs(moveDirection.z) > speedCapSolved.z) moveDirection.z = Mathf.Sign(moveDirection.z) * speedCapSolved.z;
  }

  public bool isMoving(){
    //return rigidbody.velocity.magnitude > 0f;
    return moveDirection.magnitude > 0.25f; // because player is falling when on the ground
  }

  protected void update__fall(){
    update__gravity();
  }

  virtual protected void update__gravity(){
    if(!grounded) moveDirection.y -= gravity;
  }

  virtual protected void update__checkFall(){
    //Debug.Log("checking fall at "+getPosition());

    //si le mec est en train de monter dans les airs
    if(moveDirection.y > 0f)  return;

    bool rayHit = false;
    rayHit = Physics.Raycast(collider.bounds.center, -Vector3.up, out hit, rayLength);
    
    if(!rayHit){
      event__falling();
      return;
    }
    
    if(DEBUG_DRAW)  Debug.DrawLine(collider.bounds.center, hit.point);

    //Debug.Log(hit.distance+" <= "+colliderGap);

    if(hit.distance <= colliderGap){
      event__collisionBelow();
      
      //replace body verticaly
      Vector3 current = getPosition();
      current.y = hit.point.y + feetGap;
      //Debug.Log("touched ground at "+current);
      setPosition(current);
    }else{
      event__falling();
    }

  }
  
  virtual protected void update__position(){
    solveDirection();
    
    //lastPosition = transform.position; // keep last position to go back
    applyDirection *= Time.deltaTime;
    nextPosition = getPosition() + applyDirection;
    
    //Debug.Log(nextPosition);
    setPosition(nextPosition);
    
    //if(DEBUG_DRAW)  Debug.Log("movement applied");
  }

  virtual protected void solveDirection(){
    //projete la position dans le referentiel du joueur
    //permet de conserver la direction de déplacement quand on est mid-air
    if(moveJump != null){
      applyDirection = moveJump.lastOrientation.TransformDirection(moveDirection);
      return;
    }

    applyDirection = t.TransformDirection(moveDirection);
  }

  protected void solveSpeed(){

    if(InputKeys.key_shift){
      speedCapSolved = speedCapSprint;
      speedStepSolved = speedStepSprint;
    }else{
      speedCapSolved = speedCapNormal;
      speedStepSolved = speedStepNormal;
    }
    
    speedCapSolved.z += additionnalSpeed; //additionnal speed module ?
  }

  virtual public void toggleMovement(bool flag){
    enabled = flag;
  }

  protected void event__crouch(){
    //Debug.Log("crouch");
    if(!isRunning()) return;
    if(crouchBoost > 0f)  return; // already boosting
    crouchBoost = moveDirection.z * 3f;
  }

  protected void event__falling(){
    grounded = false;
  }
  
  protected void event__collisionBelow(){
    //if(DEBUG_DRAW)  Debug.Log("collision below");
    grounded = true;
    moveDirection.y = 0f; // reset fall speed
    //Debug.Log("below");
  }

  public float getSpeed(){  return moveDirection.z; }
  virtual public float getFallingSpeed(){ return moveDirection.y; }
  virtual public void setFallingSpeed(float val){ moveDirection.y = val; }

  virtual protected Vector3 getPosition(){ return t.position; }
  virtual protected void setPosition(Vector3 p){
    t.position = p;
  }

  public bool canCrouch(){ return moveCrouch != null; }
  public bool isCrouching(){ if(!canCrouch()) return false; return moveCrouch.isCrouching(); }
  public bool isGrounded(){ return grounded; }
  public bool isWalking(){return speedCapSolved.z < speedCapSprint.z; }
  public bool isRunning(){return speedCapSolved.z >= speedCapSprint.z; }

  public bool isFallingTooFast(){ return getFallingSpeed() < -25f; }
  public bool isFalling(){ return getFallingSpeed() < 0f; } // going down
  public bool isJumping(){ return getFallingSpeed() > 0f; } // going up in mid-air

  virtual protected string debug__getString(){
    string content = ""+((grounded) ? "grounded !" : "mid-air");

    content += "\nnextPosition:"+nextPosition;
    content += "\nmoveDirection:"+moveDirection;

    content += "\nHitDistance:"+hit.distance;
    content += "\nHitNormal:"+hit.normal;

    content += "\nspeedCap (solved):"+speedCapSolved;
    //content += "\nfrixion:"+frixion;

    return content;
  }

  void OnGUI(){
    if(!DEBUG_DRAW) return;
    GUI.TextArea(new Rect(0,0,200,150), debug__getString());
  }
}
