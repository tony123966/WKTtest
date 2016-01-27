using UnityEngine;
using System.Collections; 
using UnityEngine.EventSystems;
public class ColliderDetection : MonoBehaviour {
	 
	public GameObject mainCamera;
	public GameObject collisionPlane;
	public CatmullRomManager catmullRomManager;
	public ButtonController buttonCnotroller;
	private GameObject chooseObj = null;

	void Start() 
	{
	}
	void Update()
	{
		if (!EventSystem.current.IsPointerOverGameObject()) { 
			if (buttonCnotroller.isAdd) {
					if (Input.GetMouseButtonDown(0))
					{
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						RaycastHit hit;
						if (Physics.Raycast(ray, out hit))
						{
							Debug.Log(hit.collider.gameObject.name);
							if (hit.collider.gameObject.name == collisionPlane.gameObject.name)
							{
								catmullRomManager.AddControlPoint(hit.point);
							}
						}
					}
			}
			else if (buttonCnotroller.isDelete)
			{
					if (Input.GetMouseButtonDown(0))
					{
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						RaycastHit hit;
						if (Physics.Raycast(ray, out hit))
						{
							if (hit.collider.gameObject.tag == "controlPoint")
							{
								catmullRomManager.removeControlPoint(hit.collider.gameObject);
							}
						}
					}
			}
			else if (buttonCnotroller.isMove)
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
				else 
				{
					if (Input.GetMouseButtonDown(0))
					{
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						RaycastHit hit;
						if (Physics.Raycast(ray, out hit))
						{
							if (hit.collider.gameObject.tag == "controlPoint")
							{
								chooseObj = hit.collider.gameObject;
							}
						}	
					}		
				}
			}
		}
	}
}
