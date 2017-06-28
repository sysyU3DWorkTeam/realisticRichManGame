using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uicontroller : MonoBehaviour {

	public GUIText[] Money_Label;
	public GUIText[] Score_Label;

	public int[] score;
	public int[] money;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Display(0);
		Display(1);
	}

	//显示玩家的金钱、分数
	void Display(int index) { 
		Money_Label[index].text = string.Format ("{0:N0}", money[index]); 
		Score_Label[index].text = string.Format ("{0:N0}", score[index]);
	}

    public void ShowPlayerMessage(PlayerBehaviour[] players)
    {
        for (int i = 0; i < score.Length && i < players.Length; i++)
        {
            score[i] = players[i].score;
        }
        for (int i = 0; i < money.Length && i < players.Length; i++)
        {
            money[i] = players[i].money;
        }
    }


 //   public void SetPoint(int s) {
	//	score[0] = s;
	//}

	//public int GetPoint() {
	//	return score[0];
	//}

	//public void SetMoney(int m) {
	//	money[0] = m;
	//}

	//public int GetMoney() {
	//	return money[0];
	//}

	//public void SetPoint1(int s) {
	//	score[1] = s;
	//}

	//public int GetPoint1() {
	//	return score[1];
	//}

	//public void SetMoney1(int m) {
	//	money[1] = m;
	//}

	//public int GetMoney1() {
	//	return money[1];
	//}
}