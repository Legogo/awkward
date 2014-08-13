using UnityEngine;
using System.Collections;

public class GraphicTools : MonoBehaviour {
  
  static public float setAlpha(Material m, float alpha){
    if(m == null) return alpha;
    //if(m.color == null){ Debug.LogWarning(m.name);return; }
    alpha = Mathf.Clamp(alpha,0f,1f);
    Color col = m.color;
    col.a = alpha;
    m.color = col;
    return alpha;
    //Debug.Log(m.name+","+m.color.a);
  }

}
