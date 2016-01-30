using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BeamsController : MonoBehaviour {
	public List<GameObject> beamsSplineList = new List<GameObject>();
	public GameObject controlPoint_clone;
	private LineRenderer lineRenderer;
	public float lineWidth = 1.0f;
	public int number=3;
	public float length = 100;
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
	public void NewBeamsSpline()
	{

		for (int i = 0; i < number; i++)
		{
			AddBeamsControlPoint(new Vector3(0, 0 - i * length, 0));
		}
		lineRenderer.SetVertexCount(number);
		RederBeams();
	}
	void AddBeamsControlPoint(Vector3 point)
	{
		GameObject clone;
		clone = Instantiate(controlPoint_clone, point,Quaternion.identity) as GameObject;
		clone.transform.parent = this.gameObject.transform;
		beamsSplineList.Add(clone);
	}
	public void RederBeams()
	{
		lineRenderer.SetVertexCount(beamsSplineList.Count);
		for (int i = 0; i < beamsSplineList.Count; i++)
		{
			lineRenderer.SetPosition(i, beamsSplineList[i].transform.position);
		}
	}
	public void MoveAllBeamsControlPoint(Vector3 dis)
	{
		for (int i = 0; i < beamsSplineList.Count; i++)
		{
			beamsSplineList[i].transform.Translate(dis.x, dis.y, dis.z);
		}
		RederBeams();
	}
	public void ShowControlPoint(bool isShow)
	{
		for (int i = 0; i < beamsSplineList.Count; i++)
		{
			beamsSplineList[i].gameObject.GetComponent<SphereCollider>().enabled = isShow;
			beamsSplineList[i].gameObject.GetComponent<MeshRenderer>().enabled = isShow;
		}
	}
}
