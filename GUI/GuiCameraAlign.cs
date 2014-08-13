using UnityEngine;
using System.Collections;

/// <summary>
/// GUI camera align.
/// # Permet de positionner un objet devant un camera avec des coord proportionnelles
/// # Le Z du vecteur sera la position en Z de l'objet final Space.World; 0f = transform.position.z;
/// </summary>

public class GuiCameraAlign : MonoBehaviour {
	
	public Camera guiCamera;
	public Vector2 topLeft; // manage depth with Z
	
	void Start () {
    if(guiCamera == null){
      guiCamera = Camera.main;
    }
		
		resizeEvent();
	}
	
	public void resizeEvent(){
		Vector2 screen = Vector2.zero;
		screen.x = Screen.width * topLeft.x;
		screen.y = Screen.height * topLeft.y;
		
		screen = guiCamera.ScreenToWorldPoint(screen);
		
    Vector3 result = screen;
    result.z = transform.position.z;

		transform.position = result;
	}
}
