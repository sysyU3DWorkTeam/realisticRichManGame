using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum States
{
    move,
    skill,
    mapFunc
}


public class GameController : MonoBehaviour
{
    //用来实现简易状态机
    States state = States.move;

    //炒股
    public GameObject ticketUIObj;
    public GameObject ticketCamera;
    TicketUI ticketUI;

    CardFactory cardFactory;
    GameMap gameMap;
    InterActive act;
    Uicontroller ui;
    EventController eventController;
    EventBoard eventBoard;
    FunctionManager functionManager;

    bool isBusy = false;
    bool isMoveEnd = false;
    bool isSkillEnd = false;
    bool isMapEffectEnd = false;
    bool isFunctionEnd = false;
    //炒股结束
    bool isStockEnd = false;

    private bool isWaitIA;
    //public Sound_Play SP2;

    public PlayerBehaviour[] players = new PlayerBehaviour[1];
    //角色头像列表.......................................................................................新增(采用后可删)
    public Texture[] roleImage = new Texture[4];
    //角色头像列表.......................................................................................新增(采用后可删)

    private delegate void SelectCardCallBack();

    private List<CardData> mSelectedCard = new List<CardData>();
    private int selectCardNum;
    private List<PlayerBehaviour> mBeSelectedPlayer = new List<PlayerBehaviour>();
    private int selectPlayerNum;
    private bool replaceWithSelectCard;

    private class SelectImformation
    {
        public bool selectPlayer;
        public bool selectCard;
        public int playerNum;
        public int cardNum;
        public SelectCardCallBack callback;

        public SelectImformation(bool selectPlayer, bool selectCard, int playerNum, int cardNum, SelectCardCallBack callback)
        {
            this.selectPlayer = selectPlayer;
            this.selectCard = selectCard;
            this.playerNum = playerNum;
            this.cardNum = cardNum;
            this.callback = callback;
        }
    }

    //回合轮转数
    int index = 0;
    PlayerBehaviour currentPlayer;
    PlayerBehaviour otherPlayer;

    // Use this for initialization
    void Start()
    {
        functionManager = new FunctionManager();
        functionManager.AddFunction(MoneyFunction);
        functionManager.AddFunction(SourceFunction);
        functionManager.AddFunction(RoundFunction);
        functionManager.AddFunction(PositionFunction);
        functionManager.AddFunction(StealFunction);
        functionManager.AddFunction(ReplaceFunction);

        cardFactory = this.GetComponent<CardFactory>();
        gameMap = this.GetComponent<GameMap>();
        act = this.GetComponent<InterActive>();
        ui = this.GetComponent<Uicontroller>();
        eventController = this.GetComponent<EventController>();

        eventBoard = this.GetComponentInChildren<EventBoard>();
        eventBoard.leftbuttonEvent += OnMapEffect;

        ticketUI = ticketUIObj.GetComponent<TicketUI>();
        ticketUI.StockOver += OnStockOver;
        ticketCamera.SetActive(false);
        //ticketUIObj.SetActive(false);

        //注册监听事件
        //设置玩家头像
        int count = 0;
        foreach (var player in players)
        {
            //.................................设置玩家角色......................................新增（采用后删掉）
            int roleNum = PlayerPrefs.GetInt("roleNum" + count.ToString());
            player.setRole(roleImage[roleNum]);
			player.roleName = count;
			player.PD = player.LoadDataFromJson ("/PlayerData"+count.ToString()+".json");
            count++;
            //.....................................................................................................
            //player.gameController = this;
            player.MoveOver += OnMoveOver;
            player.SkillOver += OnSkillOver;
        }

        StartGame();
    }

    void StartGame()
    {
        //为每位玩家发牌
        foreach (PlayerBehaviour player in players)
        {
            //player.position = 0;
            player.transform.position = gameMap.getStation(player.mapPosition).transform.position;
            for (int i = 0; i < 6; i++)
            {
                player.AddCard(cardFactory.Get());
            }
        }
        NextRound();
    }

    void ChangeCurrentPlayer()
    {
        //轮换机制, 换到下一位玩家
        index = (index + 1);
        currentPlayer = players[index % players.Length];
        otherPlayer = players[(index + 1) % players.Length];
        Debug.Log("回合: " + index.ToString());
    }
    void ResetSelectImformation()
    {
        selectCardNum = 0;
        selectPlayerNum = 0;
        mBeSelectedPlayer.Clear();
        mSelectedCard.Clear();
    }
    void NextRound()
    {
        //换玩家
        ChangeCurrentPlayer();
        //有玩家需要被跳过的话, 就要多换几次
        while (currentPlayer.skipsTimes > 0)
        {
            currentPlayer.skipsTimes -= 1;
            Debug.Log("玩家" + currentPlayer.name + "该回合被跳过, 剩余的等待回合数为" + currentPlayer.skipsTimes);
            ChangeCurrentPlayer();
        }

        Debug.Log("当前玩家: " + currentPlayer.name);
        //显示玩家携带的所有卡牌
        act.ShowCard(currentPlayer.GetAllCard());
        //更新玩家金钱等讯息
        SendPlayerMessageToUI();
        //清空已选择的卡牌, 已选择的人物, 等
        ResetSelectImformation();
        return;
    }

    public void UseThisCard(Card card)
    {
        if (isBusy)
        {
            Debug.Log("正忙, 不接受卡牌请求");
            return;
        }
        //这个状态中人物是要移动的
        if (state == States.move)
        {
            StartCoroutine(PlayerMove(card));
            return;
        }
        //在这里决定卡牌技能对玩家造成的影响
        else if (state == States.skill)
        {
            if (card != null)
            {
                CardData cardData = card.CurCardData;
                //判断在这个位置能不能用这张牌
                if (cardData.address != gameMap.getStation(currentPlayer.mapPosition).stationData.name)
                {
                    Debug.Log("在 " + gameMap.getStation(currentPlayer.mapPosition).stationData.name + " 不能使用 " + cardData.address + " 卡牌");
                    return;
                }
                //判断钱够不够用卡牌
                else if (currentPlayer.PD.money < cardData.cost.money)
                {
                    Debug.Log("金钱不足, 不能使用这张牌");
                    return;
                }
                //判断分数够不够用卡牌
                else if (currentPlayer.PD.score < cardData.cost.point)
                {
                    Debug.Log("分数不足, 不能使用这张牌");
                    return;
                }
            }

            //使用技能
            StartCoroutine(ExeFunction(card));
            return;
        }
        return;
    }

    private IEnumerator PlayerMove(Card card)
    {
        //进入繁忙状态, 将不再监听卡牌
        isBusy = true;
        //GameController和IA的繁忙状态必须同步
        act.SetBusy(isBusy);

        if (card != null)
        {
            card.UseCard();
            CardData cardData = card.CurCardData;
            Debug.Log("选择卡牌 " + cardData.name + " 移动" + cardData.step + "步");

            //换牌
            CardData nCardData = cardFactory.Replace(cardData);
            //Debug.Log("放回旧牌: " + cardData.name + " 得到新牌: " + nCardData.name);
            card.CurCardData = nCardData;
            currentPlayer.UseCardMove(cardData, nCardData);

            //人物的地图位置需要改变
            var step = cardData.step;
            var startPosition = currentPlayer.mapPosition;
            currentPlayer.mapPosition += step;

            //人物的物理位置需要改变
            for (int i = startPosition + 1; i <= currentPlayer.mapPosition; i++)
            {
                currentPlayer.ObatinTargetPositon(gameMap.getStation(i).transform.position);
            }

            //等待人物移动完毕
            isMoveEnd = false;
            while (!isMoveEnd)
            {
                yield return new WaitForEndOfFrame();
            }

            Debug.Log("人物移动完毕");

        }

        //进入技能状态
        state = States.skill;
        //解除繁忙, 监听卡牌
        isBusy = false;
        //GameController和IA的繁忙状态必须同步
        act.SetBusy(isBusy);
    }

    private IEnumerator ExeFunction(Card card)
    {
        //进入繁忙状态, 将不再监听卡牌
        isBusy = true;
        //GameController和IA的繁忙状态必须同步
        act.SetBusy(isBusy);

        CardData cardData = null;
        CardData nCardData = null;
        ////////////////////////////卡牌阶段//////////////////////////////
        if (card != null)
        {
            cardData = card.CurCardData;
            card.UseCard();
            Debug.Log("选择卡牌 " + cardData.name + " 使用技能");

            /////////////////////应用数据效果/////////////////////
            //应用数据效果
            currentPlayer.PD.money += (-cardData.cost.money + cardData.gain.money);
            currentPlayer.PD.score += (-cardData.cost.point + cardData.gain.point);
            Debug.Log("玩家" + currentPlayer.name + "付出代价: 金钱:" + cardData.cost.money + " 分数:" + cardData.cost.point);
            Debug.Log("玩家" + currentPlayer.name + "得到增益: 金钱:" + cardData.gain.money + " 分数:" + cardData.gain.point);
            Debug.Log("玩家" + currentPlayer.name + "总分为: 金钱:" + currentPlayer.PD.money + " 分数:" + currentPlayer.PD.score);
            //执行特殊函数
            for (int i = 0; i < cardData.specialFunctions.Length; i++)
            {
                Debug.Log("this is " + i);
                Debug.Log("mode is " + cardData.specialFunctions[i].functionMode);
                isFunctionEnd = false;
                FunctionManager.FunctionDelegate function = functionManager.GetFunction(cardData.specialFunctions[i].functionMode);
                PlayerBehaviour player = GetPlayer(cardData.specialFunctions[i].self);
                function(player, cardData.specialFunctions[i].parameters);
                //等待前端响应
                while (!isFunctionEnd)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            /////////////////////为player换牌/////////////////////
            cardFactory.Return(cardData);
            //当if不存在, 只看else
            if (replaceWithSelectCard && mSelectedCard.Count > 0)
            {
                int index = Random.Range(0, mSelectedCard.Count - 1);
                nCardData = mSelectedCard[index];
                replaceWithSelectCard = false;
            }
            else
            {
                nCardData = cardFactory.Get();
            }
            Debug.Log("放回旧牌: " + cardData.name + cardData.id + " 得到新牌: " + nCardData.name + nCardData.id);
            isSkillEnd = false;
            card.CurCardData = nCardData;
            Debug.Log(card.name + "上的牌是: " + nCardData.name + nCardData.id);
            currentPlayer.UseCardSkill(cardData, nCardData);
            //等待玩家换牌完毕
            while (!isSkillEnd)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            currentPlayer.UseCardSkill(null, null);
        }
        state = States.mapFunc;

        isBusy = false;
        //GameController和IA的繁忙状态必须同步
        act.SetBusy(isBusy);

        StartCoroutine(ExeStationEvent());
    }

    private IEnumerator ExeStationEvent()
    {
        //进入繁忙状态, 将不再监听卡牌
        isBusy = true;
        //GameController和IA的繁忙状态必须同步
        act.SetBusy(isBusy);

        //////////////////////////地形效果阶段////////////////////////////
        //应用地形效果
        Debug.Log("展示给玩家看他将要受到的地图效果, 并等待确认");
        isMapEffectEnd = false;
        var mapStation = gameMap.getStation(currentPlayer.mapPosition);

        //要炒股了
        if (mapStation.stationData.name == "stock")
        {
            ticketUI.setPB(currentPlayer);
            ticketUI.UpdateStock();
            //ticketUIObj.transform.position = ticketUIPos;
            ticketCamera.SetActive(true);
            isStockEnd = false;
            while (!isStockEnd)
            {
                yield return new WaitForEndOfFrame();
            }
            //ticketUIObj.transform.position = unSeePos;
            ticketCamera.SetActive(false);
        }

        //这个地点的相关事件没有定义的话, 就不处罚地点事件
        var eventData = eventController.getEvent(mapStation);
        if (eventData != null)
        {
            eventBoard.ShowBoard(eventData);
            //等待玩家确认地形效果
            while (!isMapEffectEnd)
            {
                yield return new WaitForEndOfFrame();
            }
            //将地形效果应用到自玩家身上
            if (eventData != null)
            {
                var dataRef = eventData.eventData;
                currentPlayer.PD.money += dataRef.money;
                currentPlayer.PD.score += dataRef.point;
                Debug.Log("玩家" + currentPlayer.name + "受到地形效果: 金钱:" + dataRef.money + " 分数:" + dataRef.point);
                Debug.Log("玩家" + currentPlayer.name + "总分为: 金钱:" + currentPlayer.PD.money + " 分数:" + currentPlayer.PD.score + " 回合跳过:" + currentPlayer.skipsTimes);
            }
        }

        state = States.move;
        isBusy = false;
        //GameController和IA的繁忙状态必须同步
        act.SetBusy(isBusy);

        //下一回合
        NextRound();
    }
    /// <summary>
    /// imformation - 选择界面的基本数据集
    /// cardlist - （无视）
    /// AddSelectPlayerCard - (无视)
    /// </summary>
    /// <param name="imformation"></param>
    /// <param name="cardList"></param>
    /// <param name="AddSelectPlayerCard"></param>
    /// <returns></returns>
    private IEnumerator WaitPlayerSelect(SelectImformation imformation, List<CardData> cardList = null, bool AddSelectPlayerCard = false)
    {
        isWaitIA = true;
        if (imformation.selectPlayer)
        {
            selectPlayerNum = imformation.playerNum;
            if (players.Length > 2)
            {
                //exhibit players  

                while (isWaitIA)
                {
                    yield return new WaitForEndOfFrame();
                }
                isWaitIA = true;
            }
            else
            {
                //后期改
                mBeSelectedPlayer.Add(otherPlayer);
            }
        }
        if (imformation.selectCard)
        {
            List<CardData> exhibitCardList = new List<CardData>();
            if (cardList != null)
            {
                exhibitCardList = cardList;
                if (AddSelectPlayerCard)
                {
                    for (int i = 0; i < mBeSelectedPlayer.Count; i++)
                    {
                        exhibitCardList.AddRange(mBeSelectedPlayer[i].GetAllCard());
                    }
                }
            }
            else
            {
                for (int i = 0; i < mBeSelectedPlayer.Count; i++)
                {
                    exhibitCardList.AddRange(mBeSelectedPlayer[i].GetAllCard());
                }
            }
            selectCardNum = imformation.cardNum;
            act.ExhibitCard(exhibitCardList);
            while (isWaitIA)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        imformation.callback();
        isWaitIA = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="self">true-currentplayer, false-other</param>
    /// <returns></returns>
    private PlayerBehaviour GetPlayer(bool self)
    {
        if (self)
        {
            return currentPlayer;
        }
        else
        {
            return otherPlayer;
        }
    }

    //玩家行走完毕
    public void OnMoveOver(PlayerBehaviour sender)
    {
        if (sender != currentPlayer) { Debug.Log("!="); return; }
        //显示玩家金钱等讯息
        SendPlayerMessageToUI();
        isMoveEnd = true;
    }

    //玩家使用技能完毕
    public void OnSkillOver(PlayerBehaviour sender)
    {
        if (sender != currentPlayer) { Debug.Log("!="); return; }
        //更新显示玩家金钱等讯息
        SendPlayerMessageToUI();
        isSkillEnd = true;
    }

    //玩家确认后对玩家应用地图效果
    public void OnMapEffect()
    {
        //更新显示玩家金钱等讯息
        SendPlayerMessageToUI();
        isMapEffectEnd = true;
    }

    //炒股结束后
    public void OnStockOver()
    {
        //更新显示玩家金钱等讯息
        SendPlayerMessageToUI();
        isStockEnd = true;
    }

    //更新玩家的讯息到UI
    public void SendPlayerMessageToUI()
    {
        ui.ShowPlayerMessage(players);
        //ui.SetMoney(currentPlayer.money);
        //ui.SetPoint(currentPlayer.score);
    }

    public bool SelectCard(Card card)
    {
        if (Mathf.Abs(selectCardNum) > mSelectedCard.Count)
        {
            mSelectedCard.Add(card.CurCardData);
            return true;
        }
        Debug.Log("超过数量限制");
        return false;
    }

    public void UnSelectCard(Card card)
    {
        mSelectedCard.Remove(card.CurCardData);
        return;
    }


    // Update is called once per frame
    void Update()
    {
    }

    public bool EndSelect()
    {
        bool ans = true;
        if (selectCardNum >= 0)
        {
            ans = (selectCardNum == mSelectedCard.Count);
            Debug.Log(mSelectedCard.Count.ToString() + ans);
        }
        else
        {
            ans = (Mathf.Abs(selectCardNum) >= mSelectedCard.Count);
            Debug.Log(mSelectedCard.Count.ToString() + ans);
        }
        if (selectPlayerNum >= 0)
        {
            ans = (selectPlayerNum == mBeSelectedPlayer.Count);
            Debug.Log(selectPlayerNum.ToString() + mBeSelectedPlayer.Count.ToString() + ans);
        }
        else
        {
            ans = (Mathf.Abs(selectPlayerNum) >= mBeSelectedPlayer.Count);
            Debug.Log(selectPlayerNum.ToString() + mBeSelectedPlayer.Count.ToString() + ans);
        }
        if (ans)
        {
            isWaitIA = false;
        }
        return ans;
    }

    private void MoneyFunction(PlayerBehaviour player, int[] parameterList)
    {
        //List<int> parameterList = FunctionManager.GetParamaters(parameters);
        player.PD.money += parameterList[0];
        isFunctionEnd = true;
        Debug.Log("玩家" + player.name + "金钱变化: " + parameterList[0]);
        Debug.Log("玩家" + player.name + "总金钱: " + player.PD.money);
    }

    private void SourceFunction(PlayerBehaviour player, int[] parameterList)
    {
        //List<int> parameterList = FunctionManager.GetParamaters(parameters);
        player.PD.score += parameterList[0];
        isFunctionEnd = true;
        Debug.Log("玩家" + player.name + "分数变化: " + parameterList[0]);
        Debug.Log("玩家" + player.name + "总分数: " + player.PD.score);
    }

    private void RoundFunction(PlayerBehaviour player, int[] parameterList)
    {
        //List<int> parameterList = FunctionManager.GetParamaters(parameters);
        player.skipsTimes += parameterList[0];
        isFunctionEnd = true;
        Debug.Log("玩家" + player.name + "回合跳过数变化: " + parameterList[0]);
        Debug.Log("玩家" + player.name + "总回合跳过数: " + player.skipsTimes);
    }

    private void PositionFunction(PlayerBehaviour player, int[] parameterList)
    {
        //List<int> parameterList = FunctionManager.GetParamaters(parameters);
        player.ObatinTargetPositon(gameMap.getStation(parameterList[0]).transform.position);
    }

    private void StealFunction(PlayerBehaviour player, int[] parameterList)
    {
        //List<int> parameterList = FunctionManager.GetParamaters(parameters);

        StartCoroutine(WaitPlayerSelect(new SelectImformation(true, true, 1, 1, BeStoleCard)));
    }

    private void ReplaceFunction(PlayerBehaviour player, int[] parameterList)
    {
        //debug
        var before = player.GetAllCard();
        foreach (var card in before)
        {
            Debug.Log("before: " + card.name + card.id);
        }

        //得到人物身上的卡牌列表
        var playerCardList = player.GetAllCard();
        for (int i = 0; i < playerCardList.Count; i++)
        {
            //替换掉所有非使用中的牌
            if (playerCardList[i].id != parameterList[0])
            {
                playerCardList[i] = cardFactory.Replace(playerCardList[i]);
            }
        }

        //刷新显示
        act.ShowCardDiff(currentPlayer.GetAllCard());
        isFunctionEnd = true;

        //debug
        var after = player.GetAllCard();
        foreach (var card in before)
        {
            Debug.Log("after: " + card.name + card.id);
        }
    }

    private void BeStoleCard()
    {
        for (int i = 0; i < mBeSelectedPlayer.Count; i++)
        {
            for (int j = 0; j < mSelectedCard.Count; j++)
            {
                CardData nCardData = cardFactory.Replace(mSelectedCard[j]);
                mBeSelectedPlayer[j].ReplaceCard(mSelectedCard[j], nCardData);
            }
        }
        isFunctionEnd = true;
        replaceWithSelectCard = true;
    }
}
