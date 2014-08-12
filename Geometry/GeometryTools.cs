using UnityEngine;
using System.Collections;

public class GeometryTools : MonoBehaviour {
	
	static public Mesh createPlane(string name, Vector3 a, Vector3 b, Vector3 c, Vector3 d){
		Mesh msh = new Mesh();
		msh.name = name;
		drawPlane(msh, a,b,c,d);
		return msh;
	}
	
	static public GameObject createPlaneObject(string name, Vector3 a, Vector3 b, Vector3 c, Vector3 d){
		GameObject obj = new GameObject(name);
		Mesh msh = createPlane("MSH_"+name,a,b,c,d);
		obj.AddComponent<MeshFilter>().mesh = msh;
		obj.AddComponent<MeshRenderer>();
		return obj;
	}
	
	/* c d
	 * |-|
	 * |\|
	 * |-|
	 * a b
	 * */
	static public void drawPlane(Mesh msh, Vector3 a, Vector3 b, Vector3 c, Vector3 d){
		//VERTICES (bottom, bottom, top, top)
		Vector3[] vert = { a, b, c, d };
		msh.vertices = vert;
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
		int[] tri = {0, 2, 1, 2, 3, 1};
		msh.triangles = tri;
		
		msh.RecalculateNormals();
	}
}
