using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ticket : MonoBehaviour {
	private List<List<string>> ticketsList = new List<List<string>> ();     //股票列表
	public int num = 11;  //获取股票的个数，默认10个
	//API的url前缀
	public string baseUrl = "http://hq.sinajs.cn/list=sh";
	// Use this for initialization
	void Start() {
		//StartCoroutine(getInfo("http://hq.sinajs.cn/list=sh601006"));
	}
	//筛选有效信息
	IEnumerator getInfo (string url) {
		List<string> infoList = new List<string> ();     //保存某一个股票信息
		WWW info = new WWW (url);
		yield return info;
		string[] str = info.text.Split (',');
		if (str.Length != 1) {
			string[] s = str[0].Split('"');
			infoList.Add (s[1]);  //股票名字
			infoList.Add (str [3]);   //当前价格
			ticketsList.Add (infoList);
		} else
			num++;
	}
	/*
	    public void query() {
		StartCoroutine(getTicketsList());
	}
	*/
	public void getTicketsList() {
		num = 19;
	    for (int i = 601000; i < 601000+num; i++) {
		    string queryStr = i.ToString ();
			string url = baseUrl + queryStr;
			StartCoroutine(getInfo(url));
	    }

    }
	public void Clear() {
		Debug.Log ("clear");
		ticketsList.RemoveRange (0, 15);
	}
	public List<List<string>> getList() {
		return ticketsList;
	}
	// Update is called once per frame
	void Update () {

	}
}