using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonController : MonoBehaviour {
	public CatmullRomController catmullromController;
	public BeamsController beamsController;
	public EaveController eaveController;
	public RoofController roofController;
	public TailsController tailsController;
	public bool isAdd = false;
	public bool isRingMirror = false;

	public Slider ringMirrorSlider;

	public int ringMirrorSliderValue;

	public GameObject chooseObj = null;
	void Start() 
	{ 
		 ringMirrorSliderValue=(int)ringMirrorSlider.value;
	}
	void Update() 
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (isAdd)
			{
				Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
				if (Input.GetMouseButtonDown(0))
				{
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit))
					{
						if (hit.collider.gameObject.name == "CollisionPlane")
						{
							catmullromController.AddControlPoint(mousePos);
							catmullromController.ResetCatmullRom();
							if (isRingMirror)
							{
								tailsController.SetCatmullRom();
								catmullromController.ResetRingMirrorControlPoint(ringMirrorSliderValue, 0);
								roofController.SetRoofMesh();

							}
						}
					}
				}
			}
			/************************************move**********************************/
				if (chooseObj)
				{
					if (Input.GetMouseButtonUp(0))
					{
						chooseObj = null;
						return;
					}
					if (chooseObj.tag == "roofRidge")
					{
						//catmullromController.MoveControlPoint(chooseObj, objPos);
						catmullromController.ResetCatmullRom();
					}
					else if (chooseObj.tag == "beam")
					{
						Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
/*
						Debug.Log("objTransform.position:" + objPos);
						Debug.Log("chooseObj.transform.position:" + chooseObj.transform.position);*/
						beamsController.MoveAllBeamsControlPoint(mousePos - chooseObj.transform.position);
					}
					else if (chooseObj.tag == "tail")
					{
						//tailsController.MoveTailsControlPoint(chooseObj, objPos);
					}
					if (isRingMirror)
					{
						if (chooseObj.tag == "tail")
						{
							tailsController.SetCatmullRom();
							catmullromController.ResetRingMirrorControlPoint(ringMirrorSliderValue, 0);
						}
						else if (chooseObj.tag == "beam")
						{
							catmullromController.ResetRingMirrorControlPoint(ringMirrorSliderValue, 0);
						}
						else if (chooseObj.tag == "roofRidge")
						{
							roofController.SetRoofMesh();
							tailsController.SetCatmullRom();
							catmullromController.ResetRingMirrorControlPoint(ringMirrorSliderValue, 0);
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
							if (hit.collider.gameObject.tag == "gizmo")
							{
								isAdd = false;
								GameObject obj = GameObject.Find("Gizmo").GetComponent<Gizmo>().SelectedObjects[0].gameObject;
								if (obj.tag == "roofRidge" || obj.tag == "beam" || obj.tag == "eave" || obj.tag == "tail")
								{
									chooseObj = obj;
								}

							}
						}
						
					}
				}
			}
	}
	public void SetButtonAdd() 
	{
		isAdd=!isAdd;
	}
	public void SetButtonDelete()
	{
		isAdd = false;
			GameObject obj = GameObject.Find("Gizmo").GetComponent<Gizmo>().SelectedObjects[0].gameObject;
			if (obj)
			{
				if (catmullromController.controlPointList.Count > 2)
				{
					catmullromController.RemoveControlPoint(obj);
					GameObject.Find("Gizmo").GetComponent<Gizmo>().SelectedObjects.Clear();
					GameObject.Find("Gizmo").GetComponent<Gizmo>().Hide();
					catmullromController.ResetCatmullRom();
				}
				if (isRingMirror)
				{
					tailsController.SetCatmullRom();
					catmullromController.ResetRingMirrorControlPoint(ringMirrorSliderValue, 0);
					roofController.SetRoofMesh();

				}
			}
	}
	public void SetButtonRingMirror()
	{
		if (catmullromController.controlPointList.Count<2) return;
		isRingMirror =true;
		catmullromController.SetRingMirror(ringMirrorSliderValue, 0);
		roofController.SetRoofMesh();
		tailsController.ResetCatmullRom();
	}
	public void SetRingMirrorSliderValue()
	{
		ringMirrorSliderValue = (int)(ringMirrorSlider.value);
		if (isRingMirror) {
			catmullromController.ResetRingMirrorControlPoint(ringMirrorSliderValue, 0);
			roofController.SetRoofMesh();
			tailsController.ResetCatmullRom();
		}
	}
	public void SetBeams()
	{
		beamsController.NewBeamsSpline();
	}
	public void SetTails()
	{
		tailsController.NewTailSpline();
	}
	public void ResetAllState() 
	{ 
		isAdd = false;
		isRingMirror = false;
		ringMirrorSliderValue = 0;
	}

}
