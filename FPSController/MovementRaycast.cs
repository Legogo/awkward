using UnityEngine;
using System.Collections;

public class MovementRaycast : Movement {
  
  Vector3[] dirs;
  public int heightCount = 0;
  public LayerMask filters;

  protected int hAverageCount = 0;
  protected Vector3 hAverage = Vector3.zero;

  void Start(){
    radius = collider.bounds.extents.x;
    init();
  }

  //rays directions
  void init(){
    dirs = new Vector3[8];
    dirs[0] = new Vector3(-1f,0f,0f);
    dirs[1] = new Vector3(-1f,0f,1f);
    dirs[2] = new Vector3(0f,0f,1f);
    dirs[3] = new Vector3(1f,0f,1f);
    dirs[4] = new Vector3(1f,0f,0f);
    dirs[5] = new Vector3(1f,0f,-1f);
    dirs[6] = new Vector3(0f,0f,-1f);
    dirs[7] = new Vector3(-1f,0f,-1f);
  }

  override protected void update__position(){
    base.update__position();

    raycastCircle (0f);

    setPosition(nextPosition);
  }

  // event, transition from ground to midair
  void event__midair(){
    //Debug.Log("RAYCAST :: mid-air EVENT");
    grounded = false;
  }

  // event, touch ground
  void event__grounded(){
    //Debug.Log("RAYCAST :: grounded EVENT");
    grounded = true;
    moveJump.resetJump();
    moveDirection.y = 0f; // reset fall speed
  }
  
  void raycastCircle(float height){
    Vector3 center = collider.bounds.center;
    Vector3 origin;

    hAverageCount = 0;
    hAverage = Vector3.zero;

    for(int i = 0; i < dirs.Length; i++){
      origin = collider.bounds.center;
      origin.y += height;

      float length = radius + applyDirection.magnitude;
      Debug.DrawLine(origin, origin + (dirs[i].normalized * length));
      if(Physics.Raycast(origin, dirs[i].normalized, out hit, length)){
        if(hit.distance < radius){
          //Vector3 dirOrigin = (nextPosition - hit.point).normalized;
          hAverage += hit.point;
          hAverageCount++;

          Debug.Log(hit.distance+","+radius);
        }
      }
    }

    if(hAverageCount > 0){
      hAverage /= hAverageCount; // average of positions on the wall
      Vector3 dir = (hAverage - center).normalized; // |WALL-ORIGIN>
      Debug.DrawLine(hAverage, t.position);
      Debug.Log(dir.magnitude+", r:"+radius);
      nextPosition.x = hAverage.x - (dir.x * radius);
      nextPosition.z = hAverage.z - (dir.z * radius);
    }
  }

}

