using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    public GameObject selectCardBoardPrefab;

    void Start ()
    {
        List<CardData> freeCardList = new List<CardData>();
        CardDataList completeCardList = LoadJson.LoadJsonFromFile<CardDataList>("/cardListData123.json");
        for (int i = 0; i < completeCardList.cardCount; i++)
        {
            freeCardList.Add(completeCardList.cardList[i]);
        }
        GameObject selectCardBoard = Instantiate(selectCardBoardPrefab);
        selectCardBoard.GetComponent<SelectCardBoardBH>().CreateSelectCardBoard(freeCardList);
    }
	
	// Update is called once per frame
	void Update () {
	}
}
