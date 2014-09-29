using UnityEngine;
using System.Collections;
/*
 * Il faut que le script soit éxécuter en -1 dans l'ordre pour éviter les problèmes avec la réassignation du NORMAL
 * fix renamed class to MouseCursor
 * il faut ref deux textures dans "states" en public
 * 
 * */
public class MouseCrosshair : MonoBehaviour {
	
	static public int cursorState = 0;
	static public int NORMAL = 0;
	static public int ACTION = 1;
	
	int cursorSizeX = 16;
	int cursorSizeY = 16;
	
	Rect cursorInfo;
	
  Texture2D reference;
  public bool followMouse = true;
	public Texture2D[] states;
  public bool DEBUG_SHOW = false;
  bool show = true;

	void Awake(){

    if(states.Length > 0){
      getReference();
      cursorSizeX = reference.width;
      cursorSizeY = reference.height;
    }

    cursorInfo = new Rect(0f,0f,cursorSizeX,cursorSizeY);
  }

  //because first cell of array might be empty
  Texture2D getReference(){
    for (int i = 0; i < states.Length; i++) {
      if(states[i] != null){
        reference = states[i];
        return reference;
      }
    }
    return null;
  }

  void Start(){
    event__resizeScreen();
		//Debug.Log("Cursor >> size : "+cursorSize+", cursor states count = "+states.Length);
	}

	public void toggle(bool flag){
    show = flag;
	}
	
	void Update(){
		//state will be changed by other scripts
		cursorState = NORMAL;
	  
    if(followMouse){
      cursorInfo.x = Input.mousePosition.x;
      cursorInfo.y = Camera.main.pixelHeight - Input.mousePosition.y;
    }

		event__resizeScreen();
	}
	
	public void event__resizeScreen(){
    int w = (Mathf.CeilToInt (Camera.main.pixelWidth * 0.5f) - Mathf.CeilToInt(cursorSizeX * 0.5f));
    int h = (Mathf.CeilToInt (Camera.main.pixelHeight * 0.5f) - Mathf.CeilToInt(cursorSizeY * 0.5f));

    //center of screen
    if(!followMouse){
      cursorInfo.x = w;
      cursorInfo.y = h;
    }

    //Debug.Log (cursorInfo);
	}
	
  public void kill(){
    enabled = false;
  }

	void OnGUI(){
    if(!show)	return;

    //can't draw cursor if array of texture doesn't have enought textures
    if(states.Length < cursorState) return;
    if(states[cursorState] != null){
      GUI.DrawTexture(cursorInfo,states[cursorState]);
    }

    if(!DEBUG_SHOW)  return;
    string content = "cursor state:"+cursorState;
    content += (cursorState < states.Length) ? "\ncursor state texture ? "+states[cursorState] : "\nnot enought texture for that state !";

    content += "\ncamera size("+Camera.main.pixelWidth+","+Camera.main.pixelHeight+")";
    content += "\ncursor position:"+cursorInfo.x+","+cursorInfo.y;
    content += "\ncursor size:"+cursorInfo.width+","+cursorInfo.height;

    GUI.TextField(new Rect(0,0,400,150), content);
	}

  static public void setState(int newState){
    cursorState = newState;
  }
}
