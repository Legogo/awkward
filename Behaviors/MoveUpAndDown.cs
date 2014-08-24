using UnityEngine;
using System.Collections;

public class MoveUpAndDown : MonoBehaviour {

	Vector3 origin;
  public AnimationCurve curve;

	void Start(){
		origin = transform.position;
	}
	void Update () {
		transform.position = origin + Vector3.up * curve.Evaluate(Mathf.PingPong(Time.time * 0.25f, 0.2f));
	}
}
