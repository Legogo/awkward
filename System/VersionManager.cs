using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class VersionManager : MonoBehaviour {
	
  public string version = "";
  Renderer render;
	TextMesh txt;

  float waitOnScreenTime = 2f;
  float fadeSpeed = 3f;

  void Awake(){
    DontDestroyOnLoad(gameObject);
    manager = this;
  }

  void Start(){
    txt = gameObject.GetComponent<TextMesh>();
    render = txt.renderer;
    updateText();
    startFading();
  }

  void startFading(){
    updateText();

    if(txt.text.Length > 0){
      StartCoroutine("fadeProcess");
    }
  }

  IEnumerator fadeProcess(){
    setAlpha(0f);

    while(render.sharedMaterial.color.a < 1f){
      setAlpha(render.sharedMaterial.color.a + (Time.deltaTime * fadeSpeed));
      yield return null;
    }

    yield return new WaitForSeconds(waitOnScreenTime);

    while(render.sharedMaterial.color.a > 0f){
      setAlpha(render.sharedMaterial.color.a - (Time.deltaTime * fadeSpeed));
      yield return null;
    }
  }

  void setAlpha(float alpha){
    Color col = render.sharedMaterial.color;
    col.a = alpha;
    render.sharedMaterial.color = col;
  }

  int convertVersion(string v){
    string[] temp = v.Split('.');
    return 1 * int.Parse(temp[1]) * int.Parse(temp[2]);
  }
  
  void updateText(){
    txt.text = version;
  }

  public void toggleVersion(){
    StopCoroutine("fetchVersion");
    StopCoroutine("fadeProcess");

    startFading();
  }

	void Update(){
		if(Input.GetKeyUp(KeyCode.Equals)){
      toggleVersion();
		}
	}


  static public VersionManager manager;
}
