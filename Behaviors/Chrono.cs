using UnityEngine;
using System.Collections;

public class Chrono : MonoBehaviour {
	
	float et; // starting time
	float totalFrames = 0;
	string currentCount = "";
	
	int state = 0;
	public static int IDLE = 0;
	public static int PLAY = 1;
	
	//to be used with textmesh
	Color currentColor = Color.white;
	
	Color normalColor = Color.white;
	Color warningColor = new Color(1f,0.5f,0f);
	Color losingColor = Color.red;
	
	TextMesh text;
	
	void Start () {
		text = GetComponent<TextMesh>();
		//setVisibleToCamera();
	}
	
	public void reset(float hour, float min, float sec){
		et = 0;
		totalFrames = 0;
		
		et = convertTime(hour,min,sec);
		play();
	}
	
	void setVisibleToCamera(){
		transform.parent = Camera.main.transform;
		Vector3 position = new Vector3(-27f,17f,30f);
		transform.localPosition = position;
	}

	string frameToTime(float levelTime, float aimTime){
	
		float timeLeft = aimTime - levelTime;
		string str = "";
		int hour = 0;
		int min = 0;
		int sec = 0;
		float msec = 0;
	
	
		if(timeLeft > 3600){
			hour = Mathf.FloorToInt(timeLeft/3600);
			timeLeft -= hour*3600;
		}
	
		if(timeLeft > 60){
			min = Mathf.FloorToInt(timeLeft/60);
			timeLeft -= min*60;
		}
	
		sec = Mathf.FloorToInt(timeLeft);
	
		msec = timeLeft - sec;
		msec = Mathf.FloorToInt(msec*100);
		//msec = Mathf.FloorToInt(msec/100)*100;
	
		if(hour > 0) str += hour;
		//str += "("+timeLeft+")"+min+":"+sec+":"+msec;
		str += min+":"+sec+":"+msec;
		//str = timeLeft.ToString();
	
		return str;
	}
	
	public void toggleplay(bool flag){
		if(flag)	play();
		else	stop();
	}
	
	public void additionnalTime(float _hour, float _min, float _sec){
		et += convertTime(_hour,_min,_sec);
	}
	
	public float convertTime(float _hour, float _min, float _sec){
		return ((_hour*3600)+(_min*60)+_sec);
	}
	
	public bool done(){
		if(et-totalFrames < 0)	return true;
		else return false;
	}
	
	void Update(){
		//totalFrames = Time.timeSinceLevelLoad
		if(state == PLAY){
			totalFrames += Time.deltaTime;
			currentCount = frameToTime(totalFrames,et);
			//print("system : "+Time.timeSinceLevelLoad);
			//print("local : "+ totalFrames);
		}
		
		//set color
		float diff = et - totalFrames;
		
		if(diff < 30)	currentColor = warningColor;
		else if(diff < 15)	currentColor = losingColor;
		else	currentColor = normalColor;
		
		text.renderer.material.color = currentColor;
		text.text = currentCount;
	}
	
	
	int getState(){return state;}
	void play(){state = PLAY;}
	void stop(){state = IDLE;}
	string getTime(){return currentCount;}
	
}
