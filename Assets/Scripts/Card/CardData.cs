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
/// gain:提升自己的功能,类型为DataReference
/// deBuffFunction:干扰对手的功能,类型为DataReference
/// </summary>
[Serializable]
public class CardData
{
    public int id;
    public string name;
    public int step;
    public string address;
    public DataReference cost;
    public DataReference gain;
    public SpecialFunction[] specialFunctions;
}

/// <summary>
/// 人物基本数据交互
/// money: 金钱
/// point: 分数
/// </summary>
[Serializable]
public class DataReference
{
    public int money;
    public int point;
}

/// <summary>
/// 特殊功能实现
/// functionmode: 功能模式, 0-改变金钱, 1-改变分数, 2-改变跳跃回合数
/// self: true-表示自己, false-表示对方
/// parameters: 功能参数, 目前只用到一个
/// </summary>
[Serializable]
public class SpecialFunction
{
    public int functionMode;
    public bool self;
    public int[] parameters;
}