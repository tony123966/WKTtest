using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoofController : MonoBehaviour {
	MeshFilter meshFilter;
	public CatmullRomController catmullRomController;
	public List<GameObject> ringMirrorSplineList = new List<GameObject>();
	void Start()
	{
		meshFilter = GetComponent<MeshFilter>();
		if (!meshFilter)
		{
			gameObject.AddComponent<MeshFilter>();
			meshFilter = GetComponent<MeshFilter>();
		}
	}
	
	public void SetRoofMesh()
	{
		if( catmullRomController.controlPointList.Count<2)return;
		Mesh mesh=new Mesh();
		meshFilter.mesh=mesh;
		mesh.Clear();
		ringMirrorSplineList = catmullRomController.ringMirrorSplineList;
		int count=ringMirrorSplineList.Count * catmullRomController.innerPointList.Count;
		Vector3[] verticies = new Vector3[count];
		for(int i=0;i<ringMirrorSplineList.Count;i++)
		{
			for(int j=0;j<catmullRomController.innerPointList.Count;j++)
			{
				verticies[i * catmullRomController.innerPointList.Count + j] = ringMirrorSplineList[i].GetComponent<CatmullRomController>().innerPointList[j];
			}
		}

		int[] tri = new int[ringMirrorSplineList.Count *( catmullRomController.innerPointList.Count-1)*6];
		int index=0;
		for (int i = 0; i < ringMirrorSplineList.Count; i++)
		{
			for (int j = 0; j < catmullRomController.innerPointList.Count-1; j++)
			{
				tri[index] = (((i + ringMirrorSplineList.Count) % ringMirrorSplineList.Count) * catmullRomController.innerPointList.Count) + j;
				tri[index + 1] = (((i - 1 + ringMirrorSplineList.Count) % ringMirrorSplineList.Count) * catmullRomController.innerPointList.Count) + j;
				tri[index + 2] = (((i + ringMirrorSplineList.Count) % ringMirrorSplineList.Count) * catmullRomController.innerPointList.Count) + j + 1;
				tri[index + 3] = (((i - 1 + ringMirrorSplineList.Count) % ringMirrorSplineList.Count) * catmullRomController.innerPointList.Count) + j;
				tri[index + 4] = (((i - 1 + ringMirrorSplineList.Count) % ringMirrorSplineList.Count) * catmullRomController.innerPointList.Count) + j + 1;
				tri[index + 5] = (((i + ringMirrorSplineList.Count) % ringMirrorSplineList.Count) * catmullRomController.innerPointList.Count) + j + 1;
				index += 6;
			}
		}
		//Debug.Log(index);
		Vector3[] normals = new Vector3[count];
		Vector3 up=Vector3.up;
		for(int i=0;i<count;i++)
		{
			Vector3 binnormal = Vector3.Cross(up, verticies[i]).normalized;
			normals[i] = Vector3.Cross(verticies[i], binnormal);
		}

		mesh.vertices=verticies;
		mesh.triangles=tri;
		mesh.normals=normals;
	}
}
