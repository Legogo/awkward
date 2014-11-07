using UnityEngine;
using System.Collections;

/// <summary>
/// FOV change based on running speed
/// </summary>

public class MovementFov : MonoBehaviour {
  Movement move;

  float fovStart = 0f;
  float fovMax = 0f;

  float target = 0f;
  float progress = 0f; // [0,1]

  public AnimationCurve curve;
  
  Camera cam;

	void Awake () {
    move = GetComponent<Movement>();
    cam = Camera.main;

    fovStart = cam.fieldOfView;
    fovMax = fovStart + 5f;

    progress = target = 0f;
	}
	
	void Update () {
    //setup target
    target = (move.isRunning()) ? 1f : 0f;
    float speed = (target >= 1f) ? 0.5f : 2f; // go back faster than go "in"

    progress = Mathf.MoveTowards(progress, target, Time.deltaTime * speed);

    //apply
    cam.fieldOfView = Mathf.Lerp(fovStart, fovMax, curve.Evaluate(progress));
	}
}
