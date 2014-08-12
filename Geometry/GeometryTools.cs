using UnityEngine;
using System.Collections;

public class GeometryTools : MonoBehaviour {
	
	static public Mesh createPlaneMesh(string name, Vector3 a, Vector3 b, Vector3 c, Vector3 d){
		Mesh msh = new Mesh();
		msh.name = name;
		drawPlane(msh, a,b,c,d);
		return msh;
	}
	
	static public GameObject createPlaneGameObject(string name, Vector3 a, Vector3 b, Vector3 c, Vector3 d){
		GameObject obj = new GameObject(name);
    Mesh msh = createPlaneMesh("MSH_"+name,a,b,c,d);
		obj.AddComponent<MeshFilter>().mesh = msh;
		obj.AddComponent<MeshRenderer>();
		return obj;
	}

  static public GameObject getStandardPlaneCenter(string name, float size){
    size *= 0.5f;
    return GeometryTools.createPlaneGameObject(name, new Vector3(-size, -size, 0f), new Vector3(size, -size, 0f), new Vector3(size, size, 0f), new Vector3(-size, size, 0f));
  }
  static public GameObject getStandardPlane(string name, float size){
    return GeometryTools.createPlaneGameObject(name, new Vector3(0f, 0f, 0f), new Vector3(size, 0f, 0f), new Vector3(size, size, 0f), new Vector3(0f, size, 0f));
  }
	
	/* c d
	 * |-|
	 * |\|
	 * |-|
	 * a b
	 * */
	static public void drawPlane(Mesh msh, Vector3 a, Vector3 b, Vector3 c, Vector3 d){
		//VERTICES (bottom, bottom, top, top)
    Vector3[] vertx = {a, b, c, d};
    msh.vertices = vertx;
		msh.RecalculateNormals();
		
		//UVS
		Vector2[] uvs = {
			new Vector2(0f, 0f), 
			new Vector2(0f, 1f),
			new Vector2(1f, 1f), 
			new Vector2(1f, 0f)
		};
		msh.uv = uvs;
		
		//TRIANGLES
		int[] tri = {0,1,2,0,2,3};
		msh.triangles = tri;
		
		msh.RecalculateNormals();
	}
}
