﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SdbInstance<T> where T : SdbIdentifiableBase
{
    private static Dictionary<string, T> sdbDatas = new Dictionary<string, T>();

    static SdbInstance()
    {
        T[] datas = Resources.LoadAll<T>("Sdb/" + typeof(T).Name);
        for(int i = 0; i < datas.Length; ++i)
        {
            sdbDatas.Add(datas[i].Id, datas[i]);
        }
    }

    public static T Get(string id)
    {
        if(string.IsNullOrEmpty(id) == true || sdbDatas.ContainsKey(id) == false)
        {
            return null;
        }

        return sdbDatas[id];
    }

    public static List<T> GetAll()
    {
        return new List<T>(sdbDatas.Values);
    }
}

public static class SpecificSdb<T> where T : UnityEngine.ScriptableObject
{
    private static T sdbData;

    static SpecificSdb()
    {
        sdbData = Resources.Load<T>("Sdb/" + typeof(T).Name);
    }

    public static T Get()
    {
        return sdbData;
    }
}