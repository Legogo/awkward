using UnityEngine;
using System.Collections;

public class SpriteAnimate : MonoBehaviour {
  public GameObject[] images;
  public float timer = 1f;
  float target = 0f;

  void Start(){
    target = timer;
    setupFrame(0);
  }

  void Update(){
    if(timer > 0f){
      timer -= Time.deltaTime;
      return;
    }

    nextFrame();

    timer = target;
  }

  void nextFrame(){
    for(int i = 0; i < images.Length; i++){
      if(images[i].activeSelf){
        setupFrame(i + 1);
        return;
      }
    }
  }

  void setupFrame(int index){
    if(index >= images.Length)  index = 0;
    //Debug.Log("set frame "+index);
    for(int i = 0; i < images.Length; i++){
      images[i].SetActive(i == index);
    }
  }
}