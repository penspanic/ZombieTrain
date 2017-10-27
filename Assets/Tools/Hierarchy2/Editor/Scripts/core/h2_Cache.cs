using System;
using System.Collections.Generic;
using UnityEngine;

public class h2_CacheItem<T>
{
    public float stamp;
    public T value;
}

public class h2_Cache<T1, T2>
{
    internal Dictionary<T1, h2_CacheItem<T2>> cache = new Dictionary<T1, h2_CacheItem<T2>>();

    public T2 Get(T1 id, float cacheTime, Func<T1, T2> refresh)
    {
        h2_CacheItem<T2> result;
        var realTime = Time.realtimeSinceStartup;

        if (cache.TryGetValue(id, out result))
        {
            if (realTime - result.stamp < cacheTime) return result.value;
        }
        else
        {
            result = new h2_CacheItem<T2>();
            cache.Add(id, result);
        }

        result.value = refresh(id);
        result.stamp = realTime;
        return result.value;
    }

    public void Clear()
    {
        cache.Clear();
    }
}