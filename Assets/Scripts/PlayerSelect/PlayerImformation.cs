using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// playerData
/// playerName:玩家名字
/// characterName：角色名字
/// </summary>
public class PlayerData
{
    public string playerName;
    public string characterName;
    
    public PlayerData(string playerName, string characterName)
    {
        this.playerName = playerName;
        this.characterName = characterName;
    }
}

public class PlayerImformation {
    private int playerNumber;
    private static PlayerImformation mInstance;

    public static PlayerImformation Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new PlayerImformation();
            }
            return mInstance;
        }
    }

    private List<PlayerData> mPlayerList;
    public List<PlayerData> PlayerList
    {
        get
        {
            return mPlayerList;
        }
    }



    private PlayerImformation() {
        mPlayerList = new List<PlayerData>();
    }
    public void AddPlayer(PlayerData nPlayer)
    {
        mPlayerList.Add(nPlayer);
    }
}
