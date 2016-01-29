using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {
	public bool isAdd = false;
	public bool isDelete = false;
	public bool isMove = false;
	public bool isRingMirror = false;
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
		isRingMirror = true;
	}
	public void ResetAllState() 
	{ 
		isAdd = false;
		isDelete = false;
		isMove = false;
		isRingMirror = false;
	}
}
