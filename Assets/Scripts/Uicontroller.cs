using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uicontroller : MonoBehaviour {

	public GUIText Money_Label;
	public GUIText Score_Label;

	public int score = 0;
	public int money = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Money_Label.text = string.Format ("{0:N0}", money); 
		Score_Label.text = string.Format ("{0:N0}", score);
	}

	public void SetPoint(int s) {
		score = s;
	}

	public int GetPoint() {
		return score;
	}

	public void SetMoney(int m) {
		money = m;
	}

	public int GetMoney() {
		return money;
	}
}
