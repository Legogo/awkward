
using UnityEngine;
using System.Collections;

/// <summary>
/// Path render.
/// Created by André Berlemont
/// 
/// This script will create some lines (made from native quads) between points (vector3) of a path
/// </summary>

public class PathRender : MonoBehaviour {

  public Texture tex; // texture to scroll on surface
  public bool closePath = false; // will close the path (connection btw last point & first point)
  public bool isStatic = true; // will regenerate path at each frame
 
  GameObject[] quads;
  public Vector3[] nodes;

  void Start () {
    quads = new GameObject[0];

    checkQuads();
    updatePath();

  }

  void checkQuads(){
    if(quads.Length < nodes.Length){
      GameObject[] newQuads = new GameObject[nodes.Length];
      //transfert old quads
      for (int i = 0; i < quads.Length; i++) {
        newQuads[i] = quads[i];
      }
      //complete with new ones
      for (int i = 0; i < newQuads.Length; i++) {
        if(newQuads[i] == null){
          newQuads[i] = createQuad(i);
        }
      }

      quads = newQuads;
    }

    //hide unused
    for (int i = 0; i < quads.Length; i++) {
      int maxQuad = (closePath) ? nodes.Length : nodes.Length-1;
      if(i >= maxQuad)  quads[i].gameObject.SetActive(false);
      else quads[i].gameObject.SetActive(true);
    }
  }

  public void assignPath(Vector3[] newNodes){
    nodes = newNodes;
    checkQuads();
    updatePath();
  }

  void updatePath(){
    for (int i = 0; i < nodes.Length; i++) {
      GameObject q = quads[i];

      int nextIdx = i+1;
      if(nextIdx >= nodes.Length){
        if(!closePath) return;
        nextIdx = 0;
      }
      Vector3 diff = (nodes[nextIdx] - nodes[i]);
      q.transform.position = nodes[i] + diff * 0.5f;
      q.transform.LookAt(nodes[nextIdx]);

      //float angle = (i >= nodes.Length - 1) ? -90f : 90f;
      q.transform.Rotate(transform.up, 90f);
      q.transform.Rotate(transform.right, 90f);

      Vector3 sc = q.transform.localScale;
      sc.x = diff.magnitude;
      q.transform.localScale = sc;

      Vector2 tile = q.renderer.material.GetTextureScale("_MainTex");
      tile.x = sc.x;
      q.renderer.material.SetTextureScale("_MainTex", tile);
    }
  }

  GameObject createQuad(int id){
    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Quad);

    obj.name = "path_"+id;
    obj.transform.parent = transform;
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localScale = new Vector3(1f,0.1f,1f);
    //obj.transform.Rotate(obj.transform.right, 90f);
    obj.renderer.material = new Material(Shader.Find("Transparent/Diffuse"));
    obj.renderer.material.mainTexture = tex;

    obj.gameObject.SetActive(false);
    GameObject.DestroyImmediate(obj.GetComponent<MeshCollider>());

    //Debug.Log("create node "+id);
    return obj;
  }

  void Update () {

    //update scrolling animation if texture exist
    if(tex != null){
      for (int i = 0; i < quads.Length; i++) {
        Vector2 offset = quads[i].renderer.material.GetTextureOffset("_MainTex");
        offset.x -= Time.deltaTime * 1f;
        quads[i].renderer.material.SetTextureOffset("_MainTex", offset);
      }
    }

    if(!isStatic) updatePath();
  }

  void OnDrawGizmos(){
    Color col = Color.red;
    Gizmos.color = col;
    for (int i = 0; i < nodes.Length; i++) {
      Gizmos.DrawSphere(nodes[i], 0.02f);
      col.r = Mathf.InverseLerp(0, nodes.Length, i);
      Gizmos.color = col;
    }
  }
}