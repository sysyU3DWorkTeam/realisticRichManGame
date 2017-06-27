using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour {

	public List<Transform> stations;

	// Use this for initialization
	void Start () {
		GameObject map = GameObject.Find ("map");
		Transform[] stas = map.GetComponentsInChildren<Transform> ();
		foreach (Transform sta in stas) {
            if (sta.gameObject == map) continue;
			stations.Add (sta);
		}
		//Debug.Log (stations.Count);
	}

    //加入了取余数
	public Vector3 getStationPostion(int index) {
        index = index % stations.Count;
        Vector3 pos = new Vector3 (stations [index].position.x, stations [index].position.y + 0.5f, stations [index].position.z);
		return pos;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
