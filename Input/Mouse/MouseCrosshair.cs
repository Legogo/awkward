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
	
	public Texture2D[] states;
  public bool DEBUG_SHOW = false;
  bool show = true;

	void Awake(){

		if(states.Length <= 0){
			states = new Texture2D[2];
			states[0] = (Texture2D)Resources.Load("Textures/GUI/cursor_normal");
			states[1] = (Texture2D)Resources.Load("Textures/GUI/cursor_action");
		}
		
    cursorSizeX = states[0].width;
    cursorSizeY = states[0].height;

    cursorInfo = new Rect(0f,0f,cursorSizeX,cursorSizeY);
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
		
		event__resizeScreen();
	}
	
	public void event__resizeScreen(){
    int w = (Mathf.CeilToInt (Camera.main.pixelWidth * 0.5f) - Mathf.CeilToInt(cursorSizeX * 0.5f));
    int h = (Mathf.CeilToInt (Camera.main.pixelHeight * 0.5f) - Mathf.CeilToInt(cursorSizeY * 0.5f));
    cursorInfo.x = w;
    cursorInfo.y = h;
    //Debug.Log (cursorInfo);
	}
	
  public void kill(){
    enabled = false;
  }

	void OnGUI(){
    if(!show)	return;
    GUI.DrawTexture(cursorInfo,states[cursorState]);
    if(!DEBUG_SHOW)  return;
    string content = "cursor state:"+cursorState;

    content += "\ncamera size("+Camera.main.pixelWidth+","+Camera.main.pixelHeight+")";
    content += "\ncursor position:"+cursorInfo.x+","+cursorInfo.y;
    content += "\ncursor size:"+cursorInfo.width+","+cursorInfo.height;

    GUI.Label(new Rect(0,0,200,100), content);
	}
}
