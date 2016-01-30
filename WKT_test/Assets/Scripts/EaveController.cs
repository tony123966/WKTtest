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
		ControlPointSetting();
		DisplayCatmullromSpline();
		RenderCatmullromSpline();
	}
	public void SetEaveSpline()
	{
		eaveSplineList.Clear();
		eaveSplineList.Add( catmullRomController.controlPointList[catmullRomController.controlPointList.Count-1]);
		for (int i = 0; i < catmullRomController.ringMirrorSplineList.Count; i++) 
		{
			eaveSplineList.Add(catmullRomController.ringMirrorSplineList[i].GetComponent<CatmullRomController>().controlPointList[catmullRomController.ringMirrorSplineList[i].GetComponent<CatmullRomController>().controlPointList.Count - 1].transform);
		}
	}
	void ControlPointSetting()
	{
		lineRenderer.SetVertexCount(numberOfPoints * (eaveSplineList.Count));
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
			p1 = eaveSplineList[(index+ eaveSplineList.Count) % eaveSplineList.Count].transform.position;
			p2 = eaveSplineList[(index + 1+ eaveSplineList.Count) % eaveSplineList.Count].transform.position;
			p3 = eaveSplineList[(index + 2 + eaveSplineList.Count) % eaveSplineList.Count].transform.position;
	
			float segmentation = 1 / (float)numberOfPoints;
			float t = 0;
			for (int i = 0; i < numberOfPoints; i++)
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
	}
	void InsertControlPoint()
	{

	}
}
