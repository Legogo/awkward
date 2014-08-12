using UnityEngine;
using System.Collections;

public class MouseSensivity : MonoBehaviour {
	
	public float factor = 1f;
	Vector2 limits = Vector2.zero;
	float show = 0f;
	
	void Start(){
		limits.x = 0.1f;
		limits.y = 3f;
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.O)){
			switchValue(-0.10f);
		}else if(Input.GetKey(KeyCode.P)){
			switchValue(0.10f);
		}
	}
	
	void switchValue(float step){
		factor += step;
		
		factor *= 10f;
		factor = Mathf.Floor(factor);
		factor /= 10f;
		
		factor = Mathf.Max(factor, limits.x);
		factor = Mathf.Min(factor, limits.y);
		
		show = 1f;
	}
	
	void OnGUI(){
		if(show <= 0f)	return;
		show -= Time.deltaTime;
		GUI.TextField(new Rect(10,10,100,30), "sensivity : "+factor);
	}
}
