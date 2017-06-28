using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterActive : MonoBehaviour {

    public Sound_Play SP;

    //用来显示卡片的
    public GameObject[] cardsInterface = new GameObject[6];
    private Card[] cards = new Card[6];
    GameController gameController;
    /// <summary>
    /// 鼠标选择的gameboject
    /// </summary>
    private GameObject selectObject = null;

    void Awake () {
        gameController = this.GetComponent<GameController>();
        for (int i = 0; i < cardsInterface.Length; i++)
        {
            cards[i] = cardsInterface[i].GetComponent<Card>();
        }
	}

    void Update()
    {
        MouseClick();
    }
    /// <summary>
    /// 处理鼠标点击事件
    /// </summary>
    void MouseClick()
    {
        //检测鼠标按下到哪个gameobject
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                selectObject = hit.collider.gameObject;
            }
        }
        //检测鼠标松开到哪个卡，如果与按下gameobject相同，则执行点击事件
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (selectObject == hit.collider.gameObject)
                {
                    if (hit.transform.tag == "CardTag")
                    {
                        if (SP != null) SP.SoundPlay(0);
                        
                        Debug.Log(hit.transform.name);
                        hit.collider.GetComponent<Card>().CurCardData = gameController.UseThisCard(hit.transform.GetComponent<Card>().UseCard());
                    }
                }
            }
        }
    }

    //从控制器得到卡片列表并刷新卡片显示
    public void ShowCard(List<CardData> allCards)
    {
        for (int i = 0; i < allCards.Count && i < cardsInterface.Length; i++)
        {
            cards[i].Shuffle(allCards[i]);
        }
    }
    
}

//public class CardInterface : MonoBehaviour
//{
//    public Card card;
//    public TextMesh text;
//    public InterActive inter;

//    //得到子对象中的text
//    void Start()
//    {
//        text = this.GetComponentInChildren<TextMesh>();
//        text.text = "hello";
//    }

//    //设置他们的老大
//    public void SetInter(InterActive inter)
//    {
//        this.inter = inter;
//    }
    
//    //鼠标点下时会呼叫老大, 报告自己被点了
//    void OnMouseDown()
//    {
//        if (inter == null)
//        {
//            Debug.Log("InterActive is null");
//            return;
//        }
//        inter.CardBeClick(this);
//    }

//    //将卡片存下来并在text上显示卡片讯息
//    public void ShowThisCard(Card card)
//    {
//        this.card = card;
//        text.text = card.CardData.step.ToString();
//    }
//}