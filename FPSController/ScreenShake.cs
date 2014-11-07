using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {
	
	public static ScreenShake instance = null;
	
	public float duration;
	public float maxStrength;
	float timeElapsed;
	
	Vector3 initPosition = Vector3.zero;
	
	void Awake () {
		if(!enabled)	return;
		instance = this;
		enabled = false;
	}
	
	void Update () {
		
		Vector3 pos = Random.insideUnitCircle * maxStrength;
		pos.x += initPosition.x;
		pos.y += initPosition.y;
		pos.z += initPosition.z;
		transform.localPosition = pos;
		
		if(duration < 0)	return;
		
		if(timeElapsed > duration){
			stop();
			return;
		}
		
		timeElapsed += Time.deltaTime;
	}
	
	void OnEnable(){
		timeElapsed = 0;
		initPosition = transform.localPosition;
	}
	
	public bool isOn(){
		return (timeElapsed > 0 || duration < 0);
	}
	
	public void launch(){
		enabled = true;
	}
	
	public void launch(float time, float strength){
		duration = time;
		maxStrength = strength;
		launch();
	}
	
	public void setWorse(float step){
		maxStrength += step;
	}
	
	public void stop(){
		enabled = false;
		transform.localPosition = initPosition;
	}
}
