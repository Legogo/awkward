using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConsoleBase : MonoBehaviour {
	
	static public ConsoleBase instance;
	protected Camera cam;
	protected KeyCode toggleKey = KeyCode.Equals; // =
	
	protected List<string> lines = new List<string>();
	protected Hashtable systemInfo = new Hashtable();
	
	protected float speed = 10000f;
	
	protected Vector2 aim = Vector2.zero;
	protected Rect consoleRect = new Rect(10,10,0,0);
	protected Rect infoRect = new Rect(0,10,300,800);
	bool showInEditorConsole = true;

	virtual protected void Awake () {
		instance = this;
		
		cam = Camera.main;
		if(cam == null){
			enabled = false;
			return;
		}
		
		consoleRect.x = 10;
		consoleRect.y = 10;
		
		clear();
	}
	
	virtual protected void Update(){
		//if(Application.isEditor) 	return;
		
		//grow console frame
		consoleRect.width = Mathf.MoveTowards(consoleRect.width, aim.x, Time.deltaTime * speed);
		consoleRect.height = Mathf.MoveTowards(consoleRect.height, aim.y, Time.deltaTime * speed);
		
		//toggle console
		if(Input.GetKeyUp(toggleKey)){
			toggle ();
		}
	}
	
	void toggle(){
	
		if(aim.x == 0f){
			//taille - la marge
			aim.x = (Screen.width - consoleRect.x * 2f);
			aim.y = (Screen.height - consoleRect.y * 2f);
		}else{
			aim.x = aim.y = 0f;
		}
		
		infoRect.x = Screen.width - (Screen.width * 0.25f);
		
		//Debug.Log(aim);
	}
	
	void OnGUI(){
		if(consoleRect.width <= 0f)	return;
		
		string output = "";
		
		for(int i = lines.Count - 15; i < lines.Count; i++){
			if(i >= 0){
				output += "\n"+lines[i];
			}
		}
		
		//Debug.Log(currentRect+","+output);
		GUI.Label(consoleRect, output);
		
		output = "[SYSTEM]";
		foreach(DictionaryEntry entry in systemInfo){
			output += "\n"+entry.Key+" = "+entry.Value;
		}
		GUI.Label(infoRect, output);
	}
	
	public void addInfo(string name, string val){
		if(systemInfo == null)	systemInfo = new Hashtable();
		foreach(DictionaryEntry entry in systemInfo){
			if(entry.Key.ToString() == name){
				systemInfo[name] = val;
				return;
			}
		}
		systemInfo.Add(name, val);
	}

	public void addLine(string context, string content){
		if(context.Length <= 0)	addLine(content);
		else addLine("["+context+"] "+content);
	}
	public void addLine(string content){
		lines.Add(content);
		if(showInEditorConsole)	Debug.Log("*** "+content);
	}
	public void addErrorLine(string content){
		Debug.LogError(content);
		lines.Add(content);
	}

	public void clear(){
		lines.Clear();
		addLine("CONSOLE", "console cleared");
	}

  static public void add(string content){ get().addLine(content); }

  static public ConsoleBase get(){
    if(instance != null) return (ConsoleBase)instance;
    GameObject obj = GameObject.Find("_system");
    if(obj == null) obj = new GameObject("_system");
    instance = obj.GetComponent<ConsoleBase>();
    if(instance == null) instance = obj.AddComponent<ConsoleBase>();
    return (ConsoleBase)instance;
  }
}
