using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 卡牌数据
/// 
/// id:主元
/// name:名字
/// step:步数
/// cost:代价,类型为DataReference
/// adress:使用地点
/// buffFunction:提升自己的功能,类型为DataReference
/// deBuffFunction:干扰对手的功能,类型为DataReference
/// </summary>
[Serializable]
public class CardData
{
    public int id;
    public string name;
    public int step;
    public DataReference cost;
    public string address;
    public DataReference buffFunction;
    public DataReference deBuffFunction;
}

/// <summary>
/// 卡牌与人物数据交互的数据参考
/// money: 金钱
/// point: 分数
/// roundSkip: 回合跳过
/// </summary>
[Serializable]
public class DataReference
{
    public int money;
    public int roundSkip;
    public int point;
}