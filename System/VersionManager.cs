using UnityEngine;
using System.Collections;

public class VersionManager : MonoBehaviour {
	
  string website = ""; // when you click on version this website open

  public Font font;
  public float fontSize = 1f;

  public string version = "";
	TextMesh txt;

  float waitOnScreenTime = 2f;
  float fadeSpeed = 3f;

  void Awake(){
    DontDestroyOnLoad(gameObject);
    manager = this;

  }

  void Start(){
    StartCoroutine(createMesh());
  }
  
  IEnumerator createMesh(){
    txt = gameObject.AddComponent<TextMesh>();
    yield return new WaitForSeconds(0.1f);
    txt.font = font;
    txt.characterSize = fontSize;
    updateText();
    yield return new WaitForSeconds(0.1f);
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

    while(renderer.sharedMaterial.color.a < 1f){
      setAlpha(renderer.sharedMaterial.color.a + (Time.deltaTime * fadeSpeed));
      yield return null;
    }

    yield return new WaitForSeconds(waitOnScreenTime);

    while(renderer.sharedMaterial.color.a > 0f){
      setAlpha(renderer.sharedMaterial.color.a - (Time.deltaTime * fadeSpeed));
      yield return null;
    }
  }

  void setAlpha(float alpha){
    Color col = renderer.sharedMaterial.color;
    col.a = alpha;
    renderer.sharedMaterial.color = col;
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

    checkClick();
	}

  void checkClick(){
    if(txt.renderer.material.color.a <= 0f) return;

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
