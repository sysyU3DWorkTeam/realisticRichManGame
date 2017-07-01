using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uicontroller : MonoBehaviour {

	public GUIText[] Money_Label;
	public GUIText[] Score_Label;

	public int[] score;
	public int[] money;

	public Texture[] roleImage;
	public GUITexture[] Role_Icon;

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

		////////////
		//Todo,显示角色形象，目前只有单人选角色
		//for (int i = 0; i < players.Length;i++) {
		for (int i = 0; i < 2;i++) {
			int roleNum = PlayerPrefs.GetInt("roleNum"+i.ToString());
			Role_Icon[i].texture = roleImage[roleNum];
		}
		//PlayerPrefs.DeleteAll();


    }
		
}
