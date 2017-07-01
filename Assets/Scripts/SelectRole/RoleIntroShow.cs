using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoleIntroShow : MonoBehaviour {
	
	public Sprite[] SImage;
	Image im;
	public int player;
	public int playerNum;
	List<int> alreadyIn = new List<int>();
	// Use this for initialization
	void Start () {
		GameObject intro = GameObject.Find("introduction");
		im = intro.GetComponent<Image>();
		playerNum = 0;
		player = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void showRoleIntro(int rolenum) {
		im.sprite = SImage[rolenum];
		player++;
	}
	public void cachImage(int roleNum) {
		if (!alreadyIn.Contains(roleNum)) {
			alreadyIn.Add (roleNum);
			Debug.Log (PlayerPrefs.GetInt("roleNum"+playerNum.ToString()));
			PlayerPrefs.SetInt("roleNum"+playerNum.ToString(),roleNum);;
			playerNum++;
		}
	}
}
