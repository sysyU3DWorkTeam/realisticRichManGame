using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardDataList
{
    //public string cardListName;
    //public string version;
    public int cardCount;
    public List<CardData> cardList;
}

public class CardFactory : MonoBehaviour
{

    /// <summary>
    /// 卡牌列表，储存没有被使用的卡牌
    /// </summary>
    private List<CardData> freeCardList = new List<CardData>();

    /// <summary>
    /// 完整的卡牌列表，提供卡牌丢失时候的应急操作
    /// </summary>
    private CardDataList completeCardList;

    /// <summary>
    /// 初始化卡牌列表
    /// </summary>
    void Awake()
    {
        //Debug.Log("OnEnable");
        completeCardList = LoadJson.LoadJsonFromFile<CardDataList>("/cardListData.json");
        for (int i = 0; i < completeCardList.cardCount; i++)
        {
            freeCardList.Add(completeCardList.cardList[i]);
        }
        //Debug.Log(freeCardList.Count + "awake");
    }
    /// <summary>
    /// 获取新的卡牌
    /// </summary>
    /// <returns></returns>
    public CardData Get()
    {
        int index = Random.Range(0, freeCardList.Count - 1);
        CardData result = freeCardList[index];
        freeCardList.RemoveAt(index);
        return result;
    }
    /// <summary>
    /// 回收卡牌，并提供新的卡牌。
    /// </summary>
    /// <param name="cardData"></param>
    public CardData Replace(CardData cardData)
    {
        freeCardList.Add(cardData);
        return Get();
    }
    /// <summary>
    /// 回收卡牌
    /// </summary>
    /// <param name="cardData"></param>
    public void Return(CardData cardData)
    {
        freeCardList.Add(cardData);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
