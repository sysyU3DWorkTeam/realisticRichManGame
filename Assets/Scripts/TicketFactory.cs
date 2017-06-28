using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketFactory : MonoBehaviour {
	public ticket t;

	List<string> infoList = new List<string> ();     //保存某一个股票信息
	List<List<string>> ticketsList;//股票列表
	// Use this for initialization
	void Start () {
		t = this.GetComponent<ticket> ();
		ticketsList = t.getList ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void Awake() {
	    
	}
	//买入指定id股票,返回需要的金币
	public float BuyTickets(int id, int num) {
		float money = 0;
		return money;
	}
	//卖出指定id股票,返回得到的金币
	public float SellTickets(int id, int num) {
		float money = 0;
		return money;
	}
	public void OnApplicationQuit() {
	    
	}
}
