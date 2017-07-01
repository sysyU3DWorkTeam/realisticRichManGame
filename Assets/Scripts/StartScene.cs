using UnityEngine;  
using System.Collections;  
using UnityEngine.UI;

//开始UI
public class StartScene : MonoBehaviour {  
	
	// Use this for initialization  
	void Start () {  

		GameObject startbtnObj = GameObject.Find("enter");//点击开始游戏按钮
		GameObject endbtnObj = GameObject.Find("exit");//点击退出游戏按钮

		Button btn1 = startbtnObj.GetComponent<Button>(); 
		Button btn2 = endbtnObj.GetComponent<Button>();

		btn1.onClick.AddListener(delegate ()  
			{  
				this.GoNextScene(startbtnObj);  
			});  
		btn2.onClick.AddListener(delegate ()  
			{  
				this.ExitScene(endbtnObj);  
			});  
	}

	// Update is called once per frame  
	void Update()  
	{  
	}  

	public void GoNextScene(GameObject NScene)  
	{  
		Application.LoadLevel("selectRole");//切换到UI场景 
	}  

	public void ExitScene(GameObject exitScene)  
	{  
		Application.Quit();//退出游戏
		Debug.Log("exit the game !!!");
	}  
}    