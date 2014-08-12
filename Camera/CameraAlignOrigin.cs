using UnityEngine;
using System.Collections;

/// <summary>
/// Camera align origin.
/// Written by André BERLEMONT
/// Align bottom left of camera to world 0f,0f,0f position
/// </summary>

public class CameraAlignOrigin : MonoBehaviour {
    
	Vector3 bottomLeft;
	Vector3 topRight;
	Vector3 centerPoint;
	Vector3 newPosition;
	
	Camera cam;
    void Start(){
		cam = Camera.main;
		align();
	}
	
	void align(){
    //get camera z
    Vector3 pos = Vector3.zero;
    pos.z = cam.nearClipPlane;

		bottomLeft = cam.ScreenToWorldPoint(pos);

    pos.x = Screen.width;
    pos.y = Screen.height;

		topRight = cam.ScreenToWorldPoint(pos);

    Debug.Log(bottomLeft+","+topRight);

		centerPoint = bottomLeft + ((topRight - bottomLeft) * 0.5f);
		
    Debug.Log(centerPoint);

		newPosition.x = (centerPoint.x - bottomLeft.x);
		newPosition.y = (centerPoint.y - bottomLeft.y);

		Debug.Log(newPosition);
		cam.transform.position = newPosition;
	}
}