using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CatmullRomController : MonoBehaviour
{

	public List<Transform> controlPointList = new List<Transform>();
	public GameObject controlPoint_clone;
	public float lineWidth = 1.0f;
	public int numberOfPoints = 50;
	public int neartestIndex = 0;
	
	private LineRenderer lineRenderer;
	public List<Vector3> innerPointList = new List<Vector3>();
	private List<GameObject> ringMirrorSplineList = new List<GameObject>();
	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		if (!lineRenderer)
		{
			gameObject.AddComponent<LineRenderer>();
			lineRenderer =GetComponent<LineRenderer>();
		}
		lineRenderer.useWorldSpace = true;
		lineRenderer.SetColors(Color.white, Color.white);
		lineRenderer.SetWidth(lineWidth, lineWidth);

		ResetCatmullRom();
	}
	public void ResetCatmullRom() 
	{
		ControlPointSetting();
		DisplayCatmullromSpline();
		RenderCatmullromSpline();
	}
	void ControlPointSetting()
	{
		if (controlPointList.Count < 4)
		{
			if (controlPointList.Count < 2)
			{
				lineRenderer.SetVertexCount(0);
				return;
			}
			else if (controlPointList.Count == 2)
			{
				controlPointList.Add(controlPointList[1]);
				controlPointList.Insert(0, controlPointList[0]);
				lineRenderer.SetVertexCount(numberOfPoints * (controlPointList.Count - 3));
			}
		}
		else
		{
			 lineRenderer.SetVertexCount(numberOfPoints * (controlPointList.Count - 3));
		}
	}
	Vector3 ReturnCatmullRomPos(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		Vector3 pos = 0.5f * ((2f * p1) + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t + (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t);
		return pos;
	}
	void DisplayCatmullromSpline()
	{
		innerPointList.Clear();
		for (int index = 0; index < controlPointList.Count; index++)
		{

			if ((index == 0 || index == controlPointList.Count - 2 || index == controlPointList.Count - 1))
			{
				continue;
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

	public void AddControlPoint(Vector3 point)
	{
		GameObject clone;
		clone = Instantiate(controlPoint_clone, point, controlPoint_clone.transform.rotation) as GameObject;
		clone.transform.parent = gameObject.transform;
		if(controlPointList.Count<4)
			controlPointList.Add(clone.transform);
		else{
			controlPointList.Remove(controlPointList[controlPointList.Count-1]);
			controlPointList.Add(clone.transform);
			controlPointList.Add(clone.transform);
		}
	}
	public void MoveControlPoint(GameObject obj, Vector3 point)
	{
		obj.transform.position = point;
	}
	public void RemoveControlPoint(GameObject obj)
	{
		if (controlPointList.Count >4) {
			for (int index = 0; index < controlPointList.Count; index++)
			{
				if (controlPointList[index] == obj.transform) 
				{ 
					if (index == 0)
					{
						controlPointList.Remove(controlPointList[0]);
						controlPointList.Remove(controlPointList[0]);
						controlPointList.Insert(0, controlPointList[0]);
					}
					else if (index == controlPointList.Count - 2)
					{
						controlPointList.Remove(controlPointList[controlPointList.Count-1]);
						controlPointList.Remove(controlPointList[controlPointList.Count-1]);
						controlPointList.Add(controlPointList[controlPointList.Count-1]);
					}
					else
					{
						controlPointList.Remove(obj.transform);
					}
					Destroy(obj);
					break;
				}
			}
		}
		else 
		{
			if (controlPointList.Count== 4)
			{
				for (int index = 0; index < controlPointList.Count; index++)
				{
					if (controlPointList[index] == obj.transform) 
					{
						if (index == 0)
						{
							controlPointList.Remove(controlPointList[0]);
							controlPointList.Remove(controlPointList[0]);
							controlPointList.Remove(controlPointList[controlPointList.Count-1]);
						}
						else if (index == controlPointList.Count - 2)
						{
							controlPointList.Remove(controlPointList[controlPointList.Count - 1]);
							controlPointList.Remove(controlPointList[controlPointList.Count - 1]);
							controlPointList.Remove(controlPointList[0]);
						}
						Destroy(obj);
						break;
					}
				}
			}
			else 
			{
				controlPointList.Remove(obj.transform);
				Destroy(obj);
			}

		}
	}
	public int NearestControlPointDis(Vector3 point)
	{
		float mindis = float.MaxValue;
		List<int> minList = new List<int>();
		for (int i = 0; i < innerPointList.Count; i++)
		{
			if (Vector3.Distance(point, innerPointList[i]) <= mindis)
			{
				mindis = Vector3.Distance(point, innerPointList[i]);
				minList.Insert(0, i / numberOfPoints);
			}
		}
		neartestIndex = minList[0];
		return neartestIndex;
	}
	public void SetRingMirror()
	{
/*
		int number=10;
		float radius=0;
		float radiusSelf = controlPointList[controlPointList.Count - 1].position.x - controlPointList[0].position.x;
		Vector3 centerPos = controlPointList[0].position -new Vector3(radius,0,0);
		Vector3 offset = transform.position - centerPos - new Vector3(radiusSelf, 0, 0);
		for (int i = 1; i <number; i++) 
		{
			float radian= (float)i * 2f * Mathf.PI / (float)number;
			float x = Mathf.Cos(radian) * radiusSelf;
			float z = Mathf.Sin(radian) * radiusSelf;
 			Vector3 pos = new Vector3(x, 0, z) + centerPos;
 			Quaternion rot = Quaternion.FromToRotation(Vector3.right, pos-centerPos);
			GameObject clone = Instantiate(this.gameObject, pos, rot) as GameObject;
			clone.transform.Translate(offset);
 			clone.GetComponent<CatmullRomController>().ResetCatmullRom();
		}*/
		ringMirrorSplineList.Clear();
		ringMirrorSplineList.Add(this.gameObject);
		int number=10;
		float radius=0;
		Vector3 centerPos = controlPointList[0].position -new Vector3(radius,0,0);
		for (int i = 1; i <number; i++) 
		{
			float angle = (float)i*360 / (float)number;
			GameObject clone = Instantiate(this.gameObject, this.transform.position, this.transform.rotation) as GameObject;
			clone.transform.RotateAround(centerPos, Vector3.up, angle);
 			clone.GetComponent<CatmullRomController>().ResetCatmullRom();
			clone.GetComponent<CatmullRomController>().ShowControlPoint(false);
			ringMirrorSplineList.Add(clone);
		}
	}
	public void ShowControlPoint(bool isShow)
	{
		for (int i = 0; i < controlPointList.Count; i++)
		{
 			controlPointList[i].gameObject.GetComponent<SphereCollider>().enabled = isShow;
 			controlPointList[i].gameObject.GetComponent<MeshRenderer>().enabled = isShow;
		}
	}
	public void ResetRingMirrorControlPoint()
	{
		for(int i=1;i<ringMirrorSplineList.Count;i++){
			Destroy(ringMirrorSplineList[i]);
		}
		SetRingMirror();
	}
}
