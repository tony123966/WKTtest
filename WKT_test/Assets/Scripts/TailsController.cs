using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TailsController : MonoBehaviour {

	public List<GameObject> TailSplineList = new List<GameObject>();
	public List<Vector3> innerPointList = new List<Vector3>();
	public CatmullRomController catmullRomController;
	public GameObject tailControlPoint_clone;
	private LineRenderer lineRenderer;
	public float lineWidth = 1.0f;
	public int numberOfPoints = 10;
	public int number =2;
	public float length = 30;

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
	public void NewTailSpline()
	{
		if (catmullRomController.controlPointList.Count < 2) return;
		TailSplineList.Clear();
		Vector3 pos = catmullRomController.controlPointList[catmullRomController.controlPointList.Count - 1].position;
		Vector3 vector = (catmullRomController.controlPointList[catmullRomController.controlPointList.Count - 1].position - catmullRomController.controlPointList[0].position).normalized;
		for(int i=0;i<=number;i++){
			AddTailControlPoint(pos + i * vector * length);
		}
		TailSplineList[0].gameObject.GetComponent<SphereCollider>().enabled = false;
		TailSplineList[0].gameObject.GetComponent<MeshRenderer>().enabled = false;
		ControlPointSetting();
		DisplayCatmullromSpline();
		RederTailsSpline();
	}
	public void ResetCatmullRom()
	{
		ControlPointSetting();
		DisplayCatmullromSpline();
		RederTailsSpline();
	}
	public void SetCatmullRom()
	{
		if (catmullRomController.controlPointList.Count < 2) return;
		initTailsControlPoint();
		ControlPointSetting();
		DisplayCatmullromSpline();
		RederTailsSpline();
	}
	void AddTailControlPoint(Vector3 point)
	{
		GameObject clone;
		clone = Instantiate(tailControlPoint_clone, point, Quaternion.identity) as GameObject;
		clone.transform.parent = this.gameObject.transform;
		TailSplineList.Add(clone);
	}
	public void RederTailsSpline()
	{
		for (int i = 0; i < innerPointList.Count; i++)
		{
			lineRenderer.SetPosition(i, innerPointList[i]);
		}
	}
	void ControlPointSetting()
	{
		if (TailSplineList.Count < 2 || catmullRomController.controlPointList.Count<2)
		{
			lineRenderer.SetVertexCount(0);
		}
		else
		{
			lineRenderer.SetVertexCount(numberOfPoints * (TailSplineList.Count - 1));
		}
	}
	public void ShowControlPoint(bool isShow)
	{
		for (int i = 0; i < TailSplineList.Count; i++)
		{
			TailSplineList[i].gameObject.GetComponent<SphereCollider>().enabled = isShow;
			TailSplineList[i].gameObject.GetComponent<MeshRenderer>().enabled = isShow;
		}
	}
	public void MoveTailsControlPoint(GameObject obj, Vector3 point)
	{
		obj.transform.position = point;
		ResetCatmullRom();
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

		if (TailSplineList.Count<2)return;
		else if (TailSplineList.Count == 2) 
		{
			p0 = TailSplineList[0].transform.position;
			p1 = TailSplineList[0].transform.position;
			p2 = TailSplineList[1].transform.position;
			p3 = TailSplineList[1].transform.position;


			float segmentation = 1 / (float)numberOfPoints;
			float t = 0;
			for (int i = 0; i < numberOfPoints; i++)
			{
				Vector3 newPos = ReturnCatmullRomPos(t, p0, p1, p2, p3);
				innerPointList.Add(newPos);
				t += segmentation;
			}
		}

		for (int index = 0; index < TailSplineList.Count-1; index++)
			{
				if (index == 0)
				{
					p0 = TailSplineList[0].transform.position;
					p1 = TailSplineList[0].transform.position;
					p2 = TailSplineList[1].transform.position;
					p3 = TailSplineList[2].transform.position;
				}
				else if (index == TailSplineList.Count - 2)
				{
					p0 =TailSplineList[index -1].transform.position;
					p1 =TailSplineList[index].transform.position;
					p2 =TailSplineList[index+1].transform.position;
					p3 =TailSplineList[index+1].transform.position;
				}
				else
				{
					p0 = TailSplineList[index - 1].transform.position;
					p1 = TailSplineList[index].transform.position;
					p2 = TailSplineList[index + 1].transform.position;
					p3 = TailSplineList[index + 2].transform.position;
				}

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
	void initTailsControlPoint()
	{
		if (catmullRomController.controlPointList.Count == 0) return;
		Vector3 dis = catmullRomController.controlPointList[catmullRomController.controlPointList.Count - 1].position - TailSplineList[0].transform.position;

		for(int i=0;i<TailSplineList.Count;i++)
		{
			TailSplineList[i].transform.Translate(dis);
		}
	}
}
