using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatmullRomControllerTest : MonoBehaviour {
	public List<Transform> controlPointList = new List<Transform>();
	public bool isLoop=false;
	void OnDrawGizmos()
	{

		Gizmos.color = Color.white;
		for (int i = 0; i < controlPointList.Count; i++)
		{
			Gizmos.DrawWireSphere(controlPointList[i].position, 0.3f);
		}
		for (int i = 0; i < controlPointList.Count; i++)
		{
			if ((i == 0 || i == controlPointList.Count - 2 || i == controlPointList.Count - 1) && !isLoop)
			{
				continue;
			}
			DisplayCatmullromSpline(i);
		}
	}
	Vector3 ReturnCatmullRomPos(float t,Vector3 p0,Vector3 p1,Vector3 p2,Vector3 p3) 
	{ 
		Vector3 pos=0.5f*((2f*p1)+(-p0+p2)*t+(2f*p0-5f*p1+4f*p2-p3)*t*t+(-p0+3f*p1-3f*p2+p3)*t*t*t);
		return pos;
	}
	void DisplayCatmullromSpline(int index) 
	{
		Vector3 lastPos = Vector3.zero;

		Vector3 p0 = controlPointList[(index - 1 + controlPointList.Count) % controlPointList.Count].transform.position;
		Vector3 p1 = controlPointList[(index + controlPointList.Count) % controlPointList.Count].transform.position;
		Vector3 p2 = controlPointList[(index + 1 + controlPointList.Count) % controlPointList.Count].transform.position;
		Vector3 p3 = controlPointList[(index + 2 + controlPointList.Count) % controlPointList.Count].transform.position;
		float segmentation=0.1f;
		for (float t= 0; t <1; t += segmentation)
		{
			Vector3 newPos = ReturnCatmullRomPos(t, p0, p1, p2, p3);
			if (t == 0) { 
				lastPos = newPos;
				continue;
			}
			Gizmos.DrawLine(lastPos, newPos);
			lastPos = newPos;
		}
		Gizmos.DrawLine(lastPos, controlPointList[(index +1+ controlPointList.Count) % controlPointList.Count].transform.position);
	}
}
