using UnityEngine;
using System.Collections; 
using UnityEngine.EventSystems;
public class ColliderDetection : MonoBehaviour {
	 
	public GameObject mainCamera;
	public GameObject collisionPlane;
	public CatmullRomController catmullRomController;
	public ButtonController buttonCnotroller;
	private static GameObject chooseObj = null;

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
							if (hit.collider.gameObject.name == collisionPlane.gameObject.name)
							{
								catmullRomController.AddControlPoint(hit.point);
								catmullRomController.ResetCatmullRom();
								if (buttonCnotroller.isRingMirror) catmullRomController.ResetRingMirrorControlPoint();
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
								catmullRomController.RemoveControlPoint(hit.collider.gameObject);
								catmullRomController.ResetCatmullRom();
								if (buttonCnotroller.isRingMirror) catmullRomController.ResetRingMirrorControlPoint();
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
							catmullRomController.MoveControlPoint(chooseObj, hit.point);
							if (buttonCnotroller.isRingMirror) catmullRomController.ResetRingMirrorControlPoint();
						}
					}
					if (Input.GetMouseButtonUp(0))
					{
						chooseObj=null;
					}
					catmullRomController.ResetCatmullRom();
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
