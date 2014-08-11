using UnityEngine;
using System.Collections;

public class CollisionTools {
	
	static public Rect getRect(GameObject obj){
		Rect bounds = new Rect();
		if(obj.renderer == null)	return bounds;
		
		bounds.x = obj.transform.position.x;
		bounds.y = obj.transform.position.z;
		bounds.width = obj.renderer.bounds.size.x;
		bounds.height = obj.renderer.bounds.size.z;
		
		return bounds;
	}
	
	//permet de savoir si deux objets se croisent
	static public bool touchXZ(Rect a, Rect b){
		Vector2 gap = (b.center - a.center);
		gap.x = Mathf.Abs(gap.x);
		gap.y = Mathf.Abs(gap.y);
		
		if(gap.x < a.width * 0.5f + b.width * 0.5f){
			if(gap.y < a.height * 0.5f + b.height * 0.5f){
				return true;
			}
		}
		
		return false;
	}
	
	static public float rayX(Rect a, Rect b){
		float gap = b.center.x - a.center.x;
		float size = (a.width * 0.5f + b.width * 0.5f);
		if(Mathf.Abs(gap) > size)	return 0f;
		return (Mathf.Abs(gap) - size) * Mathf.Sign(gap);
	}
	static public float rayY(Rect a, Rect b){
		float gap = b.center.y - a.center.y;
		float size = (a.height * 0.5f + b.height * 0.5f);
		if(Mathf.Abs(gap) > size)	return 0f;
		return (Mathf.Abs(gap) - size) * Mathf.Sign(gap);
	}
}
