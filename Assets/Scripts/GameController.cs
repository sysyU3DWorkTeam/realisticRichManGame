using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum States
{
    move,
    skill
}

public class GameController : MonoBehaviour {
    States state = States.move;

    CardFactory cardFactory;
    GameMap gameMap;
    InterActive act;
    Uicontroller ui;

    bool isBusy = false;

    public PlayerBehaviour[] players = new PlayerBehaviour[1];
    int index = 0;
    PlayerBehaviour currentPlayer;

    // Use this for initialization
    void Start () {
        cardFactory = this.GetComponent<CardFactory>();
        gameMap = this.GetComponent<GameMap>();
        act = this.GetComponent<InterActive>();
        ui = this.GetComponent<Uicontroller>();

        foreach (var player in players) {
            player.gameController = this;
        }

        StartGame();
	}
	
    void StartGame()
    {


        //为每位玩家发牌
        foreach (PlayerBehaviour player in players)
        {
            player.position = 0;
            player.transform.position = gameMap.getStationPostion(0);
            for (int i = 0; i < 6; i++)
            {
                player.AddCard(cardFactory.Get());
            }
        }

        //当前玩家为第一位玩家
        currentPlayer = players[index];
        NextRound();
    }

    void NextRound()
    {
        Debug.Log("新的回合");

        //轮换机制, 换到下一位玩家
        index = (index + 1) % players.Length;
        currentPlayer = players[index];

        //显示玩家携带的所有卡牌
        act.ShowCard(currentPlayer.GetAllCard());

        //显示玩家金钱等讯息
        SendPlayerMessageToUI();
    }

    public CardData UseThisCard(CardData cardData)
    {
        if (isBusy)
        {
            Debug.Log("正忙, 不接受卡牌请求");
            return cardData;
        }

        //进入繁忙状态, 将不再监听卡牌
        isBusy = true;

        //这个状态中人物是要移动的
        if (state == States.move)
        {
            Debug.Log("选择卡牌移动");

            //人物的地图位置需要改变
            var step = cardData.step;
            var startPosition = currentPlayer.position;
            currentPlayer.position += step;

            //人物的物理位置需要改变
            for (int i = startPosition + 1; i <= currentPlayer.position; i++)
            {
                currentPlayer.ObatinTargetPositon(gameMap.getStationPostion(i));
            }

            CardData nCardData = cardFactory.Replace(cardData);
            currentPlayer.UseCardMove(cardData, nCardData);

            state = States.skill;
            return nCardData;
        }

        //在这里决定卡牌技能对玩家造成的影响
        //Todo
        else
        {
            Debug.Log("选择卡牌放技能");
            //为玩家加一分, 测试, 需删除
            //Delete
            currentPlayer.score += 1;
            //删掉玩家使用的牌并给新牌
            CardData nCardData = cardFactory.Replace(cardData);
            currentPlayer.UseCardSkill(cardData, nCardData);
            state = States.move;

            return nCardData;
        }
    }

    //玩家行走完毕
    public void OnMoveOver()
    {
        //显示玩家金钱等讯息
        SendPlayerMessageToUI();
        //解除繁忙, 继续监听卡牌
        isBusy = false;
    }

    //玩家选择放弃使用技能
    public void OnSkillOver()
    {
        //显示玩家金钱等讯息
        SendPlayerMessageToUI();
        //解除繁忙, 继续监听卡牌
        isBusy = false;
        //将状态设置回行走状态
        if (state == States.skill)
            state = States.move;

        //下一回合
        NextRound();
    }

    //同步玩家的讯息到UI
    public void SendPlayerMessageToUI()
    {
        ui.SetMoney(currentPlayer.money);
        ui.SetPoint(currentPlayer.score);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
