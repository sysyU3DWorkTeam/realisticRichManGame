using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StationsDataList
{
    public int stationsNum;
    public List<StationData> dataList;
}
/// <summary>
/// 地图上每个点的类
/// </summary>
public class MapStation
{
    public StationData stationData;
    public Transform transform;
    public MapStation(StationData sd, Transform t)
    {
        stationData = sd;
        transform = t;
    }
}
public class GameMap : MonoBehaviour {

    private StationsDataList mStationsList;

	public List<MapStation> mapStations;

	// Use this for initialization
	void Awake () {
        mapStations = new List<MapStation>();
        GameObject map = GameObject.Find("map");
        List<Transform> stas = new List<Transform>(map.GetComponentsInChildren<Transform>());
		stas.RemoveAt (0);
        /*for (int i = 0; i < stas.Count; i++)
        {
            Debug.Log(stas[i].name);
        }
*/
        //stas.Sort((a, b) => (a.name.CompareTo(b.name)));
        //路径
        mStationsList = LoadJson.LoadJsonFromFile<StationsDataList>("/MapStationsData.json");

        //Debug.Log(mStationsList.stationsNum);
        mStationsList.dataList.Sort((a, b) => (a.stationNo.CompareTo(b.stationNo)));
        //for (int i = 0; i < mStationsList.stationsNum && i < stas.Count; i++)
		for (int i = 0;i < 58; i++)
        {
            //Debug.Log(mStationsList.dataList[i].name + " + " + stas[i].name);
            //mapStations.Add(new MapStation(mStationsList.dataList[i], stas[i]));
			mapStations.Add(new MapStation(mStationsList.dataList[0], stas[i]));
        }
		//Debug.Log (stations.Count);
	}

    /// <summary>
    /// 返回一个MapStation
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
	public MapStation getStation(int index) {
        //Debug.Log("stat: " + mapStations.Count);
        //Debug.Log("index: " + index);
        
        index = index % mapStations.Count;
		return mapStations[index];
	}

	// Update is called once per frame
	void Update () {
		
	}
}
