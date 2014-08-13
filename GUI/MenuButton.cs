using UnityEngine;
using System.Collections;

public class MenuButton : MouseSelectionObject {

  override public void event_releaseOnObject(){
    action();
  }

  virtual public void action(){
    //...
  }
}
