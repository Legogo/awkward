using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuiObject : MonoBehaviour {
	
	List<GameObject> renders;
	Vector3 camPosition;
	
	bool translateState = false;
	float fadeSpeed = 1f; // vitesse et direction de fading
	
	float eventTimer = 0f; // dans combien de temps sa modif va etre déclanchée
	float willFadeSpeedState = 0f; // (buffer de fadeSpeed)
	
	TextMesh text;
	
	//EDITOR REF
	public float timeBeforeEvent = -1f; // time before show/hide event
	public float willFadeSpeed = 0f; // fade at event time ?
	
	void Awake(){
		text = GetComponent<TextMesh>();
		
		GuiManager.addObject(this);
		
		renders =  new List<GameObject>();
		
		if(renderer != null)	renders.Add(gameObject);
		foreach(Transform t in transform){
			if(t.gameObject.renderer != null)	renders.Add(t.gameObject);
		}
		
	}
	
	void Start () {
		//ref
		camPosition = transform.position;
	}
	
	public void reset(){
		eventTimer = timeBeforeEvent;
		willFadeSpeedState = willFadeSpeed;
		
		fadeSpeed = 0f;
		setInitialVisiblity();
	}
	
	void setInitialVisiblity(){
		
		//re-affiche au reset
		if(eventTimer > -1){
			if(willFadeSpeed > 0){
				setAlpha(1);
			}else{
				setAlpha(0);
			}
			toggleVisible(true);
		}
	}
	
	public void setFaded(){
		setAlpha((fadeSpeed > 0) ? 1 : 0);
	}
	
	void Update(){
		
		updateShowTimer();
		
		updateFade();
		
		if(translateState){
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, camPosition, 1f * Time.deltaTime);
			
			if(Vector3.Distance(transform.localPosition, camPosition) < 1f){
				transform.localPosition = camPosition;
				translateState = false;
			}
			
			return;
		}
		
	}
	
	void updateShowTimer(){
		
		if(eventTimer > 0){
			eventTimer -= Time.deltaTime;
			
			//event end
			if(eventTimer <= 0){
				
				//fade at end ?
				if(willFadeSpeedState != 0){
					callFade(willFadeSpeedState);
				}else{
					
					//or not ...
					if(willFadeSpeedState >= 0)	toggleVisible(false);
					else toggleVisible(true);
				}
				
				eventTimer = -1f;
			}
		}
		
	}
	
	void updateFade(){
		if(fadeSpeed == 0){
			return;
		}
		
		float current = getAlpha();
		current += fadeSpeed * Time.deltaTime;
		
		current = Mathf.Max(0, current);
		current = Mathf.Min(current, 1);
		
		if(current <= 0 || current >= 1)	fadeSpeed = 0;
		
		//Debug.Log(fadeSpeed+","+current);
		setAlpha(current);
	}
	
	void setVisibleToCamera(){
		// Positionne l'objet par rapport à la camera en prenant ses coord d'origine
		Vector3 initPosition = transform.position;
		
		Camera cam = Camera.main.GetComponent<Camera>();
		transform.parent = cam.transform;
		transform.localPosition = initPosition;
	}
	
	public void callFade(float speed){
		fadeSpeed = speed;
		//Debug.Log(name+" called fading "+fadeSpeed);
	}
	public void callFade(float speed, Color newColor){
		if(renderer != null){
			newColor.a = 0f;
			setColor(newColor);
		}
		
		callFade(speed);
	}
	
	//show the element for an specific amount of time
	public void callTemporary(float time){ callTemporary(time, -1f); }
	public void callTemporary(float time, float fadeOutSpeed){
		eventTimer = time;
		willFadeSpeedState = fadeOutSpeed;
		
		toggleVisible(true);
		setAlpha(1);
		
		//Debug.Log(name+" called temporary for "+time);
	}
	
	public void callTranslate(){
		Vector3 outPosition = transform.localPosition;
		outPosition.y = -1;
		transform.localPosition = outPosition;
		translateState = true;
	}
	
	public void call(){
		toggleVisible(true);
	}
	
	public void kill(){
		toggleVisible(false);
	}
	
	public void toggleVisible(bool flag){
		
		foreach(GameObject go in renders){
			if(go.renderer != null){
				go.renderer.enabled = flag;
			}
		}
		
		//Debug.Log(name+" toggled "+flag);
	}
	
	public float getAlpha(){
		if(renderer == null){
			if(renders.Count <= 0)	return 0f;
			return renders[0].renderer.material.color.a;
		}
		return renderer.material.color.a;
			
	}
	
	public void setAlpha(float newAlpha){
		
		if(renders.Count <= 0){
			Debug.Log("NO RENDERER ON "+name);
			return;
		}
		
		Color col;
		foreach(GameObject obj in renders){
			col = obj.renderer.material.color;
			col.a = newAlpha;
			obj.renderer.material.color = col;
		}
		
		//Debug.Log(name+",renders count = "+renders.Count+", alpha = "+newAlpha);
	}
	
	void setColor(Color col){
		if(renderer == null){
			if(renders.Count <= 0)	return;
			
			foreach(GameObject obj in renders){
				obj.renderer.material.color = col;
			}
			return;
		}
		renderer.material.color = col;
	}
	
	public void toggleFade(float speed){
		float newSpeed = speed;
		
		if(fadeSpeed != 0){
			newSpeed = -Mathf.Sign(fadeSpeed) * speed;
		}else{
			newSpeed = ((getAlpha() > 0) ? -1 : 1) * speed;
		}
		
		callFade(newSpeed);
		//Debug.Log(newSpeed);
	}
	
	public void stopFading(){
		fadeSpeed = 0f;
	}
	
	public void callFadeDelay(float delay, float speed){
		stopFading();
		
		willFadeSpeedState = speed;
		eventTimer = delay;
		
		setInitialVisiblity();
	}
	
	public void setText(string newText){
		if(text != null){
			text.text = newText;
		}
	}
}
