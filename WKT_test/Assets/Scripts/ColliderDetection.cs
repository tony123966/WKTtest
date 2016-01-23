using UnityEngine;
using System.Collections;

public class ColliderDetection : MonoBehaviour {
	public GameObject mainCamera;
	public GameObject collisionPlane;
	public CatmullRomManager catmullRomManager;
	
	public GameObject chooseObj=null;
	public Vector3 clickPos = Vector3.zero;
	void Update() 
	{
		if (chooseObj) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.gameObject.name == collisionPlane.gameObject.name)
				{
					catmullRomManager.MoveControlPoint(chooseObj, hit.point);
				}
			}
			if (Input.GetMouseButtonUp(0)) 
			{ 
				chooseObj=null;
			}
		}
		else {
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.gameObject.name == collisionPlane.gameObject.name)
					{
						catmullRomManager.AddControlPoint(hit.point);
					}
					else if (hit.collider.gameObject.tag == "controlPoint")
					{
						chooseObj = hit.collider.gameObject;
						clickPos = hit.point;
					}
				}
			}
		}
			if (Input.GetMouseButtonDown(1))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.gameObject.tag == "controlPoint")
					{
						catmullRomManager.removeControlPoint(hit.collider.gameObject);
						chooseObj=null;
						clickPos = hit.point
					}
				}
			}
	}
}
