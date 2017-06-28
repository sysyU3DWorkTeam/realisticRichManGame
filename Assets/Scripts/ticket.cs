using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ticket : MonoBehaviour {
	private List<List<string>> ticketsList = new List<List<string>> ();     //股票列表

	//API的url前缀
	public string baseUrl = "http://hq.sinajs.cn/list=sh";
	// Use this for initialization
	void Start() {
		query ("http://hq.sinajs.cn/list=sh601006");
	}
	//筛选有效信息
	IEnumerator getInfo (string url) {
		List<string> infoList = new List<string> ();     //保存某一个股票信息
		WWW info = new WWW (url);
		yield return info;
		string[] str = info.text.Split (',');
		infoList.Add (str[0]);  //股票名字
		infoList.Add(str[3]);   //当前价格
		ticketsList.Add(infoList);
	}
	public void query(string _url) {
		StartCoroutine(getInfo(_url));
	}
	void getTicketsList() {
		
	    for (int i = 0; i < 10; i++) {
		    string queryStr = "60100" + i.ToString ();
			string url = baseUrl + queryStr;
			query (url);
	    }

    }
	public List<List<string>> getList() {
		return ticketsList;
	}
	// Update is called once per frame
	void Update () {

	}
}