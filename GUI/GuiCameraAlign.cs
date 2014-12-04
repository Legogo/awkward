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

  public void resizeEvent(){
    Vector2 screen = Vector2.zero;
    screen.x = guiCamera.pixelWidth * topLeft.x;
    screen.y = guiCamera.pixelHeight * topLeft.y;
    
    screen = guiCamera.ScreenToWorldPoint(screen);
    
    Vector3 result = screen;
    result.z = transform.position.z;
    
    transform.position = result;
  }

  #if UNITY_EDITOR
  void OnDrawGizmos(){
    if(guiCamera == null) guiCamera = Camera.main;
    resizeEvent();
  }
  #endif
}