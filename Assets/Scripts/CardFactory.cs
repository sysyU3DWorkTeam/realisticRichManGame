using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFactory : MonoBehaviour {
    /// <summary>
    /// 卡牌列表，储存没有被使用的卡牌
    /// </summary>
    private List<CardData> cardList = new List<CardData>();
    /// <summary>
    /// 完整的卡牌列表，提供卡牌丢失时候的应急操作
    /// </summary>
    private List<CardData> completeCardList = new List<CardData>();
    /// <summary>
    /// 初始化卡牌列表
    /// </summary>
    void Awake () {
        Debug.Log("OnEnable");
        for (int i = 1; i <= 6; i++)
        {
            cardList.Add(new CardData(i));
            cardList.Add(new CardData(i));
            cardList.Add(new CardData(i));
            cardList.Add(new CardData(i));
            completeCardList.Add(new CardData(i));
            completeCardList.Add(new CardData(i));
            completeCardList.Add(new CardData(i));
            completeCardList.Add(new CardData(i));
        }
	}
	/// <summary>
    /// 获取新的卡牌
    /// </summary>
    /// <returns></returns>
    public CardData Get()
    {
        int index = (int)Random.Range(0, cardList.Count);
        CardData result = cardList[index];
        cardList.RemoveAt(index);
        return result;
    }
    /// <summary>
    /// 回收卡牌，并提供新的卡牌。
    /// </summary>
    /// <param name="cardData"></param>
    public CardData Replace(CardData cardData)
    {
        cardList.Add(cardData);
        return Get();
    }
    /// <summary>
    /// 回收卡牌
    /// </summary>
    /// <param name="cardData"></param>
    public void Return(CardData cardData)
    {
        cardList.Add(cardData);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
