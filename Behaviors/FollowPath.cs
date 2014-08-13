using UnityEngine;
using System.Collections;

public class FollowPath : MonoBehaviour {
  
  public Vector3[] path;
  public float speed = 1f;

  public Transform agent; // if null; current transform is used
  public bool lookAtNextPosition = false;
  
  protected int dir = 1;
  
  protected bool live = false;
  
  //float progress = 0f; //[0,1]
  protected int currentIndex = 0;
  
  virtual protected void Start () {
    if(agent == null) agent = transform;

    setOnPath(0);
    live = true;
  }
  
  virtual protected void Update () {
    if(!live) return;
    updatePosition();
  }
  
  protected void updatePosition(){

    agent.transform.position = Vector3.MoveTowards(agent.transform.position, path[getNextIndex()], speed * Time.deltaTime);
    if(nearNode()){
      //Debug.Log("NEAR NODE");
      if(dir > 0f)  nextNode();
      else  prevNode();
    }
  }
  
  public bool nearNode(){
    return Vector3.Distance(agent.transform.position, path[getNextIndex()]) < 0.01f;
  }
  
  public void switchDir(){
    currentIndex = getNextIndex();
    dir *= -1;
  }
  
  virtual public void play(){
    live = true;
  }
  virtual public void stop(){
    live = false;
  }
  
  protected void setOnPath(int index){
    currentIndex = index;
    agent.transform.position = path[currentIndex];
    if(lookAtNextPosition){
      Vector3 pos = getNextNodePosition();
      agent.transform.LookAt(pos);
    }
  }
  
  virtual protected void nextNode(){
    int newIndex = currentIndex+1;
    if(newIndex >= path.Length) newIndex = 0;
    setOnPath(newIndex);
  }
  
  protected void prevNode(){
    int newIndex = currentIndex-1;
    if(newIndex < 0)  newIndex = path.Length - 1;
    setOnPath(newIndex);
    //Debug.Log("New index = "+newIndex);
  }
  protected Vector3 getNextNodePosition(){
    return path[getNextIndex()];
  }
  protected int getNextIndex(){
    if(dir > 0) return (currentIndex + 1 >= path.Length) ? 0 : (currentIndex + 1);
    return (currentIndex - 1 < 0) ? path.Length-1 : (currentIndex - 1);
  }
  
  protected void OnDrawGizmos(){
    if(path == null)  return;
    if(path.Length < 1) return;
    
    Gizmos.color = Color.yellow;
    Gizmos.DrawCube(path[0], Vector3.one);
    
    Gizmos.color = Color.red;
    Vector3 prev = path[0];
    for(int i = 1; i < path.Length; i++){
      Gizmos.DrawLine(prev, path[i]);
      prev = path[i];
    }
    Gizmos.DrawLine(prev, path[0]);
    
    Gizmos.color = Color.magenta;
    //Gizmos.DrawSphere(path[getNextIndex()], 0.2f);
    
  }
  
  public bool DEBUG_DRAW = false;
  void OnGUI(){
    if(!DEBUG_DRAW) return;
    string content = "";
    content += "\nlive:"+live;
    content += "\nnodeIndex:"+currentIndex+"/"+(path.Length-1);
    GUI.TextArea(new Rect(10,10,300,300), content);
  }
}
