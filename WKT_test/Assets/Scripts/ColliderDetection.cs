using UnityEngine;
using System.Collections; 
using UnityEngine.EventSystems;
public class ColliderDetection : MonoBehaviour {
	 
	public GameObject mainCamera;
	public GameObject collisionPlane;
	public CatmullRomController catmullRomController;
	public BeamsController beamsController;
	public EaveController eaveController;
	public RoofController roofController;
	public TailsController tailsController;
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
									tailsController.SetCatmullRom();
									catmullRomController.ResetRingMirrorControlPoint(buttonCnotroller.ringMirrorSliderValue, 0);
									eaveController.SetCatmullRom(); 
									roofController.SetRoofTriangle();
				
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
								if (catmullRomController.controlPointList.Count > 2)
								 {
									catmullRomController.RemoveControlPoint(hit.collider.gameObject);
									catmullRomController.ResetCatmullRom();
								}
								if (buttonCnotroller.isRingMirror)
								{
									tailsController.SetCatmullRom();
									catmullRomController.ResetRingMirrorControlPoint(buttonCnotroller.ringMirrorSliderValue, 0);
									eaveController.SetCatmullRom();
									roofController.SetRoofTriangle();
									
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
						if (buttonCnotroller.isRingMirror)
						{
							if (chooseObj.tag == "tail")
							{
								tailsController.SetCatmullRom();
								catmullRomController.ResetRingMirrorControlPoint(buttonCnotroller.ringMirrorSliderValue, 0);
							}
							else if (chooseObj.tag == "eave")
							{
							}
							else if (chooseObj.tag == "beam")
							{
								catmullRomController.ResetRingMirrorControlPoint(buttonCnotroller.ringMirrorSliderValue, 0);
							}
							if (chooseObj.tag == "roofRidge")
							{
								roofController.SetRoofTriangle();
								tailsController.SetCatmullRom();
								catmullRomController.ResetRingMirrorControlPoint(buttonCnotroller.ringMirrorSliderValue, 0);
								eaveController.SetCatmullRom();
							}
						}
						if (hit.collider.gameObject == collisionPlane.gameObject)
						{
							if (chooseObj.tag == "roofRidge")
							{
								catmullRomController.MoveControlPoint(chooseObj, hit.point);
								catmullRomController.ResetCatmullRom();
							}
							else if (chooseObj.tag == "beam")
							{
								beamsController.MoveAllBeamsControlPoint(hit.point - chooseObj.transform.position);
							}
							else if (chooseObj.tag == "eave") 
							{
								eaveController.MoveEaveControlPoint( hit.point - chooseObj.transform.position);
							}
							else if (chooseObj.tag == "tail")
							{
								tailsController.MoveTailsControlPoint(chooseObj, hit.point);
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
							if (hit.collider.gameObject.tag == "roofRidge" || hit.collider.gameObject.tag == "beam" || hit.collider.gameObject.tag == "eave" || hit.collider.gameObject.tag == "tail")
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
