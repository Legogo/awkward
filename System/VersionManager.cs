using UnityEngine;
using System.Collections;

public class VersionManager : MonoBehaviour {
	
  string website = ""; // when you click on version this website open

  string webVersion = "";
  string localVersion = "";
	public TextMesh version; // set in editor

  float fadeSpeed = 1.2f;

  void Awake(){
    DontDestroyOnLoad(gameObject);
    manager = this;
  }

  void Start(){
    localVersion = version.text;
    setAlpha(0f);
  }

  void startFading(){
    updateText();

    if(version.text.Length > 0){
      StartCoroutine("fadeProcess");
    }
  }

  IEnumerator fadeProcess(){
    setAlpha(0f);

    while(renderer.material.color.a < 1f){
      setAlpha(renderer.material.color.a + (Time.deltaTime / fadeSpeed));
      yield return null;
    }

    yield return new WaitForSeconds(3f);

    while(renderer.material.color.a > 0f){
      setAlpha(renderer.material.color.a - (Time.deltaTime / fadeSpeed));
      yield return null;
    }
  }

  void setAlpha(float alpha){
    Color col = renderer.material.color;
    col.a = alpha;
    renderer.material.color = col;
  }

  int convertVersion(string v){
    string[] temp = v.Split('.');
    return 1 * int.Parse(temp[1]) * int.Parse(temp[2]);
  }
  
  void updateText(){
    version.text = localVersion;
  }

  void toggleVersion(){
    StopCoroutine("fetchVersion");
    StopCoroutine("fadeProcess");
    checkVersion();
  }

	void Update(){
		if(Input.GetKeyUp(KeyCode.V)){
      toggleVersion();
		}

    checkClick();
	}

  void checkClick(){
    if(version.renderer.material.color.a <= 0f) return;

    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit, 100)){
      //Debug.DrawLine(ray.origin, hit.point);
      if(Input.GetMouseButtonUp(0)){
        goGrabLastVersion();
      }
    }

  }

  void goGrabLastVersion(){
    Debug.Log("opening browser to grab last version");
    if(website.Length > 1) Application.OpenURL (website);
  }
	
  static public VersionManager manager;
}
