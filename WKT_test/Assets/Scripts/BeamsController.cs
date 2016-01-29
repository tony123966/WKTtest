using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BeamsController : MonoBehaviour {
	public List<GameObject> BeamsSplineList = new List<GameObject>();
	public GameObject controlPoint_clone;
	private LineRenderer lineRenderer;
	public float lineWidth = 1.0f;
	public void NewBeamsSpline()
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
		for(int i=0;i<5;i++)
		{
			AddBeamsControlPoint(new Vector3(0,0-i*10,0));
		}
		lineRenderer.SetVertexCount( BeamsSplineList.Count);
		RederBeams();
	}
	void AddBeamsControlPoint(Vector3 point)
	{
		GameObject clone;
		clone = Instantiate(controlPoint_clone, point, controlPoint_clone.transform.rotation) as GameObject;
		clone.transform.parent = this.gameObject.transform;
		BeamsSplineList.Add(clone);
	}
	public void RederBeams()
	{
		for (int i = 0; i < BeamsSplineList.Count; i++)
		{
			lineRenderer.SetPosition(i, BeamsSplineList[i].transform.position);
		}
	}
	public void MoveAllBeamsControlPoint(Vector3 dis)
	{
		for(int i=0;i<BeamsSplineList.Count;i++)
		{
			BeamsSplineList[i].transform.Translate(dis.x, dis.y, dis.z);
		}
		RederBeams();
	}
}
