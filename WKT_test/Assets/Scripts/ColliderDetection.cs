using UnityEngine;
using System.Collections; 
using UnityEngine.EventSystems;
public class ColliderDetection : MonoBehaviour {
	 
	public GameObject mainCamera;
	public GameObject collisionPlane;
	public CatmullRomController catmullRomController;
	public BeamsController beamsController;
	public EaveController eaveController;
	public ButtonController buttonCnotroller;
	public GameObject chooseObj = null;
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
							if (hit.collider.gameObject == collisionPlane.gameObject)
							{
								catmullRomController.AddControlPoint(hit.point);
								catmullRomController.ResetCatmullRom();
								if (buttonCnotroller.isRingMirror) 
								{
									catmullRomController.ResetRingMirrorControlPoint(buttonCnotroller.ringMirrorSliderValue, 0);
									eaveController.SetCatmullRom(); 
								}
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
							if (hit.collider.gameObject.tag == "roofRidge")
							{
								catmullRomController.RemoveControlPoint(hit.collider.gameObject);
								catmullRomController.ResetCatmullRom();
								if (buttonCnotroller.isRingMirror)
								{
									catmullRomController.ResetRingMirrorControlPoint(buttonCnotroller.ringMirrorSliderValue, 0);
									eaveController.SetCatmullRom(); 
								}
							}
						}
					}
			}
			else if (buttonCnotroller.isMove)
			{
				if (chooseObj)
				{
					if (Input.GetMouseButtonUp(0))
					{
						chooseObj = null;
						return;
					}
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit))
					{
						if (hit.collider.gameObject == collisionPlane.gameObject)
						{
							if (chooseObj.tag == "roofRidge") {
								catmullRomController.MoveControlPoint(chooseObj, hit.point);
								catmullRomController.ResetCatmullRom();
							}
							else if (chooseObj.tag == "beam")
							{
								beamsController.MoveAllBeamsControlPoint(hit.point - chooseObj.transform.position);
							}
							if (buttonCnotroller.isRingMirror) 
							{ 
								catmullRomController.ResetRingMirrorControlPoint(buttonCnotroller.ringMirrorSliderValue, 0);
								eaveController.SetCatmullRom(); 
							}
						}
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
							if (hit.collider.gameObject.tag == "roofRidge" || hit.collider.gameObject.tag == "beam")
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
