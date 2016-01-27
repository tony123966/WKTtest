using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatmullRomManager : MonoBehaviour
{

	public List<Transform> controlPointList = new List<Transform>();
	public List<Vector3> totalPointList = new List<Vector3>();
	public GameObject controlPoint;
	public bool isLoop = false;
	public float lineWidth = 1.0f;
	public int numberOfPoints = 50;
	public int neartestIndex = 0;
	private LineRenderer lineRenderer;

	void Start()
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


	}
	void FixedUpdate()
	{
		if (controlPointList.Count < 4) { lineRenderer.SetVertexCount(0);return; }
		if (isLoop) lineRenderer.SetVertexCount(numberOfPoints * (controlPointList.Count));
		else lineRenderer.SetVertexCount(numberOfPoints * (controlPointList.Count - 3));
		DisplayCatmullromSpline();
		RenderCatmullromSpline();
	}

	Vector3 ReturnCatmullRomPos(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		Vector3 pos = 0.5f * ((2f * p1) + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t + (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t);
		return pos;
	}
	void DisplayCatmullromSpline()
	{
		totalPointList.Clear();
		for (int index = 0; index < controlPointList.Count; index++)
		{
			if (!isLoop)
			{
				if ((index == 0 || index == controlPointList.Count - 2 || index == controlPointList.Count - 1))
				{
					continue;
				}
			}
			Vector3 p0 = controlPointList[(index - 1 + controlPointList.Count) % controlPointList.Count].transform.position;
			Vector3 p1 = controlPointList[(index + controlPointList.Count) % controlPointList.Count].transform.position;
			Vector3 p2 = controlPointList[(index + 1 + controlPointList.Count) % controlPointList.Count].transform.position;
			Vector3 p3 = controlPointList[(index + 2 + controlPointList.Count) % controlPointList.Count].transform.position;

			float segmentation = 1 / (float)numberOfPoints;
			float t = 0;
			for (int i = 0; i < numberOfPoints; i++)
			{
				Vector3 newPos = ReturnCatmullRomPos(t, p0, p1, p2, p3);
				totalPointList.Add(newPos);
				t += segmentation;
			}
		}
	}
	void RenderCatmullromSpline()
	{
		for (int i = 0; i < totalPointList.Count; i++)
		{
			lineRenderer.SetPosition(i, totalPointList[i]);
		}
	}

	public void AddControlPoint(Vector3 point)
	{
		GameObject clone;
		clone = Instantiate(controlPoint, point, controlPoint.transform.rotation) as GameObject;
		clone.transform.parent = gameObject.transform;
		controlPointList.Add(clone.transform);
	}
	public void InsertControlPoint(Vector3 point)
	{
		GameObject clone;
		clone = Instantiate(controlPoint, point, controlPoint.transform.rotation) as GameObject;
		clone.transform.parent = gameObject.transform;
		controlPointList.Insert(neartestIndex, clone.transform);
	}
	public void MoveControlPoint(GameObject obj, Vector3 point)
	{
		obj.transform.position = point;
	}
	public void removeControlPoint(GameObject obj)
	{
		controlPointList.Remove(obj.transform);
		Destroy(obj);
	}
	public int nearestControlPointDis(Vector3 point)
	{
		float mindis = float.MaxValue;
		List<int> minList = new List<int>();
		for (int i = 0; i < totalPointList.Count; i++)
		{
			if (Vector3.Distance(point, totalPointList[i]) <= mindis)
			{
				mindis = Vector3.Distance(point, totalPointList[i]);
				minList.Insert(0, i / numberOfPoints);
			}
		}
		neartestIndex = minList[0];
		return neartestIndex;
	}
}
