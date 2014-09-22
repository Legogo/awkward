
using UnityEngine;
using System.Collections;

public class SpriteTools {
  public SpriteRenderer createSprite(string name, Sprite spr){
    GameObject obj = new GameObject(name);
    SpriteRenderer s = obj.AddComponent<SpriteRenderer>();
    s.sprite = spr;
    return s;
  }
}