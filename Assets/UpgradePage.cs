using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	public void OnClickPower()
	{
		
		CloseWin();
	}
	public void OnClickSpeed()
	{
		CloseWin();
	}
	public void OnClickTime()
	{
		CloseWin();
	}

	void CloseWin()
	{
		gameObject.SetActive(false);
	}
}
