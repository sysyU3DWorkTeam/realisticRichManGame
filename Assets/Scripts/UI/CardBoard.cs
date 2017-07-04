using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBoard : MonoBehaviour {

    public GameObject CardPrefab;
    
    private Queue<Transform> freeCardQueue = new Queue<Transform>();
    private List<Card> cardList = new List<Card>();
	// Use this for initialization
	void Awake () {
        Transform[] cards = GetComponentsInChildren<Transform>();
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].transform != transform)
            {
                freeCardQueue.Enqueue(cards[i]);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddCard(CardData cardData)
    {
        GameObject card = Instantiate(CardPrefab);
        Transform cardTransform = freeCardQueue.Dequeue();
        card.transform.position = cardTransform.position;
        card.transform.rotation = cardTransform.rotation;
        card.transform.SetParent(cardTransform);
        card.transform.tag = "ExhibitCard";
        card.GetComponent<Card>().SetCardTexture(cardData);
        cardList.Add(card.GetComponent<Card>());
    }
}
