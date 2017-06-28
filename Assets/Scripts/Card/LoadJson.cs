using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LoadJson : MonoBehaviour {

    public static CardDataList LoadCardListJsonFromFile(string path)
    {
        if (!File.Exists(Application.dataPath + path))
        {
            return null;
        }

        StreamReader sr = new StreamReader(Application.dataPath + path);
        
    
        if (sr == null)
        {
            return null;
        }
        string json = sr.ReadToEnd();

        if (json.Length > 0)
        {
            return JsonUtility.FromJson<CardDataList>(json);
        }

        return null;
    }
    
}
