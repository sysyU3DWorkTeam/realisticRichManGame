using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bgm_Play : MonoBehaviour {

	public Texture bgm_btn;
	public Texture bgm_btn1;
	public AudioClip music;
	public AudioSource musicsource;

	public float musicVolume;

	GUIStyle guiRectStyle;

	float screenX;
	float screenY;

	// Use this for initialization
	void Start () {

		musicVolume = 0.5f;
		musicsource.clip = music;
		musicsource.Play();

		screenX = Screen.width * 0.06f;
		screenY = Screen.height * 0.06f;

		guiRectStyle = new GUIStyle ();
		guiRectStyle.border = new RectOffset (0, 0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI () {
		if (GUI.Button (new Rect (screenX - bgm_btn.width * 0.5f,
			bgm_btn.height * 0.1f + 10f, 
			bgm_btn.width* 0.45f, bgm_btn.height* 0.45f),
			bgm_btn,
			guiRectStyle))
		{
				
				musicsource.Play();
		}

		if (GUI.Button (new Rect (screenX - bgm_btn1.width * 0.15f,
			bgm_btn1.height * 0.1f + 20f, 
			bgm_btn1.width* 0.85f, bgm_btn1.height* 0.85f	),
			bgm_btn1,
			guiRectStyle))
		{
				musicsource.Stop();
		}
	}

}
