using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum playerStates {
	move,
	skill
}  

public class PlayerBehaviour : MonoBehaviour {

    public delegate void OnActionOver(PlayerBehaviour sender);
    public event OnActionOver MoveOver;
    public event OnActionOver SkillOver;

    public GameObject player;  // 玩家模型

	//public GameController gameController;

	public Sound_Play SP1; //移动音效

	public int skipsTimes = 0;
    public int mapPosition = 0;

    private List<Vector3> myPositionList = new List<Vector3>(); //存放控制器给的位置参数 
    private List<CardData> myCardList = new List<CardData>(); //绑定卡牌类， 每个人绑定6个卡牌，用一个卡牌list来保存

	public int score = 0;
	public int money = 0;

	public Texture end_btn;
	private playerStates states;

	GUIStyle guiRectStyle;
	GUIStyle guiLabelStyle;

	float screenX;
	float screenY;
    float speed = 5;  //走路速度

    // Use this for initialization
    void Start () {
        player = this.gameObject;
		//skipsTimes = 0;
		//score = 0;
		//money = 0;
		states = playerStates.move;
        loadsrc();
		ScreenSetting ();
    }
	
	// 每一帧获取新的参数，移动player
	void FixedUpdate () {
        PlayerMove();
    }

    //初始化模型
    void loadsrc()
    {

    }

    void PlayerMove()
    {
        if (myPositionList.Count > 0)  //给参数才会走，防止出现溢出
        {
			
            float step = speed * Time.deltaTime;  // 速度
            player.transform.position = Vector3.MoveTowards(player.transform.position, myPositionList[0], step);  // 移动
            if (player.transform.position == myPositionList[0])
            {
                myPositionList.RemoveAt(0);
				SP1.SoundPlay(10);
				//SP1.SoundPlay(2);
			}  
			if (myPositionList.Count == 0)
			{
                MoveOver(this);
				//if (gameController != null)
				//	gameController.OnMoveOver();
				states = playerStates.skill;
			}
        }
    }

	/// /设置图形用户界面样式
	void ScreenSetting ()
	{
		float screenXOff = Screen.width;  //左右偏移
		float screenYOff = Screen.height; //上下偏移

		screenX = screenXOff * 0.5f;  
		screenY = screenXOff * 0.5f;  

		guiRectStyle = new GUIStyle ();
		guiRectStyle.border = new RectOffset (0, 0, 0, 0);
		guiLabelStyle = new GUIStyle ();
		guiLabelStyle.fontSize = 20;
		guiLabelStyle.normal.textColor = new Color(46f/256f, 163f/256f, 256f/256f, 256f/256f); 

	}

    //从控制器获取新的位置参数，赋值给TargetPosition，玩家就会走到TargetPosition
    public void ObatinTargetPositon(Vector3 Position)
    {
        myPositionList.Add(Position);
    }

	//使用卡牌，同时从自己的list里面删除卡牌
	public void UseCardMove(CardData card, CardData nCard)
	{
		//缺少使用card之后的动作
		//---------------------------------------这里不应该是替换，在当前回合内，这个地方卡牌应该为空
		myCardList[myCardList.IndexOf(card)] = nCard;
		return;
	}

    //使用卡牌，同时从自己的list里面删除卡牌
    public void UseCardSkill(CardData card, CardData nCard)
    {
        myCardList[myCardList.IndexOf(card)] = nCard;
        this.states = playerStates.move;

        SkillOver(this);
        //gameController.OnSkillOver();
        return;
    }

    //获得所有带着的卡牌
    public List<CardData> GetAllCard()
    {
        return myCardList;
    }

    //由Controller调用, 人物将新卡加入List
    public void AddCard(CardData card)
    {
        myCardList.Add(card);
    }

	//玩家回合不同阶段
	public void playing() {
	    
	}

	void OnGUI() {
		if (states == playerStates.skill) {
			GUI.Label (new Rect (screenX - end_btn.width * 0.5f + end_btn.width*0.3f,
				end_btn.height * 0.1f+10, 
				300, end_btn.height*0.3f),
				"是否发动技能?",guiLabelStyle);
			;
			if (GUI.Button (new Rect (screenX - end_btn.width * 0.5f,
				end_btn.height * 0.1f, 
				end_btn.width*0.2f, end_btn.height*0.2f	),
				end_btn,
				guiRectStyle))
			{
				SP1.SoundPlay(3);
				SkillOver(this);
				states = playerStates.move;
			}
		}
	}
}
