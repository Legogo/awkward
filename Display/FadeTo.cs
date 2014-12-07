using UnityEngine;
using System.Collections;

public class FadeTo : MonoBehaviour {

  TextMesh txt;
  Material mat;

  float start = 0f;
  float end = 0f;

  float timeTarget = 0f;
  float progress = 0f;

  Color c;

	void Awake(){
    txt = gameObject.GetComponent<TextMesh>();
    if(txt == null) mat = renderer.material;

    enabled = false;
  }

	void Update(){

    progress += Time.deltaTime;
    progress = Mathf.Clamp(progress, 0f, timeTarget);
    update_display();

    if(isDone()) destroy(true);
  }

  void update_display(){
    float result = Mathf.Lerp(start, end, Mathf.Lerp(0f,1f,Mathf.InverseLerp(0f, timeTarget, progress)));
    if(txt != null){
      c = txt.color;
      c.a = result;
      txt.color = c;
    }
    if(mat != null){
      c = mat.color;
      c.a = result;
      mat.color = c;
    }
  }

  bool isDone(){
    return progress >= timeTarget;
  }

  public void destroy(bool finishProcess){
    if(finishProcess){
      progress = timeTarget;
      update_display();
    }
    GameObject.DestroyImmediate(this);
  }

  public void call(Material targetMat, float from, float to, float time){
    mat = targetMat;
    call (from, to, time);
  }
  public void call(float from, float to, float time){
    start = from;
    end = to;
    timeTarget = time;
    enabled = true;
  }


  static public FadeTo call(GameObject obj, float from, float to, float time){
    FadeTo tmp = obj.AddComponent<FadeTo>();
    tmp.call(from, to, time);
    return tmp;
  }
  static public FadeTo call(GameObject obj, Material mat, float from, float to, float time){
    FadeTo tmp = obj.AddComponent<FadeTo>();
    tmp.call(mat, from, to, time);
    return tmp;
  }
}
