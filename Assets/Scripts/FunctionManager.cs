using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionManager{
    /// <summary>
    /// function模板
    /// player - 玩家
    /// parameterList - 参数列表
    /// </summary>
    /// <param name="player"></param>
    /// <param name="parameterList"></param>
    public delegate void FunctionDelegate(PlayerBehaviour player, int[] parameterList);

    private List<FunctionDelegate> mFunctionList;

    public FunctionManager()
    {
        mFunctionList = new List<FunctionDelegate>();
    }
    public FunctionManager(List<FunctionDelegate> nList)
    {
        mFunctionList = nList;
    }
    /// <summary>
    /// 添加function
    /// </summary>
    /// <param name="function"></param>
    public void AddFunction(FunctionDelegate function)
    {
        mFunctionList.Add(function);
    }
    /// <summary>
    /// 删除function
    /// </summary>
    /// <param name="function"></param>
    public void DeleteFunction(FunctionDelegate function)
    {
        mFunctionList.Remove(function);
    }

    public FunctionDelegate GetFunction(int mode)
    {
        return mFunctionList[mode];
    }

    public static List<int> GetParamaters(string parameters)
    {
        List<int> parameterList = new List<int>();
        string[] p = parameters.Split('|');
        for (int i = 0; i < p.Length; i++)
        {
            parameterList.Add(int.Parse(p[i]));
        }
        return parameterList;
    }
    
    
}
