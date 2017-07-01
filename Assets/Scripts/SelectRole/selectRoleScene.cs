using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//选择角色场景

enum SelectStates {
	selecting,
	selected
}
public class selectRoleScene : MonoBehaviour {

	Button Rbtn;
	public RoleIntroShow RIS;
	public int roleNum;
	public Sprite replace;
	public Sprite raw;
	SelectStates p;

	// Use this for initialization  
	void Start () {
		p = SelectStates.selecting;
		//角色按钮
		string name = "role" + roleNum.ToString();
		GameObject role = GameObject.Find(name);
		Rbtn = role.GetComponent<Button>();
		raw = Rbtn.gameObject.GetComponent<Image> ().sprite;
		//选择按钮
		GameObject Select = GameObject.Find("Select");
		Button Sbtn = Select.GetComponent<Button>(); 

		Sbtn.onClick.AddListener(delegate ()  
			{  
				RIS.player = 0;
				//当被选择是才playnum++
				if (p == SelectStates.selected) {
					Rbtn.enabled = false;
					Debug.Log(RIS.playerNum);
					RIS.cachImage(roleNum);
				}
				if (RIS.playerNum == 2) {
					this.GoNextScene(Select);  
				}
			});
		Rbtn.onClick.AddListener(delegate ()  
			{
				Debug.Log(123);
				if (p == SelectStates.selecting && RIS.player == 0) {
					Rbtn.gameObject.GetComponent<Image>().sprite = replace;
					p = SelectStates.selected;
					RIS.showRoleIntro(roleNum);  //显示简介	

				} else if (p == SelectStates.selected && RIS.player == 1) {
					Rbtn.gameObject.GetComponent<Image>().sprite = raw;
					p = SelectStates.selecting;
					RIS.showRoleIntro(roleNum);  //显示简介	
					RIS.player = 0;
				}
			});
	}

	// Update is called once per frame  
	void Update()  
	{  

	} 

	void OnGUI() {


	}

	public void GoNextScene(GameObject NScene)  
	{
		Application.LoadLevel("UI");//切换到UI场景 
	}  
		

}