using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour{

    private TextMesh textMesh;
    
    private CardData mCardData;
    public CardData CardData
    {
        get
        {
            return mCardData;
        }
        set
        {
            mCardData = value;
            ExpCardData(mCardData);
        }
    }

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMesh>();
    }
    /// <summary>
    /// carddata修改时执行的操作
    /// </summary>
    /// <param name="cardData"></param>
    private void ExpCardData(CardData cardData)
    {
        textMesh.text = cardData.step.ToString();
    }
}
