using UnityEngine;
using System.Collections;

public class DeviceInfo : MonoBehaviour {

  public const int IPAD = 0;
  public const int IP5 = 1;

  static public bool is3GS(){
    return Camera.main.pixelHeight <= 480;
  }

  static public bool isIP5(){
    return getRatio() == 56; // 0.56f
  }

  static public int getRatio(){
    return Mathf.FloorToInt((Camera.main.pixelWidth / Camera.main.pixelHeight) * 100f);
  }

  public static Vector2 getMainGameViewSize()
  {
    System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
    System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
    System.Object Res = GetSizeOfMainGameView.Invoke(null,null);
    return (Vector2)Res;
  }

}
