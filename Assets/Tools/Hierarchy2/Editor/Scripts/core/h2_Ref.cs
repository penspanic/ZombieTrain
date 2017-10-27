using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class h2_Ref<T1, T> where T : h2_RefInfo<T1>, new()
{
    private const float FRAME_TIME = 0.01f;

    // --------------- QUEUE ----------------------

    internal int QTotal;
    internal List<T> refreshQueue = new List<T>();

    internal Dictionary<T1, T> rMap = new Dictionary<T1, T>();
    internal List<T> rMissing = new List<T>();

    internal List<T> MissingList
    {
        get
        {
            if (!QReady || (rMissing != null)) return rMissing;
            return rMissing = rMap.Values.ToList().FindAll(item => item.state == h2_RefState.MISSING);
        }
    }

    internal bool QReady
    {
        get { return (QTotal > 0) && (refreshQueue.Count == 0); }
    }

    internal int QCurrent
    {
        get { return QTotal - refreshQueue.Count; }
    }

    internal float QProgress
    {
        get { return QTotal == 0 ? 0 : (QTotal - refreshQueue.Count)/(float) QTotal; }
    }

    internal T Get(T1 id, bool autoNew, bool checkQueue = false)
    {
        T result;
        if (rMap.TryGetValue(id, out result)) return result;

        if (!autoNew) return null;

        result = New(id, ID2Object(id));

        if (checkQueue)
        {
            EditorApplication.update -= QCheck;
            EditorApplication.update += QCheck;
        }

        return result;
    }

    internal T New(T1 id, Object obj)
    {
        var result = new T();
        result.Reset(id, obj);
        rMap.Add(id, result);

        QTotal++;
        refreshQueue.Add(result);
        return result;
    }

    // --------------- CONVERT ----------------------

    internal virtual T1 Object2ID(Object obj)
    {
        return default(T1);
    }

    internal virtual Object ID2Object(T1 id)
    {
        return null;
    }

    // --------------- REFERENCES ----------------------

    internal List<T1> GetDependency(T1 id)
    {
        var result = new List<T1>();

        var item = Get(id, false);
        if (item == null) return result;

        result.AddRange(item.refTo);
        return result;
    }

    internal List<T1> GetUsage(T1 id)
    {
        var result = new List<T1>();

        h2_RefInfo<T1> item = Get(id, false);
        if (item == null) return result;

        result.AddRange(item.refBy);
        return result;
    }

    internal List<Object> GetUsageObjects(T1 id)
    {
        return GetUsage(id).Select(pid => ID2Object(pid)).ToList();
    }


    internal void GetDependency(T item, List<T1> result, Dictionary<T1, T> pMap)
    {
        AppendResult(item, item.refTo, result, pMap,
            GetDependency
        );
    }

    internal void GetUsage(T item, List<T1> result, Dictionary<T1, T> pMap)
    {
        AppendResult(item, item.refBy, result, pMap,
            GetUsage
        );
    }

    internal void AppendResult(T item, List<T1> arr, List<T1> result, Dictionary<T1, T> pMap,
        Action<T, List<T1>, Dictionary<T1, T>> getRef)
    {
        var from = result.Count;
        var count = 0;

        // Prioritize direct children over grand children :
        // Add direct children to pMap first
        // Order in result array is not important
        for (var i = 0; i < arr.Count; i++)
        {
            T c;
            if (pMap.TryGetValue(arr[i], out c)) continue;

            pMap.Add(arr[i], item);
            result.Add(arr[i]);
            count++;
        }

        // Deep scan for reference
        for (var i = 0; i < count; i++) getRef(Get(result[from + i], false), result, pMap);
    }

    protected void QCheck()
    {
        if (refreshQueue.Count == 0)
        {
            EditorApplication.update -= QCheck;
            return;
        }

        var t = Time.realtimeSinceStartup + FRAME_TIME;

        do
        {
            var last = refreshQueue.Count - 1;
            if (last < 0) break;

            var item = refreshQueue[last];
            refreshQueue.RemoveAt(last);
            QProcess(item);
        } while (Time.realtimeSinceStartup < t);
    }

    protected virtual void QProcess(T item)
    {
        item.Scan();

        for (var i = 0; i < item.refTo.Count; i++)
        {
            h2_RefInfo<T1> r = Get(item.refTo[i], true);
            r.refBy.Add(item.id);
        }
    }
}

public enum h2_RefState
{
    MISSING,
    NEW,
    READY,
    DELETED
}

public class h2_RefInfo<T1>
{
    public T1 id;
    public List<T1> refBy = new List<T1>();

    public List<T1> refTo = new List<T1>();
    public h2_RefState state;
    public Object target;

    internal virtual void Reset(T1 id, Object obj)
    {
        this.id = id;
        target = obj;
        state = obj != null ? h2_RefState.NEW : h2_RefState.MISSING;
        refTo.Clear();
    }

    internal virtual void Scan()
    {
        state = h2_RefState.READY;
    }
}