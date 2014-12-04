using UnityEngine;
using System.Collections;

public class ColorTo : MonoBehaviour {

  TextMesh txt;
  Material mat;

  Color c_start = Color.white;
  Color c_end = Color.white;

  float timeTarget = 0f;
  float progress = 0f;

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
    Color c = Color.Lerp(c_start, c_end, Mathf.Lerp(0f,1f,Mathf.InverseLerp(0f, timeTarget, progress)));
    if(txt != null) txt.color = c;
    if(mat != null) mat.color = c;
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

  public void call(Material targetMat, Color from, Color to, float time){
    mat = targetMat;
    call (from, to, time);
  }
  public void call(Color from, Color to, float time){
    c_start = from;
    c_end = to;
    timeTarget = time;
    enabled = true;
  }


  static public ColorTo call(GameObject obj, Color from, Color to, float time){
    ColorTo c = obj.AddComponent<ColorTo>();
    c.call(from, to, time);
    return c;
  }
  static public ColorTo call(GameObject obj, Material mat, Color from, Color to, float time){
    ColorTo c = obj.AddComponent<ColorTo>();
    c.call(mat, from, to, time);
    return c;
  }
}
