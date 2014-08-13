using UnityEngine;
using System.Collections;

public class MenuButtonSprite : MouseSelectionObject {

  public Sprite[] states;
  SpriteRenderer render;

  override protected void Awake(){
    base.Awake ();
    render = GetComponent<SpriteRenderer>();
  }

  override public void event_out(){
    base.event_out();
    render.sprite = states[0];
  }
  override public void event_hover(){
    base.event_hover();
    render.sprite = states[1];
  }
  override public void event_click(){
    base.event_click();
    if(states.Length > 2) render.sprite = states[2];
    else render.sprite = states[1];
  }

  override public void event_releaseOnObject(){
    base.event_releaseOnObject();
    action();
  }
  
  virtual public void action(){
    //...
  }
}
