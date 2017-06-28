using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LoadJson : MonoBehaviour
{

    public static T LoadJsonFromFile<T>(string path)
    {
        if (!File.Exists(Application.dataPath + path))
        {
            return default(T);
        }

        StreamReader sr = new StreamReader(Application.dataPath + path);


        if (sr == null)
        {
            return default(T);
        }
        string json = sr.ReadToEnd();

        if (json.Length > 0)
        {
            return JsonUtility.FromJson<T>(json);
        }

        return default(T);
    }

}
