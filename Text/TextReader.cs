
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class TextReader : MonoBehaviour {

  string pathDatas = "Text/trad_FR";
  string[] datas;

  void Awake(){
    datas = treatText(pathDatas);
  }

  string[] treatText(string path){
    string[] temp;
    TextAsset p = (TextAsset)Resources.Load(path);
    temp = p.text.Split('\n');
    for (int i = 0; i < temp.Length; i++) { temp[i] = temp[i].Replace("~ ", System.Environment.NewLine); }
    return temp;
  }
  
  public string[] getDialog (string name) {
    List<string> list = new List<string> ();
    for (int i = 0; i < datas.Length; i++) {
      if (datas[i].Split('_')[0] == name) list.Add(datas[i]);
    }
    return list.ToArray ();
  }
  
  static public TextReader get(){
    GameObject obj = GameObject.Find("[SYSTEM]");
    if(obj == null) return null;
    return obj.GetComponent<TextReader>();
  }

  /*
  void OnGUI(){
    string[] info;

    if(info == null)  return;

    scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, 
      GUILayout.Width(Screen.width), GUILayout.Height(Screen.height)
    );

    //GUILayout.BeginArea(new Rect(0,0,600,600));
    for (int i = 0; i < info.Length; i++) {
      string line = info[i];
      string[] temp = line.Split('\t');


      //GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal(GUILayout.Height(50f));
      for (int j = 0; j < temp.Length; j++) {
        GUILayout.TextArea(temp[j], GUILayout.Width(250f), GUILayout.Height(50f));
      }
      GUILayout.EndHorizontal();
    }

    GUILayout.EndScrollView ();
  }
  */

  // Wrap text by line height
  static public string resolveTextSize(string input, int lineLength){
    
    // Split string by char " "       
    string[] words = input.Split(" "[0]);
    
    // Prepare result
    string result = "";
    
    // Temp line string
    string line = "";
    
    // for each all words        
    foreach(string s in words){
      // Append current word into line
      string temp = line + " " + s;
      
      // If line length is bigger than lineLength
      if(temp.Length > lineLength){
        
        // Append current line into result
        result += line + "\n";
        // Remain word append into new line
        line = s;
      }
      // Append current word into current line
      else {
        line = temp;
      }
    }
    
    // Append last line into result      
    result += line;
    
    // Remove first " " char
    return result.Substring(1,result.Length-1);
  }
}

