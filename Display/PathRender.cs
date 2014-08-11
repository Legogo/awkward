
using UnityEngine;
using System.Collections;

public class PathRender : MonoBehaviour {

  public Texture tex;
  GameObject[] quads;
  public Vector3[] nodes;

  void Start () {
    quads = new GameObject[nodes.Length];
    for (int i = 0; i < nodes.Length; i++) {
      quads[i] = createQuad(i);
    }

    updatePath();
  }

  public void updatePath(){
    for (int i = 0; i < nodes.Length; i++) {
      GameObject q = quads[i];

      int nextIdx = i+1;
      if(nextIdx >= nodes.Length) nextIdx = 0;
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

    GameObject.DestroyImmediate(obj.GetComponent<MeshCollider>());

    return obj;
  }

  void Update () {

    updatePath();

    for (int i = 0; i < quads.Length; i++) {
      Vector2 offset = quads[i].renderer.material.GetTextureOffset("_MainTex");
      offset.x -= Time.deltaTime * 1f;
      quads[i].renderer.material.SetTextureOffset("_MainTex", offset);
    }
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