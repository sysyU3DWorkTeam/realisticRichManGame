using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventBoard : MonoBehaviour {

    /// <summary>
    /// 左按钮（默认按钮）
    /// </summary>
    public Button leftButton;
    /// <summary>
    /// 右按钮
    /// </summary>
    public Button rightButton;
    public RawImage rawImage;

    /// <summary>
    /// 按钮事件委托
    /// </summary>
    public delegate void buttonDelegate();
    /// <summary>
    /// 左按钮响应事件
    /// </summary>
    public event buttonDelegate leftbuttonEvent;

    /// <summary>
    /// 右按钮响应事件
    /// </summary>
    public event buttonDelegate rightbuttonEvent;


    private Transform panel;
    private RectTransform leftButtonRect;
    private RectTransform rightButtonRect;

    // Use this for initialization
    void Start () {
        panel = this.transform.FindChild("EventBoardPanel");
        leftButtonRect = leftButton.GetComponent<RectTransform>();
        rightButtonRect = rightButton.GetComponent<RectTransform>();

        leftButton.onClick.AddListener(delegate ()
        {
            //Debug.Log(this.gameObject.name + "被点击了啦");
            CloseBoard();
            //原先没有调用left事件
            leftbuttonEvent();
        });

        rightButton.onClick.AddListener(delegate ()
        {
            CloseBoard();
            rightbuttonEvent();
        });

        panel.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

    }
    /// <summary>
    /// 显示地点时间board，根据eventData判断显示按钮数量
    /// （目前都是一个按钮）
    /// </summary>
    /// <param name="data"></param>
    /// 

    public void ShowBoard(EventData data)
    {
        panel.gameObject.SetActive(true);
        Texture t = Resources.Load(data.eventName) as Texture;
        rawImage.texture = t;
        ShowWithOneButton();
    }
    
    private void ShowWithOneButton()
    {
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(false);
        leftButtonRect.anchorMax = new Vector2(0.5f, 0.5f);
        leftButtonRect.anchorMin = new Vector2(0.5f, 0.5f);
    }

    private void ShowWithTwoButton()
    {
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
        leftButtonRect.anchorMax = new Vector2(0.0f, 0.5f);
        leftButtonRect.anchorMin = new Vector2(0.0f, 0.5f);
    }

    private void CloseBoard()
    {
        panel.gameObject.SetActive(false);
    }

}
