using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventDataList
{
    //public string cardListName;
    //public string version;
    public int eventCount;
    public List<EventData> eventList;
}
public class EventController : MonoBehaviour {

    private EventDataList mEventList;
    
    private void Awake()
    {
        mEventList = LoadJson.LoadJsonFromFile<EventDataList>("/EventDataList.json");
        mEventList.eventList.Sort((a, b) => (a.eventNo.CompareTo(b.eventNo)));
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// 获取当前地点的事件，返回的是EventData
    /// </summary>
    /// <param name="station"></param>
    /// <returns></returns>
    public EventData getEvent(MapStation station)
    {
        List<EventData> stationEvents = new List<EventData>();
        for (int i = 0; i < mEventList.eventCount; i++)
        {
            if (mEventList.eventList[i].position.Equals(station.stationData.name))
            {
                stationEvents.Add(mEventList.eventList[i]);
            }
        }
        if (stationEvents.Count > 0)
        {
            int index = Random.Range(0, stationEvents.Count - 1);
            return stationEvents[index];
        }
        else
        {
            return null;
        }
    }
}
