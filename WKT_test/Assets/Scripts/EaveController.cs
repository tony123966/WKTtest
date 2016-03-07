using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EaveController : MonoBehaviour {

	public CatmullRomController catmullRomController;
	public List<Transform> eaveSplineList = new List<Transform>();
	public List<Vector3> innerPointList = new List<Vector3>();
	public GameObject eaveControlPoint_clone;
	private LineRenderer lineRenderer;

	public int numberOfPoints = 10;
	public float lineWidth = 1.0f;
	public List<GameObject> eaveControlPointList = new List<GameObject>();
	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		if (!lineRenderer)
		{
			gameObject.AddComponent<LineRenderer>();
			lineRenderer = GetComponent<LineRenderer>();
		}
		lineRenderer.useWorldSpace = true;
		lineRenderer.SetColors(Color.white, Color.white);
		lineRenderer.SetWidth(lineWidth, lineWidth);
		lineRenderer.SetVertexCount(0);
	}
	public void SetCatmullRom()
	{
		SetEaveSpline();
		DisplayCatmullromSpline();
		ControlPointSetting();
		RenderCatmullromSpline();
		ShowControlPoint(false);
	}
	public void ResetCatmullRom()
	{	
		DisplayCatmullromSpline();
		ControlPointSetting();
		RenderCatmullromSpline();
		ShowControlPoint(false);
	}
	public void SetEaveSpline()
	{
		eaveSplineList.Clear();
		for (int i = 0; i < catmullRomController.ringMirrorSplineList.Count; i++) 
		{
			eaveSplineList.Add(catmullRomController.ringMirrorSplineList[i].GetComponent<CatmullRomController>().controlPointList[catmullRomController.ringMirrorSplineList[i].GetComponent<CatmullRomController>().controlPointList.Count - 1].transform);
		}
		ResetEaveControlPoint();
		eaveControlPointList.Clear();
		for(int i=0;i<catmullRomController.ringMirrorSplineList.Count;i++)
		{
			GameObject clone;
			clone=InsertControlPoint(0.5f * (eaveSplineList[(i * 2 + eaveSplineList.Count) % eaveSplineList.Count].position + eaveSplineList[(i * 2+1 + eaveSplineList.Count) % eaveSplineList.Count].position));
			eaveSplineList.Insert(1 + i * 2, clone.transform);
			eaveControlPointList.Add(clone);
		}
	}
	void ControlPointSetting()
	{
		lineRenderer.SetVertexCount(numberOfPoints * (eaveSplineList.Count)+1);
	}
	Vector3 ReturnCatmullRomPos(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		Vector3 pos = 0.5f * ((2f * p1) + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t + (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t);
		return pos;
	}
	void DisplayCatmullromSpline()
	{
		innerPointList.Clear();
		Vector3 p0, p1, p2, p3;

		if (eaveSplineList.Count <= 2) return;

		for (int index = 0; index < eaveSplineList.Count; index++)
		{

			p0 = eaveSplineList[(index - 1 + eaveSplineList.Count) % eaveSplineList.Count].transform.position;
			p1 = eaveSplineList[(index + eaveSplineList.Count) % eaveSplineList.Count].transform.position;
			p2 = eaveSplineList[(index + 1 + eaveSplineList.Count) % eaveSplineList.Count].transform.position;
			p3 = eaveSplineList[(index + 2 + eaveSplineList.Count) % eaveSplineList.Count].transform.position;
	
			float segmentation = 1 / (float)(numberOfPoints);
			float t = 0;
			for (int i = 0; i <numberOfPoints; i++)
			{
				Vector3 newPos = ReturnCatmullRomPos(t, p0, p1, p2, p3);
				innerPointList.Add(newPos);
				t += segmentation;
			}
		}
	}
	void RenderCatmullromSpline()
	{
		
		for (int i = 0; i < innerPointList.Count; i++)
		{
			lineRenderer.SetPosition(i, innerPointList[i]);
		}
		lineRenderer.SetPosition(innerPointList.Count, innerPointList[0]);//?????
	}

	GameObject InsertControlPoint(Vector3 pos)
	{
		GameObject clone;
		clone = Instantiate(eaveControlPoint_clone, pos, eaveControlPoint_clone.transform.rotation) as GameObject;
		clone.transform.parent = gameObject.transform;

		return clone;
	}
	void ResetEaveControlPoint()
	{
		for (int i = 0; i < eaveControlPointList.Count; i++)
		{
			Destroy(eaveControlPointList[i]);
		}
	}
	public void MoveAllEaveControlPoint(Vector3 dis)
	{
		for(int i=0;i<eaveControlPointList.Count;i++){
			eaveControlPointList[i].transform.Translate(dis);
		}
		ResetCatmullRom();
	}
	public void ShowControlPoint(bool isShow)
	{
		for (int i = 1; i < eaveControlPointList.Count; i++)
		{
			eaveControlPointList[i].gameObject.GetComponent<SphereCollider>().enabled = isShow;
			eaveControlPointList[i].gameObject.GetComponent<MeshRenderer>().enabled = isShow;
		}
	}
}
