using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ticket : MonoBehaviour {
	public string url = "http://hq.sinajs.cn/list=sz002218";
	// Use this for initialization
	void Start() {
		StartCoroutine(getInfo());
	}
	IEnumerator getInfo () {
		WWW info = new WWW (url);
		yield return info;
		TextMesh showInfo = this.transform.GetComponentInChildren<TextMesh> ();
		showInfo.text = info.text;
		Debug.Log (info.text);
	}
	public void query(string _url) {
		url = _url;
		StartCoroutine(getInfo());
	}
	// Update is called once per frame
	void Update () {
		
	}
}
