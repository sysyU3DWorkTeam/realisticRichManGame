using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicketUI : MonoBehaviour {
	public delegate void OnActionOver();
	public event OnActionOver StockOver;

	public TicketFactory tf;
	public GameObject content;
	public GameObject TicketContent;
	public GameObject P;
	public PlayerBehaviour PB;
	public Text moneyText;
	public Text success;
	public RawImage playerImage;

	public InputField code;
	public InputField number;

	private List<ArrayList> info;
	// Use this for initialization
	void Start () {
		tf = this.GetComponent<TicketFactory> ();
		//setPB (P.GetComponent<PlayerBehaviour>());
		//UpdateStock ();
	}
	
	public void UpdateStock() {
		StartCoroutine (wait());
	}
	public void setPB(PlayerBehaviour _PB) {
		//if (_PB == null)
		//	Debug.Log (12345);
		PB = _PB;
	}
	public void Buy() {
		success.text = "";
		if (code.text != "" && number.text != "") {
			int id = int.Parse (code.text);
			int num = int.Parse (number.text);
			float price = float.Parse (info [id] [2].ToString ());
			if (price * num > PB.PD.money && num <= int.Parse (info [id] [3].ToString ()))
				success.text = "购买数额过大";
			else {
				tf.BuyTickets (id, num);	
				success.text = "操作成功";
				float preBuy = PB.PD.price [id] * PB.PD.ticketNum [id];
				ArrayList theInfo = info [id];
				float readyBuy = num * price;
				int total = PB.PD.ticketNum [id] + num;
				PB.PD.price [id] = (preBuy + readyBuy)/total;
				PB.PD.ticketNum [id] = total;
				PB.PD.money -= (int)readyBuy;
				show ();
			}
		} else {
			success.text = "请输入正确的信息";
		}
	}
	public void Sell() {
		success.text = "";
		if (code.text != "" && number.text != "") {
			int id = int.Parse (code.text);
			int num = int.Parse (number.text);
			float price = float.Parse (info [id] [2].ToString ());
			if (num > PB.PD.ticketNum[id])
				success.text = "卖出数额过大";
			else {
				tf.SellTickets (id, num);	
				success.text = "操作成功";
				ArrayList theInfo = info [id];
				float readySell = num * price;
				int total = PB.PD.ticketNum [id] - num;
				PB.PD.ticketNum [id] = total;
				PB.PD.money += (int)readySell;
				show ();
			}
		} else {
			success.text = "请输入正确的信息";
		}
	}
	public void Exit() {
		StockOver ();
	}
	IEnumerator wait() {
		tf.query ();
		yield return new WaitForSeconds (1f);
		show ();
	}

	public void show() {
		info = tf.GetTickets ();
		playerImage.texture = PB.role;
		Debug.Log (PB.PD.money);
		moneyText.text = PB.PD.money.ToString ();
		if (info != null) {

			if (content.GetComponentsInChildren<Text> ().Length == 0) {
				for (int j = 0; j < info.Count; j++) {
					GameObject _content = GameObject.Instantiate (TicketContent);
					_content.transform.SetParent (content.transform);
					_content.transform.localPosition = new Vector3 (_content.transform.localPosition.x, _content.transform.localPosition.y, 0);
					_content.SetActive (true);
					Text[] texts = _content.GetComponentsInChildren<Text> ();
					//Debug.Log (info.Count);
					ArrayList theInfo = info [j];
					/*if (PB == null)
					Debug.Log (1234);
					*/
					PlayerData PD = PB.PD;
					/*
				if (PD == null)
					Debug.Log (123);
					*/
					theInfo.Add (PD.price [j]);
					theInfo.Add (PD.ticketNum [j]);
					//Debug.Log (PD.price [j]);
					if (PD.price [j] == 0)
						theInfo.Add ("0%");
					else
						theInfo.Add ((float.Parse (theInfo [2].ToString ()) / PD.price [j]*100-100f).ToString () + "%");
					//theInfo.Add ((theInfo[2]/PB.PD.price[j]*100-100).ToString()+'%');
					for (int i = 0; i < theInfo.Count; i++) {
						texts [i].text = theInfo [i].ToString ();
					}

				}
			} else {
				//int id = int.Parse (code.text);
				Text[] texts = content.GetComponentsInChildren<Text> ();
				for (int j = 0; j < info.Count; j++) {
					ArrayList theInfo = info [j];
					PlayerData PD = PB.PD;
					if (PD.ticketNum [j] == 0)
						PD.price [j] = 0;
					theInfo.Add (PD.price [j]);
					theInfo.Add (PD.ticketNum [j]);
					if (PD.price [j] == 0)
						theInfo.Add ("0%");
					else
						theInfo.Add ((float.Parse (theInfo [2].ToString ()) / PD.price [j]*100-100f).ToString () + "%");
					for (int i = 0; i < theInfo.Count; i++) {
						texts [i+j*theInfo.Count].text = theInfo [i].ToString ();
					}
				}
			}
		}
	}
}
