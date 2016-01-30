using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {
	public CatmullRomController catmullromController;
	public BeamsController beamsController;
	public EaveController eaveController;
	public bool isAdd = false;
	public bool isDelete = false;
	public bool isMove = false;
	public bool isRingMirror = false;

	public Slider ringMirrorSlider;

	public int ringMirrorSliderValue;
	void Start() 
	{ 
		 ringMirrorSliderValue=(int)ringMirrorSlider.value;
	}
	public void SetButtonAdd() 
	{
		isAdd=!isAdd;
		isDelete = false;
		isMove = false;
	}
	public void SetButtonDelete()
	{
		isDelete=!isDelete;
		isAdd = false;
		isMove = false;
	}
	public void SetButtonMove()
	{
		isMove=!isMove;
		isAdd = false;
		isDelete = false;
	}
	public void SetButtonRingMirror()
	{
		isRingMirror =true;
		catmullromController.SetRingMirror(ringMirrorSliderValue, 0);
		eaveController.SetCatmullRom();
	}
	public void SetRingMirrorSliderValue()
	{
		ringMirrorSliderValue = (int)(ringMirrorSlider.value);
		if (isRingMirror) {
			catmullromController.ResetRingMirrorControlPoint(ringMirrorSliderValue, 0);
			eaveController.SetCatmullRom();
		}
	}
	public void SetBeams()
	{
		beamsController.NewBeamsSpline();
	}
	public void ResetAllState() 
	{ 
		isAdd = false;
		isDelete = false;
		isMove = false;
		isRingMirror = false;
		ringMirrorSliderValue = 0;
	}
}
