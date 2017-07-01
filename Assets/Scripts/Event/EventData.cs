using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EventData {
    public int eventNo;
    public string eventName;
    public string position;
    public DataReference eventData;
    public SpecialFunction[] specialFunctions;
}
