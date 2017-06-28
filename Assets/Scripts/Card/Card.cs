using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour{

    public enum State
    {
        NORMAL,
        SELECT,
        USE
    }

    //private TextMesh textMesh;
    public Renderer CardSurRenderer;
    private Animator animator;
    private BoxCollider boxCollider;
    private Material material;
    private CardData mCurCardData;

    public State state = State.NORMAL;

    public CardData CurCardData
    {
        get
        {
            return mCurCardData;
        }
        set
        {
            if (state == State.NORMAL && value != null)
            {
                mCurCardData = value;
                OnCardDataChange();
            }
        }
    }
    
    private void Awake()
    {
        //textMesh = GetComponentInChildren<TextMesh>();
        animator = GetComponent<Animator>();
        material = CardSurRenderer.material;
        boxCollider = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        //StartWaitInit();
    }
    private void Update()
    {

    }
    /// <summary>
    /// carddata修改时执行的操作
    /// </summary>
    /// <param name="cardData"></param>
    private void OnCardDataChange()
    {
        animator.SetTrigger("Use");
        state = State.USE;
        boxCollider.enabled = false;
    }

    /// <summary>
    /// 使用卡片调用
    /// </summary>
    /// <returns></returns>
    public CardData UseCard()
    {
        //animator.SetTrigger("Use");
        //isInited = false;
        //state = State.USE;
        //boxCollider.enabled = false;
        return CurCardData;
    }

    private IEnumerator WaitNewData()
    {
        while(state != State.USE)
        {
            yield return new WaitForEndOfFrame();
        }
        Texture t = Resources.Load(CurCardData.name) as Texture;
        material.SetTexture("_MainTex", t);
        animator.SetTrigger("Init");
        state = State.NORMAL;
        boxCollider.enabled = true;
    }
    /// <summary>
    /// 洗牌操作
    /// </summary>
    /// <param name="nCardData"></param>
    public void Shuffle(CardData nCardData)
    {
        UseCard();
        CurCardData = nCardData;
    }

    public void ReloadCard()
    {
        StartCoroutine("WaitNewData");
    }
    /// <summary>
    /// 选择卡片调用
    /// </summary>
    public void SelectCard()
    {
        animator.SetTrigger("Select");
    }
}
