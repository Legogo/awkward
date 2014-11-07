using UnityEngine;
using System.Collections;

public class MovementFly : Movement {
  override protected void Awake () {
    base.Awake();
    enabled = false;
  }

  public void toggle(){
    enabled = !enabled;
    GetComponent<MovementRigid>().enabled = !enabled;
    collider.enabled = !enabled;
  }

  override protected void solveDirection(){
    applyDirection = vertical.TransformDirection(moveDirection);
  }

  protected override void update__gravity ()
  {
    //no gravity
  }

  protected override void update__checkFall ()
  {
    //no event on colision !
  }
}
