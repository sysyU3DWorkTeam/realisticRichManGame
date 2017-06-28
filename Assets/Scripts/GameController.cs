using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum States
{
    move,
    skill
}

public class GameController : MonoBehaviour
{
    States state = States.move;
    
    CardFactory cardFactory;
    GameMap gameMap;
    InterActive act;
    Uicontroller ui;

    bool isBusy = false;

	//public Sound_Play SP2;

    public PlayerBehaviour[] players = new PlayerBehaviour[1];
    //回合轮转数
    int index = 0;
    PlayerBehaviour currentPlayer;

    // Use this for initialization
    void Start()
    {
        cardFactory = this.GetComponent<CardFactory>();
        gameMap = this.GetComponent<GameMap>();
        act = this.GetComponent<InterActive>();
        ui = this.GetComponent<Uicontroller>();

        //注册监听事件
        foreach (var player in players)
        {
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
            player.transform.position = gameMap.getStation(0).transform.position;
            for (int i = 0; i < 6; i++)
            {
                player.AddCard(cardFactory.Get());
            }
        }

        //当前玩家为第一位玩家
        currentPlayer = players[index];
        NextRound();
    }

    void ChangeCurrentPlayer()
    {
        Debug.Log("回合: " + index.ToString());

        //轮换机制, 换到下一位玩家
        index = (index + 1);
        currentPlayer = players[index % players.Length];
    }

    void NextRound()
    {
        //换玩家
        ChangeCurrentPlayer();
        //有玩家需要被跳过的话, 就要多换几次
        while (currentPlayer.skipsTimes > 0)
        {
            currentPlayer.skipsTimes -= 1;
            ChangeCurrentPlayer();
        }

        //显示玩家携带的所有卡牌
        act.ShowCard(currentPlayer.GetAllCard());
        //更新玩家金钱等讯息
        SendPlayerMessageToUI();
        return;
    }

    public CardData UseThisCard(CardData cardData)
    {
        if (isBusy)
        {
            Debug.Log("正忙, 不接受卡牌请求");
            return null;
        }

        //进入繁忙状态, 将不再监听卡牌
        isBusy = true;

        //这个状态中人物是要移动的
        if (state == States.move)
        {
            Debug.Log("选择卡牌移动");

            //人物的地图位置需要改变
            var step = cardData.step;
            var startPosition = currentPlayer.mapPosition;
            currentPlayer.mapPosition += step;

            //人物的物理位置需要改变
            for (int i = startPosition + 1; i <= currentPlayer.mapPosition; i++)
            {
                currentPlayer.ObatinTargetPositon(gameMap.getStation(i).transform.position);
            }

            CardData nCardData = cardFactory.Replace(cardData);
            currentPlayer.UseCardMove(cardData, nCardData);
            Debug.Log(cardData.id + " " + nCardData.id);
            //更新卡牌的显示界面

            state = States.skill;
            return nCardData;
        }

        //在这里决定卡牌技能对玩家造成的影响
        //Todo
        else
        {
            Debug.Log("选择卡牌放技能");

			//if (cardData.buffFunction.money != 0 || cardData.buffFunction.point != 0) { //卡牌应用到自己身上的音效
			//	SP2.SoundPlay(4);
			//}
			//else SP2.SoundPlay(5); //卡牌应用到对方身上的音效

            //将卡牌正面效果应用到自玩家身上
            currentPlayer.money += (-cardData.cost.money + cardData.buffFunction.money);
            currentPlayer.score += (-cardData.cost.point + cardData.buffFunction.point);
            currentPlayer.skipsTimes += (cardData.cost.roundSkip);

            //将卡牌负面效果应用到对面玩家身上
            //Todo

            //Delete
            //currentPlayer.score += 1;
            //删掉玩家使用的牌并给新牌
            CardData nCardData = cardFactory.Replace(cardData);
            currentPlayer.UseCardSkill(cardData, nCardData);
            Debug.Log(cardData.id + " " + nCardData.id);
            //更新卡牌的显示界面
            act.ShowCard(currentPlayer.GetAllCard());

            state = States.move;

            return nCardData;
        }
    }

    //玩家行走完毕
    public void OnMoveOver(PlayerBehaviour sender)
    {
        if (sender != currentPlayer)
        {
            Debug.Log("!=");
            return;
        }
        //显示玩家金钱等讯息
        SendPlayerMessageToUI();
        //解除繁忙, 继续监听卡牌
        isBusy = false;
    }

    //玩家选择放弃使用技能
    public void OnSkillOver(PlayerBehaviour sender)
    {
        if (sender != currentPlayer)
        {
            Debug.Log("!=");
            return;
        }
        //更新显示玩家金钱等讯息
        SendPlayerMessageToUI();

        //解除繁忙, 继续监听卡牌
        isBusy = false;
        //将状态设置回行走状态
        if (state == States.skill)
            state = States.move;

        //下一回合
        NextRound();
    }

    //更新玩家的讯息到UI
    public void SendPlayerMessageToUI()
    {
        ui.ShowPlayerMessage(players);
        //ui.SetMoney(currentPlayer.money);
        //ui.SetPoint(currentPlayer.score);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
