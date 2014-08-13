using UnityEngine;
using System.Collections;

public class ObjectBumpCos : MonoBehaviour {

  Vector3 scale;
  float progress = 0f;
  public float strength = 0.1f;

  void Start(){
    scale = transform.localScale;
  }

  void Update(){

    progress += Time.deltaTime * 5f;
    if(progress > Mathf.PI * 2f)  progress = 0f;

    transform.localScale = scale + Vector3.one * (Mathf.Cos(progress) * strength);
  }
}
