using UnityEngine;
using System.Collections;

public class RoofController : MonoBehaviour {
	MeshFilter meshFilter;
	void Start()
	{
		meshFilter = GetComponent<MeshFilter>();
		if (!meshFilter)
		{
			gameObject.AddComponent<MeshFilter>();
			meshFilter = GetComponent<MeshFilter>();
		}
		//CreateRoofTriangle(); 
	}
	
	void CreateRoofTriangle()
	{
		Mesh mesh=new Mesh();
		meshFilter.mesh=mesh;
		Vector3[] verticies=new Vector3[3]
		{
			new Vector3(0,0,0),new Vector3(0,10,0),new Vector3(10,10,0)
		};

		int[] tri=new int[3];
		tri[0] = 0;
		tri[1] = 1;
		tri[2] = 2;

		Vector3[] normals=new Vector3[3];
		normals[0] = -Vector3.forward;
		normals[1] = -Vector3.forward;
		normals[2] = -Vector3.forward;
		
		Vector2[] uv=new Vector2[3];
		uv[0] = new Vector2(0,0);
		uv[0] = new Vector2(1, 0);
		uv[1] = new Vector2(0, 1);

		mesh.vertices=verticies;
		mesh.triangles=tri;
		mesh.normals=normals;
		mesh.uv=uv;
	}
}
