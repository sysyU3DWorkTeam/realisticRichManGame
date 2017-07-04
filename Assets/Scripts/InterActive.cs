using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterActive : MonoBehaviour
{

    enum State
    {
        ZERO,
        ONE,
    }

    public Sound_Play SP;
    private State state;

    //用来显示卡片的
    public GameObject[] cardsInterface = new GameObject[6];
    private Card[] cards = new Card[6];
    //结束该阶段的按钮
    public GameObject endButton;
    public GameObject selectCardBoardPrefab;

    private GameObject selectCardBoard;
    bool isBusy = false;

    GameController gameController;
    /// <summary>
    /// 鼠标选择的gameboject
    /// </summary>
    private GameObject selectObject = null;

    void Awake()
    {
        gameController = this.GetComponent<GameController>();
        for (int i = 0; i < cardsInterface.Length; i++)
        {
            cards[i] = cardsInterface[i].GetComponent<Card>();
        }
        state = State.ZERO;
    }

    void Start()
    {
        SP = GameObject.Find("SceneDirector").GetComponent<Sound_Play>();
    }
    void Update()
    {
        MouseClick();
    }

    /// <summary>
    /// 设置繁忙状态
    /// </summary>
    /// <param name="busy"></param>
    public void SetBusy(bool busy)
    {
        isBusy = busy;
        //繁忙时结束按钮不可视化
        endButton.SetActive(!busy);
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
                    StimulateObj(hit.collider.gameObject);
                }
            }
        }
    }
    /// <summary>
    /// Select界面结束
    /// 外部button调用
    /// </summary>
    public void EndSelect()
    {
        if (gameController.EndSelect())
        {
            Debug.Log("选择成功，关闭选择界面");
            Destroy(selectCardBoard);
            //关闭显示
            state = State.ZERO;
        }
    }

    private void StimulateObj(GameObject obj)
    {
        switch (state)
        {
            case State.ZERO:
                {
                    if (obj.transform.tag == "CardTag")
                    {
                        if (SP != null) SP.SoundPlay(0);

                        //Debug.Log(obj.transform.name);
                        gameController.UseThisCard(obj.GetComponent<Card>());
                    }
                    //如果点击的是结束按钮, 传入null, GC那边对null有特殊处理
                    if (obj.transform.tag == "EndButton")
                    {
                        if (SP != null) SP.SoundPlay(0);
                        gameController.UseThisCard(null);
                    }
                    break;
                }
            case State.ONE:
                {
                    if (obj.transform.tag == "ExhibitCard")
                    {
                        if (SP != null) SP.SoundPlay(0);
                        Card card = obj.GetComponent<Card>();
                        if (card.state == Card.State.SELECT)
                        {
                            card.UnSelectCard();
                            gameController.UnSelectCard(card);
                        }
                        else if (card.state == Card.State.NORMAL)
                        {
                            if (gameController.SelectCard(card))
                            {
                                card.SelectCard();
                            }
                        }
                    }
                    if (obj.transform.tag == "EndButton")
                    {
                        Debug.Log("EndButton click, 选择结束");
                        if (SP != null) SP.SoundPlay(0);
                        EndSelect();
                    }
                    break;
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

    //从控制器得到卡片列表并刷新卡片显示
    public void ShowCardDiff(List<CardData> allCards)
    {
        for (int i = 0; i < allCards.Count && i < cardsInterface.Length; i++)
        {
            if (allCards[i].id != cards[i].CurCardData.id)
                cards[i].Shuffle(allCards[i]);
        }
    }

    /// <summary>
    /// 显示卡片，根据cardlist创建card并显示
    /// </summary>
    /// <param name="cardList"></param>
    public void ExhibitCard(List<CardData> cardList)
    {
        selectCardBoard = Instantiate(selectCardBoardPrefab);
        selectCardBoard.GetComponent<SelectCardBoardBH>().CreateSelectCardBoard(cardList);

        state = State.ONE;
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