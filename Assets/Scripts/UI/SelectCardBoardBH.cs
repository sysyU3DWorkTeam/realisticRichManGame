using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCardBoardBH : MonoBehaviour {

    public GameObject cardBoardPrefab;
    public GameObject nextPageButton;
    public GameObject lastPageButton;

    private List<CardBoard> cardBoardList = new List<CardBoard>();
    private List<CardData> exhibitCardList = new List<CardData>();
    private CardBoard curCardBoard = null;
    private int cardBoardPage = 0;
    private int totalPage = 0;
	// Use this for initialization
	void Start () {
        Transform[] t = GetComponentsInChildren<Transform>();
        foreach(Transform ts in t) {
            //Debug.Log(ts.name);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateSelectCardBoard(List<CardData> cardDataList)
    {
        exhibitCardList = cardDataList;
        totalPage = exhibitCardList.Count % 6 == 0 ? 
            exhibitCardList.Count / 6 : exhibitCardList.Count / 6 + 1;
        ShowBoard(0);
    }

    private CardBoard AddCardBoard(List<CardData> cardDataList)
    {
        CardBoard cardBoard = Instantiate(cardBoardPrefab).GetComponent<CardBoard>();
        cardBoard.transform.SetParent(transform);
        cardBoard.transform.position = transform.position + new Vector3(0f, 4.5f, 0f);
        for (int i = 0; i < cardDataList.Count; i++)
        {
            cardBoard.AddCard(cardDataList[i]);
        }
        cardBoardList.Add(cardBoard);
        return cardBoard;
    }

    private void ShowBoard(int page)
    {
        CardBoard nextCardBoard = null;
        if (page >= cardBoardList.Count)
        {
            List<CardData> addCardList = new List<CardData>();
            for (int i = 6 * page; i < exhibitCardList.Count && i - page * 6 < 6; i++)
            {
                addCardList.Add(exhibitCardList[i]);
            }
            nextCardBoard = AddCardBoard(addCardList);
        }
        else
        {
            nextCardBoard = cardBoardList[page];
        }
        if (curCardBoard != null)
        {
            curCardBoard.gameObject.SetActive(false);
        }
        curCardBoard = nextCardBoard;
        curCardBoard.gameObject.SetActive(true);
        lastPageButton.SetActive(page > 0);
        nextPageButton.SetActive(page < totalPage - 1);
    }
    public void NextBoard()
    {
        if (cardBoardPage >= totalPage) { Debug.Log("页数上溢"); return; }
        ShowBoard(++cardBoardPage);
    }
    public void LastBoard()
    {
        if (cardBoardPage < 0) { Debug.Log("页数下溢"); return; }
        ShowBoard(--cardBoardPage);
    }
}
