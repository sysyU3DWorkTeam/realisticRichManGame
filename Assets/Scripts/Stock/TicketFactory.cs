using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class TicketFactory : MonoBehaviour {
	public ticket t;     //引用一个ticket
	public List<TicketData> data;
	bool getdata = false;
	public List<List<string>> ticketsList;//股票列表
	// Use this for initialization
	void Start () {

		//初始化，游戏开始读取文件
		//
		TicketDataList datalist = LoadFromJson ("/GameTicketData.json");
		data = datalist.list;
		//query ();
		/*List<ArrayList> temp = GetTickets ();
		for (int j = 0 ;j < temp.Count; j++) {
			
			for (int i = 0; i < temp[j].Count; i++)
				Debug.Log (temp[j][i]);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void Awake() {
		t = this.GetComponent<ticket> ();
	}
	//等待获取股票信息，等待1s
	IEnumerator waitAndGet() {
		if (t.getList().Count != 0) t.Clear ();
		t.getTicketsList ();
		yield return new WaitForSeconds(0.8f);
		ticketsList = t.getList ();
		Debug.Log (ticketsList.Count);
		/*List<ArrayList> temp = GetTickets ();
		Debug.Log (ticketsList.Count);
		for (int j = 8 ;j < temp.Count; j++) {
			
			//for (int i = 0; i < temp[j].Count; i++)
				Debug.Log (temp[j][3]);
		}*/
	}
	//买入指定id股票,返回需要的金币
	//如果操作成功就返回true
	public bool BuyTickets(int id, int num) {
		if (data [id].number >= num) {
			data [id].number -= num;
			return true;
		}
		return false;
	}
	//返回股票；列表
	//从0开始每项数据分别为
	//id, 名称，当前价格，剩余数量
	public List<ArrayList> GetTickets() {
		List<ArrayList> tickets = new List<ArrayList> ();
		for (int i = 0; i < ticketsList.Count; i++) {
			ArrayList ticket = new ArrayList();
			ticket.Add(data[i].id);
			ticket.Add(ticketsList[i][0]);
			ticket.Add(ticketsList[i][1]);
			ticket.Add(data[i].number);
			tickets.Add(ticket);
		}
		return tickets;
	}
	//卖出指定id股票,返回得到的金币
	public bool SellTickets(int id, int num) {
		data [id].number += num;
		return true;
	}
	public void OnApplicationQuit() {
		WriteToJson ("/GameTicketData.json");
	}
	//调用query 进行查询
	public void query () {
		StartCoroutine (waitAndGet());
	}

	//从json中读取数据

	public TicketDataList LoadFromJson(string path)
	{
		if (!File.Exists(Application.dataPath + path))
		{
			//如果没有存档就读初始化文件
			if (!File.Exists (Application.dataPath + "/TicketData.json"))
				return null;
			else
				path = "/TicketData.json";
		}
		StreamReader sr = new StreamReader(Application.dataPath + path);


		if (sr == null)
		{
			return null;
		}
		string json = sr.ReadToEnd();

		if (json.Length > 0)
		{
			return JsonUtility.FromJson<TicketDataList>(json);
		}

		return null;
	}
	//将股市中的股票数量写入json进行保存
	public bool WriteToJson(string path)
	{
		StreamWriter sw;
		FileInfo f = new FileInfo (Application.dataPath + path);
		if (f.Exists)
			f.Delete ();
		sw = f.CreateText ();

		TicketDataList datalist = new TicketDataList ();
		datalist.list = data;
		string json = JsonUtility.ToJson (datalist);
		sw.WriteLine (json);
		sw.Close();
		sw.Dispose ();
		return true;
	}
}
