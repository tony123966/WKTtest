using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CatmullRomController : MonoBehaviour
{
	public List<Transform> controlPointList = new List<Transform>();
	public GameObject beams_clone;
	public GameObject controlPoint_clone;
	public float lineWidth = 1.0f;
	public int numberOfPoints = 50;

	
	private LineRenderer lineRenderer;
	public List<Vector3> innerPointList = new List<Vector3>();
	public List<GameObject> ringMirrorSplineList = new List<GameObject>();
	public List<GameObject> ringMirrorBeamsSplineList = new List<GameObject>();

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
		if (controlPointList.Count < 2) 
		{
			lineRenderer.SetVertexCount(0);
			return; 
		}
		else 
		{
			lineRenderer.SetVertexCount(numberOfPoints * (controlPointList.Count - 1));
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
		Vector3 p0, p1, p2, p3;

		if (controlPointList.Count<2)return;
		else if (controlPointList.Count == 2) 
		{
			p0 = controlPointList[0].transform.position;
			p1 = controlPointList[0].transform.position;
			p2 = controlPointList[1].transform.position;
			p3 = controlPointList[1].transform.position;


			float segmentation = 1 / (float)numberOfPoints;
			float t = 0;
			for (int i = 0; i < numberOfPoints; i++)
			{
				Vector3 newPos = ReturnCatmullRomPos(t, p0, p1, p2, p3);
				innerPointList.Add(newPos);
				t += segmentation;
			}
		}
		else {
			for (int index = 0; index < controlPointList.Count-1; index++)
			{
				if (index == 0)
				{
					p0 = controlPointList[0].transform.position;
					p1 = controlPointList[0].transform.position;
					p2 = controlPointList[1].transform.position;
					p3 = controlPointList[2].transform.position;
				}
				else if (index == controlPointList.Count - 2)
				{
					p0 = controlPointList[index -1].transform.position;
					p1 = controlPointList[index].transform.position;
					p2 = controlPointList[index+1].transform.position;
					p3 = controlPointList[index+1].transform.position;
				}
				else
				{
					p0 = controlPointList[index - 1].transform.position;
					p1 = controlPointList[index].transform.position;
					p2 = controlPointList[index + 1].transform.position;
					p3 = controlPointList[index + 2].transform.position;
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
		controlPointList.Add(clone.transform);
	}
	public void MoveControlPoint(GameObject obj, Vector3 point)
	{
		obj.transform.position = point;
	}
	public void RemoveControlPoint(GameObject obj)
	{

		for (int index = 0; index < controlPointList.Count; index++)
		{
			if (controlPointList[index] == obj.transform)
			{
				controlPointList.Remove(controlPointList[index]);
				Destroy(obj);
				break;
			}

		}
	}
	public void SetRingMirror(int number,float radius)
	{
		/*
				if (controlPointList.Count == 0) return;
				ringMirrorSplineList.Clear();
				ringMirrorBeamsSplineList.Clear();
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
					clone.GetComponent<CatmullRomController>().ShowControlPoint(false);
					clone.GetComponent<ColliderDetection>().enabled = false;
					ringMirrorSplineList.Add(clone);
				}*/
		if (controlPointList.Count == 0) return;
		ringMirrorSplineList.Clear();
		ringMirrorBeamsSplineList.Clear();
		Vector3 centerPos = controlPointList[0].position -new Vector3(radius,0,0);
		for (int i = 1; i <number; i++) 
		{
			float angle = (float)i*360 / (float)number;
			GameObject clone = Instantiate(this.gameObject, this.transform.position, this.transform.rotation) as GameObject;
			clone.transform.RotateAround(centerPos, Vector3.up, angle);
			if (i % 2 != 0)ScaleCatmullromSpline(clone,0.2f);
			clone.GetComponent<CatmullRomController>().ResetCatmullRom();
			clone.GetComponent<CatmullRomController>().ShowControlPoint(false);
			clone.GetComponent<ColliderDetection>().enabled = false;
			ringMirrorSplineList.Add(clone);

			if (i % 2 == 0) { 
				clone = Instantiate(beams_clone, beams_clone.transform.position, beams_clone.transform.rotation) as GameObject;
				clone.transform.RotateAround(centerPos, Vector3.up, angle);
				clone.GetComponent<BeamsController>().RederBeams();
				clone.GetComponent<BeamsController>().ShowControlPoint(false);
				ringMirrorBeamsSplineList.Add(clone);
			}


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
	public void ResetRingMirrorControlPoint(int number, float radius )
	{
		for(int i=0;i<ringMirrorSplineList.Count;i++){
			Destroy(ringMirrorSplineList[i]);
		}
		for (int i = 0; i < ringMirrorBeamsSplineList.Count; i++)
		{
			Destroy(ringMirrorBeamsSplineList[i]);
		}
		SetRingMirror(number, radius);
	}
	void ScaleCatmullromSpline(GameObject obj,float scaleValue)
	{
		List<Transform> controlPointList_obj=obj.GetComponent<CatmullRomController>().controlPointList;
		Vector3 vecDiff = controlPointList_obj[controlPointList_obj.Count - 1].transform.position - controlPointList_obj[0].transform.position;
		controlPointList_obj[controlPointList_obj.Count - 1].transform.position -= (vecDiff + new Vector3(0, -vecDiff.y*1.5f, 0)) * scaleValue;
	
	}

}
