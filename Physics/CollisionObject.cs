using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Center is transform.position of the object
 * Size is a V3 to draw debug
 * Bounds is width/height size of object in world
 * 
 * Collision is only on X,Z referential
 * 
 * Not collidable if not enable
 * */

public class CollisionObject : MonoBehaviour {
	
	public Rect bounds; // x,y updated each frame
	public Vector3 size = Vector3.zero;
	public Vector3 center = Vector3.zero;
	
	Color gizmoColorColiding = Color.red;
	Color gizmoColorNotColiding = Color.yellow;
	
	public List<CollisionObject> colided;
	
  void Start(){
    autoRect ();
  }

	// permet de récup un rect basé sur la taille du renderer
	public void autoRect(){
		bounds = CollisionTools.getRect(gameObject);
		updateBounds();
	}
	
  public void updateBoundsWithCenter(Vector3 newCenter){
    bounds.x = newCenter.x - bounds.width * 0.5f;
    bounds.y = newCenter.z - bounds.height * 0.5f;
    
    center.x = newCenter.x;
    center.z = newCenter.z;
    
    center.y = 3f; // for debug layer
    
    size.x = bounds.width;
    size.z = bounds.height;
  }
	public void updateBounds(){
    updateBoundsWithCenter(transform.position);
	}
	
	Vector3 nextPosition;
	
	//simple comparison
  public bool checkListTouch__circle(CollisionObject[] list){
    updateBounds();
    for(int i = 0; i < list.Length; i++){
      if(!list[i].enabled) continue;
      if(checkTouch__circle(list[i])) return true;
    }
    return false;
  }

  public bool checkTouch__circle(CollisionObject obj){
    obj.updateBounds();
    if(Vector3.Distance(bounds.center, obj.bounds.center) < bounds.width * 0.5 + obj.bounds.width * 0.5){
      return true;
    }
    return false;
  }

	public bool checkListTouch__rect(CollisionObject[] list){
		updateBounds();
		for(int i = 0; i < list.Length; i++){
			list[i].updateBounds();
      if(!list[i].enabled) continue;

			if(CollisionTools.touchXZ(bounds, list[i].bounds)){
				return true;
			}
		}
		return false;
	}
	
	//complexe comparison for movement
	public void moveStep(CollisionObject[] list, Vector3 step){
		
    nextPosition = transform.position + step;
		updateBoundsWithCenter(nextPosition);
		
		for(int i = 0; i < list.Length; i++){
			CollisionObject wall = list[i];
      if(!wall.enabled) continue;

			//collisions are based on renderer not colliders
			float gapX = CollisionTools.rayX(bounds, wall.bounds);
			float gapY = CollisionTools.rayY(bounds, wall.bounds);
			
      //0 is no collision
			if(gapX != 0f && gapY != 0f){
        //Debug.Log(wall.name+", x:"+gapX+", y:"+gapY);
				if(Mathf.Abs(gapX) < Mathf.Abs(gapY)){
					nextPosition.x += gapX;
				}else{
					nextPosition.z += gapY;
				}
			}

		}

    transform.position = nextPosition;
	}
	
	//debug
	void OnDrawGizmos(){
    if(Application.isEditor)  autoRect();
    if(bounds.width == 0)	return;
		
		if(colided.Count < 1){
			Gizmos.color = gizmoColorNotColiding;
		}else{
			Gizmos.color = gizmoColorColiding;
		}
		
		Gizmos.DrawCube(center, size);
	}

  static public CollisionObject[] getAll(CollisionObject[] filter){
    CollisionObject[] obs = (CollisionObject[])GameObject.FindObjectsOfType(typeof(CollisionObject));
    List<CollisionObject> list = new List<CollisionObject>();
    for (int i = 0; i < obs.Length; i++) {
      bool toAdd = true;
      for (int j = 0; j < filter.Length; j++) {
        //Debug.Log(i+":"+obs[i]+", "+j+":"+filter[j]);
        if(obs[i] == filter[j]) toAdd = false;
      }
      if(toAdd) list.Add(obs[i]);
    }
    return (CollisionObject[])list.ToArray();
  }
}
