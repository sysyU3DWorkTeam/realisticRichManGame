using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TicketData {
	public int id;
	public int number;
}

[Serializable]
public class TicketDataList {
	public List<TicketData> list;
}
