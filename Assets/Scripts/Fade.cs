using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour
{
	GUITexture ChangeScreen; //改变场景的效果

	public float Fade_Time = 1.5f; //场景变化持续时间

	public bool isFadeOut = false;
	public bool isFadeIn = true;

	float time = 0.0f;

	void Start ()
	{
		//获取组件GUITexture作为场景遮罩
		ChangeScreen = GetComponent<GUITexture> ();
		ChangeScreen.color = new Color(0,0,0,0);
	}

	void Update ()
	{
		float timeToChange;
		Color c1;
		Color c2 = new Color (0, 0, 0, 0);

		//场景加入阴影的效果
		if (isFadeOut == true)
		{
			time += Time.deltaTime;
			timeToChange = time / Fade_Time;
			c1 = new Color (0, 0, 0, 0.4f);
			Debug.Log("fadeout");
			ChangeScreen.color = Color.Lerp (c2,c1,timeToChange);

		}

		//场景褪去阴影的效果
		if (isFadeIn == true)
		{
			time += Time.deltaTime;
			timeToChange = time / Fade_Time;
			c1 = new Color (0, 0, 0, 0.4f);
			//Debug.Log("fadein");
			ChangeScreen.color = Color.Lerp (c1,c2,timeToChange);
		}

		//不展示遮罩效果
		if (time >= Fade_Time)
		{
			time = 0;
			isFadeOut = false;
			isFadeIn = false;
		}
	}
		
	//场景加入阴影的效果
	public void FadeOut ()
	{
		isFadeOut = true;
		isFadeIn = false;
	}

	//场景褪去阴影的效果
	public void FadeIn ()
	{
		isFadeIn = true;
		isFadeOut = false;
	}

}
