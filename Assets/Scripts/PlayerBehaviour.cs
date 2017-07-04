using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

enum playerStates {
	move,
	skill
}  

[SerializeField]
public class PlayerData {
	public int money;
	public int score;
	public int[] ticketId;
	public float[] price;
	public int[] ticketNum;
	public PlayerData() {
		ticketId = new int[15]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14};
		price = new float[15]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
		ticketNum = new int[15]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
		score = 10000;
		money = 10000;
	}
}

public class PlayerBehaviour : MonoBehaviour {

    public delegate void OnActionOver(PlayerBehaviour sender);
    public event OnActionOver MoveOver;
    public event OnActionOver SkillOver;

	public PlayerData PD;
    public GameObject player;  // 玩家模型
	public GameObject speakImage;
	public Texture role;
	//public GameController gameController;

	public Sound_Play SP1; //移动音效
	public int roleName = -1;
	public int skipsTimes = 0;
    public int mapPosition = 0;
    private List<Vector3> myPositionList = new List<Vector3>(); //存放控制器给的位置参数 
    private List<CardData> myCardList = new List<CardData>(); //绑定卡牌类， 每个人绑定6个卡牌，用一个卡牌list来保存
	public SpeakData SD;

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
		SD = GameObject.Find ("players").GetComponent<SpeakData>();
        player = this.gameObject;
		//PD = new PlayerData ();
		//skipsTimes = 0;
		//score = 0;
		//money = 0;
		states = playerStates.move;
		SP1 = GameObject.Find ("SceneDirector").GetComponent<Sound_Play>();
        loadsrc();
		ScreenSetting ();
    }
	
	// 每一帧获取新的参数，移动player
	void Update () {
        PlayerMove();
    }
	void Awake() {
		
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
	IEnumerator Speak() {
		speakImage.transform.position = this.transform.position + new Vector3 (2f, 5f, 0.5f);
		speakImage.SetActive (true);
		yield return new WaitForSeconds (1.5f);
		speakImage.SetActive (false);
		SkillOver (this);
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
        if (card != null && nCard != null)
        {
            myCardList[myCardList.IndexOf(card)] = nCard;
        }
		return;
	}


    //使用卡牌，同时从自己的list里面删除卡牌
     public void UseCardSkill(CardData card, CardData nCard)
    {
        if (card != null && nCard != null)
        {
            myCardList[myCardList.IndexOf(card)] = nCard;
            this.states = playerStates.move;
            speakImage.GetComponent<SpriteRenderer>().sprite = SD.getSpeak();
            StartCoroutine(Speak());
        }
        return;
    }

    public void ReplaceCard(CardData card, CardData nCard)
    {
        for (int i = 0; i < myCardList.Count; i++)
        {
            if (myCardList[i].id.Equals(card.id))
            {
                myCardList[i] = nCard;
                break;
            }
        }
        return;
    }

    //换很多张牌
    public void ReplaceCards(List<CardData> cardDatas)
    {
        for (int i = 0; i < cardDatas.Count && i < myCardList.Count; i++)
        {
            myCardList[i] = cardDatas[i];
        }
    }

    public void findcard(CardData Card)
    {

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

	//void OnGUI() {
	//	if (states == playerStates.skill) {
	//		GUI.Label (new Rect (screenX - end_btn.width * 0.5f + end_btn.width*0.3f,
	//			end_btn.height * 0.1f+10, 
	//			300, end_btn.height*0.3f),
	//			"是否发动技能?",guiLabelStyle);
	//		;
	//		if (GUI.Button (new Rect (screenX - end_btn.width * 0.5f,
	//			end_btn.height * 0.1f, 
	//			end_btn.width*0.2f, end_btn.height*0.2f	),
	//			end_btn,
	//			guiRectStyle))
	//		{
	//			SP1.SoundPlay(3);
	//			SkillOver(this);
	//			states = playerStates.move;
	//		}
	//	}
	//}

	public void setRole(Texture _role) {
		role = _role;
		this.GetComponent<Renderer> ().material.mainTexture = role;
	}
	public PlayerData LoadDataFromJson(string path)
	{
		Debug.Log (path);
		if (path == "" || !File.Exists(Application.dataPath + path))
		{
			//如果没有存档就读初始化文件
			if (!File.Exists (Application.dataPath + "/InitPlayerData.json"))
				return null;
			else
				path = "/InitPlayerData.json";
		}
		StreamReader sr = new StreamReader(Application.dataPath + path);
		Debug.Log (path);

		if (sr == null)
		{
			return null;
		}
		string json = sr.ReadToEnd();
		Debug.Log (json);

		if (json.Length > 0)
		{
			return JsonUtility.FromJson<PlayerData>(json);
		}

		return null;
	}
	//将股市中的股票数量写入json进行保存
	public bool WriteToJson(string path)
	{
		StreamWriter sw;
		FileInfo f = new FileInfo (Application.dataPath + path);
		if (f.Exists)
			f.Delete ();
		sw = f.CreateText ();

		string json = JsonUtility.ToJson (PD);
		sw.WriteLine (json);
		sw.Close();
		sw.Dispose ();
		return true;
	}
	public void OnApplicationQuit() {
		WriteToJson ("/PlayerData" +roleName.ToString()+".json");
	}
}
