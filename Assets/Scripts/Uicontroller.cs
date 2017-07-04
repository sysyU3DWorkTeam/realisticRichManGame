using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
	Play,
	End,
}

public class Uicontroller : MonoBehaviour {

	public GameState gamestate;

	public GUIText[] Money_Label;
	public GUIText[] Score_Label;

	public int[] score;
	public int[] money;

	public Texture[] roleImage;
	public GUITexture[] Role_Icon;


	public Texture Exit_btn;
	public Texture Replay_btn;
	public GUITexture Win_Gui_Icon;
	public GUITexture Lose_Gui_Icon;
	public GUITexture Change_screen;
	public GameObject result_window;

	GUIStyle guiRectStyle;

	public Fade fade;
	public Sound_Play SPOver;

	float time = 0.0f;
	int musictime = 0;
	float screenX;
	float screenY;


	// Use this for initialization
	void Start () {
		screenX = Screen.width * 0.5f;
		screenY = Screen.height * 0.5f;
		guiRectStyle = new GUIStyle();
		guiRectStyle.border = new RectOffset(0,0,0,0);

		gamestate = GameState.Play;
		Time.timeScale = 1;
		Change_screen.color = new Color(0,0,0,0);
		fade.FadeIn();
	}
	
	// Update is called once per frame
	void Update () {
		Display(0);
		Display(1);
		if(score[0] < 0 || money[0] < 0) {
			time += Time.deltaTime;
			musictime++;
			ShowResult(1,0);
		}
		if(score[1] < 0|| money[1] < 0) {
			time += Time.deltaTime;
			musictime++;
			ShowResult(0,1);
		}
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
			score[i] = players[i].PD.score;
        }
        for (int i = 0; i < money.Length && i < players.Length; i++)
        {
            money[i] = players[i].PD.money;
        }


		//显示角色形象
		//for (int i = 0; i < players.Length;i++) {
		for (int i = 0; i < 2;i++) {
			int roleNum = PlayerPrefs.GetInt("roleNum"+i.ToString());
			Role_Icon[i].texture = roleImage[roleNum];
		}
		//PlayerPrefs.DeleteAll();


    }

	void ShowResult(int win, int lose) {
		result_window.gameObject.SetActive(true);
		Change_screen.color = new Color(0,0,0,0.35f);
		fade.FadeOut(); 

		Win_Gui_Icon.texture = Role_Icon[win].texture;
		Lose_Gui_Icon.texture = Role_Icon[lose].texture;
		gamestate = GameState.End;

		if (musictime == 1)
		    SPOver.SoundPlay(9);
		if (time >= 1.3f)
		    Time.timeScale = 0;

	}

	void OnGUI () {
		
		if(gamestate == GameState.End) {
			//replay the game
			if(GUI.Button(new Rect (screenX * 0.83f - Replay_btn.width * 0.5f,
				screenY * 1.5f - Replay_btn.height * 0.5f,
				Replay_btn.width,Replay_btn.height),
				Replay_btn,
				guiRectStyle))
			{
					Time.timeScale = 1;
					SceneManager.LoadScene("selectRole");
			}

			//exit the game
			if(GUI.Button(new Rect (screenX * 0.83f + Exit_btn.width * 0.5f + 40f,
				screenY * 1.5f - Exit_btn.height * 0.5f,
				Exit_btn.width,Exit_btn.height),
				Exit_btn,
				guiRectStyle))
			{
				Application.Quit();
			}
		}
	}
		
}
