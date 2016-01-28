using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {
	public bool isAdd = false;
	public bool isDelete = false;
	public bool isMove = false;
	public bool isMirror = false;
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
	public void SetButtonMirror()
	{
		isMirror=true;
	}
	public void resetAllState() 
	{ 
		isAdd = false;
		isDelete = false;
		isMove = false;
		isMirror = false;
	}
}
