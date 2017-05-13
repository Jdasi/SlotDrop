using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class JHelper
{
    public static List<string> GetObjectKeys(JsonData data)
    {
        List<string> keys = new List<string>();

        foreach (string key in data.Keys)
        {
            keys.Add(key);
        }

        return keys;
    }
}
