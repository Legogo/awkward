using UnityEngine;
using System.Collections;

/// <summary>
/// Plane fader.
/// Written by André BERLEMONT
/// 
/// This script create a plane in front of the camera to have simple fading
/// User can specify speed,color,callback when calling PlaneFader.get().fadein(speed,color,callback);
/// 
/// </summary>

public class PlaneFader : MonoBehaviour {

  GameObject plane;
  float target = 0f;

  Camera cam;

  float speedIn = 0.5f; // fade target = 0 (start)
  float speedOut = 0.5f; // fade target = 1 (mort)
  float delay = 0f;
  
  public Color startColor = Color.white;
  public bool startFadedIn = false;
  public bool fadeOutAtStart = false;

  public delegate void delegateCallBack();
  delegateCallBack onProcessDone;

  void Awake(){
    cam = gameObject.GetComponent<Camera>();
    if(cam == null) cam = Camera.main;

    //create fader
    plane = GameObject.Find("fadePlane");
    if(plane == null){
      plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
      plane.name = "fadePlane";
      plane.renderer.material = new Material(Shader.Find("Unlit/TransparentColor"));
      plane.renderer.material.color = startColor;
      plane.transform.Rotate(-Vector3.right * 90f);
      plane.transform.localScale = Vector3.one * 20f;
      plane.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y ,cam.transform.position.z + 5f);
      plane.layer = cam.gameObject.layer;
    }
    
    if(startFadedIn)  setAlpha(1f);
    else setAlpha(0f);

    #if !UNITY_EDITOR
    DEBUG = false;
    #endif

    AudioListener.volume = 1f;

    enabled = false;
    init();
  }

  void init(){
    if(fadeOutAtStart)  fadeOut(0.5f);
  }
  
  void Update(){

    //create a fake lag
    if(delay > 0f){
      delay -= Time.deltaTime;
      if(delay < 0f){
        init();
      }
      return;
    }
    
    float current = getAlpha();
    float speed = getSpeed();
    //Debug.Log(current);
    if(current != target){
      setAlpha(Mathf.MoveTowards(current, target, Time.deltaTime * speed));
      return;
    }

    if(target <= 0f)  plane.renderer.enabled = false;
    enabled = false;
    
    event_done();
  }

  float getSpeed(){
    return (target < 1f) ? speedOut : speedIn;
  }

  void event_done(){
    if(DEBUG) Debug.Log("done fading");
    if(onProcessDone != null){
      onProcessDone();
      onProcessDone = null;
    }
  }

  void setAlpha(float newAlpha){
    Color col = plane.renderer.material.color;
    col.a = newAlpha;
    plane.renderer.material.color = col;
  }

  float getAlpha(){ return plane.renderer.material.color.a; }

  public void fadeIn(float speed, Color col, delegateCallBack cb){
    speedIn = speed;

    float alpha = getAlpha();
    col.a = alpha;
    plane.renderer.material.color = col;
    onProcessDone = cb;

    target = 1f;

    if(DEBUG) Debug.Log("fade in ! speed:"+speedIn+"/"+speedOut+", "+getAlpha()+"/"+target);

    plane.renderer.enabled = true;
    enabled = true;
  }
  public void fadeOut(float speed){
    speedOut = speed;
    onProcessDone = null; // kill previous callbacks

    target = 0f;

    plane.renderer.enabled = true;
    enabled = true;

    if(DEBUG) Debug.Log("fade out ! "+getAlpha()+"/"+target);
  }

  public bool isFadedOut(){
    return getAlpha() <= 0f;
  }
  public bool isDone(){
    if(!enabled) return true;
    return false;
  }

  public bool DEBUG = false;
  void OnGUI(){
    if(!DEBUG)  return;
    string content = "";
    content += "fade ? "+getAlpha();
    content += "\nspeed ? "+getSpeed();
    content += "\ntarget ? "+target;
    GUI.TextArea(new Rect(10,10,300,300), content);
  }

  static public PlaneFader get(){
    return (PlaneFader)GameObject.FindObjectOfType(typeof(PlaneFader));
  }
}
