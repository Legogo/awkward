using UnityEngine;
using System.Collections;

/*
 * 2012-11-26
 * # Manipulation et affichage d'une texture à l'écran
 * # Resize auto de la texture si l'écran est plus petit
 * # Centrage auto de la texture dans l'écran
 * */

public class GuiTexture : MonoBehaviour {
	
	public Texture textureToDisplay;
	public GameObject player;
	
	public bool USE_DRAW_DISTANCE = false;
	public Vector3 spawnPoint = Vector3.zero;
	public float spawnDistance = 2f; // spawn at 1.7
	float currentDistance = 0;
	
	public bool USE_DRAW_TIMER = false;
	public float drawTime = 3f; //sec
	float timer = 0f;
	
	public bool USE_TOUCHSOMETHING = false;
	
	float cantBeRemoved = 0f; // timer to force display for time
	
	Rect texRect = new Rect();
	
	bool draw = false;
	
	void Start(){
		
		if(textureToDisplay == null){
			Debug.LogWarning("GuiTexture need texture to be defined");
			enabled = false;
			return;
		}
		
		toggleVisible(false);
	}
	
	void Update () {
		
		draw = true;
		if(cantBeRemoved > 0){
      cantBeRemoved -= GameTime.deltaTime;
			return;
		}
		
		if(USE_DRAW_DISTANCE)	draw = nearStart();
		
		if(draw && USE_DRAW_TIMER){
      timer += GameTime.deltaTime;
			draw = (timer > drawTime);
		}
		
		if(USE_TOUCHSOMETHING)	checkTouchedSomething();
	}
	
	public void resizeEvent(){
		texRect = solveTextureSize(textureToDisplay);
		//Debug.Log(htpRect);
	}
	
	public void forceDisplay(){
		forceDisplay(3f);
	}
	public void forceDisplay(float time){
		toggleVisible(true);
		cantBeRemoved = time;
	}
	public void toggleVisible(bool flag){
		//Debug.Log(textureToDisplay.name+", toggle "+flag);
		if(flag)	resizeEvent();
		draw = flag;
		enabled = flag;
	}
	public bool isVisible(){
		return draw;
	}
	
	// Permet de savoir si l'user a bougé ou non
	void checkTouchedSomething(){
		if(Input.anyKey)	timer = 0;
		if(Input.GetAxis("Mouse X") != 0)	timer = 0;
		if(Input.GetMouseButton(0))	timer = 0;
	}
	
	Rect solveTextureSize(Texture tex){
		
		Rect rect = new Rect();
		
		float width = tex.width;
		float height = tex.height;
		
		float maxWidth = Screen.width;
		float maxHeight = Screen.height;
		
		if (height > maxHeight) {
			width = (maxHeight / height) * width;
			height = maxHeight;
		}
		
		if (width > maxWidth) {
			height = (maxWidth / width) * height;
			width = maxWidth;
		}
		
		rect.x = (Screen.width - width) * 0.5f;
		rect.y = (Screen.height - height) * 0.5f;
		rect.width = width;
		rect.height = height;
		
		return rect;
	}
	
	
	void OnGUI(){
		if(!draw) return;
		GUI.DrawTexture(texRect, textureToDisplay);
	}
	
	bool nearStart(){
		if(player == null)	return false;
		
		currentDistance = Vector3.Distance(player.transform.position, spawnPoint);
		return (currentDistance < spawnDistance);
	}
	
	
}
