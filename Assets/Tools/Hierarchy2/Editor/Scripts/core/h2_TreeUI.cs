#define DEV_MODE

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class h2_TreeUI
{
    private readonly float itemH = 16f;
    public float maxH = 0;
    private Vector2 position;
    private Rect visibleRect;

    public void Draw(h2_TreeUIItem root)
    {
        var evtType = Event.current.type;
        var r = GUILayoutUtility.GetRect(1f, Screen.width, 16f, Screen.height);

        if (evtType != EventType.layout) visibleRect = r;

        var n = root.GetNVisible(Mathf.Max(root._treeStamp, 1));
        var contentRect = new Rect(0f, 0f, 1f, n*itemH);
        var nVisible = Mathf.RoundToInt(visibleRect.height/itemH) + 1;
        var min = Mathf.Max(0, Mathf.FloorToInt(position.y/itemH));
        var max = Mathf.Min(min + nVisible, n);
        var noScroll = contentRect.height < visibleRect.height;

        //Debug.Log("Drawing :: " + min + ":" + max + ":" + n);

        if (noScroll) position = Vector2.zero;

        position = GUI.BeginScrollView(visibleRect, position, contentRect);
        {
            var rect = new Rect(0, 0, r.width - (noScroll ? 4f : 16f), itemH);
            var current = 0;
            root.Draw(min, max, ref rect, ref current);
        }

        GUI.EndScrollView();
    }

    public void Draw(int total, Action<int, Rect> itemDrawer)
    {
        var evtType = Event.current.type;

        var maxHeight = Mathf.Min(maxH == 0 ? Screen.height : maxH, total*itemH);
        var r = GUILayoutUtility.GetRect(1f, Screen.width, 16f, maxHeight);

        if (evtType != EventType.layout)
            visibleRect = r;

        var contentRect = new Rect(0f, 0f, 1f, total*itemH);
        var nVisible = Mathf.RoundToInt(visibleRect.height/itemH) + 1;
        var min = Mathf.Max(0, Mathf.FloorToInt(position.y/itemH));
        var max = Mathf.Min(min + nVisible, total);
        var noScroll = contentRect.height < visibleRect.height;

        if (noScroll) position = Vector2.zero;

        position = GUI.BeginScrollView(visibleRect, position, contentRect);
        {
            var rect = new Rect(0, 0, r.width - (noScroll ? 4f : 16f), itemH);
            for (var i = min; i < max; i++)
            {
                rect.y = i*itemH;
                itemDrawer(i, rect);
            }
        }

        GUI.EndScrollView();
    }

    public void Draw<T>(List<T> objList, Action<int, Rect, T> itemDrawer)
    {
        Draw(objList.Count, (i, rect) => { itemDrawer(i, rect, objList[i]); });
    }
}

public class h2_TreeUIItem
{
    private const float arrowWidth = 14f;

    // -------------------------- DRAWER ----------------------------
    private static GUIStyle foldStyle;

    // -------------------------- PARENT / CHILDREN ----------------------------

    protected bool _expand = true;

    protected int _itemHeight = 16;
    protected int _nFakeSize;
    protected int _nVisible;
    internal int _treeStamp;
    internal List<h2_TreeUIItem> children = new List<h2_TreeUIItem>();

    // -------------------------- PARENT / CHILDREN ----------------------------

    private h2_TreeUIItem parent;

    public virtual int GetNVisible(int stamp)
    {
        if (!_expand) return 1 + _nFakeSize;
        if (_treeStamp == stamp) return _nVisible + _nFakeSize;

        RefreshVisibleCount(stamp);
        return _nVisible + _nFakeSize;
    }

    public void ToggleExpand()
    {
        var delta = _nVisible - 1;
        _expand = !_expand;
        SetDeltaVisible(_expand ? delta : -delta, _treeStamp);
    }

    private void RefreshVisibleCount(int stamp)
    {
        _treeStamp = stamp;
        _nVisible = 1;

        for (var i = 0; i < children.Count; i++)
        {
            var c = children[i];
            _nVisible += c.GetNVisible(stamp);
        }
    }

    private void SetDeltaVisible(int delta, int stamp)
    {
        _treeStamp = stamp;
        _nVisible += delta;

        if (parent != null) parent.SetDeltaVisible(delta, stamp);
        else GetNVisible(++stamp);
    }

    public h2_TreeUIItem AddChild(h2_TreeUIItem child)
    {
#if DEV_MODE
        {
            if (!IsValidChild(child)) return this;

            if (child.parent == this)
            {
                Debug.LogWarning("Child.parent already == this");
                return this;
            }
            if (children.Contains(child))
            {
                Debug.LogWarning("Something broken, child already in this.children list <" + child +
                                 "> but its parent not set to this");
                return this;
            }
        }
#endif

        if (child.parent != null) child.parent.RemoveChild(child);
        child.parent = this;
        children.Add(child);

        var delta = child.GetNVisible(_treeStamp);
        SetDeltaVisible(delta, _treeStamp);
        return this;
    }

    public h2_TreeUIItem RemoveChild(h2_TreeUIItem child)
    {
#if DEV_MODE
        {
            if (!IsValidChild(child)) return this;

            if (child.parent != this)
            {
                Debug.LogWarning("child.parent != this, can not remove");
                return this;
            }

            if (!children.Contains(child))
            {
                Debug.LogWarning("Something broken, child.parent == this but this.children does not contains child");
                return this;
            }
        }
#endif


        child.parent = null;
        children.Remove(child);

        //Recursive update nVIsible items up the tree
        var delta = child.GetNVisible(_treeStamp);
        SetDeltaVisible(-delta, _treeStamp);

        return this;
    }

#if DEV_MODE
    private bool IsValidChild(h2_TreeUIItem child)
    {
        if (child == null)
        {
            Debug.LogWarning("Child should not be null <" + child + ">");
            return false;
        }
        //if (child.target == null){
        //	Debug.LogWarning("Child's target should not be null <" + child + ">");
        //	return false;
        //}
        return true;
    }
#endif

    public virtual void Draw(int from, int to, ref Rect r, ref int current)
    {
        if (current >= to)
        {
            Debug.LogWarning("Out of view " + current + ":" + from + ":" + to + ":" + this);
            return; //finish drawing
        }

        var n = _nFakeSize + 1;
        var h = _itemHeight*n;

        if (current + n >= from)
        {
            //partially / fully visible ? just draw

            if (children.Count > 0)
            {
                // Draw expand / collapse arrow
                var arrowRect = r;
                arrowRect.width = arrowWidth;
                EditorGUI.BeginChangeCheck();

                if (foldStyle == null) foldStyle = "IN Foldout";
                GUI.Toggle(arrowRect, _expand, GUIContent.none, foldStyle);

                if (EditorGUI.EndChangeCheck()) ToggleExpand();
            }

            var drawRect = r;
            drawRect.x += arrowWidth;
            drawRect.width -= arrowWidth;
            drawRect.height = h;
            Draw(drawRect);
        }

        current += n;
        r.y += h;

        if (!_expand || (current >= to)) return;

        r.x += arrowWidth;
        r.width -= arrowWidth;

        for (var i = 0; i < children.Count; i++)
        {
            children[i].Draw(from, to, ref r, ref current);
            if (current >= to) return;
        }

        r.x -= arrowWidth;
        r.width += arrowWidth;
    }

    protected virtual void Draw(Rect r)
    {
    }
}

public class h2_TIObject<T> : h2_TreeUIItem where T : Object
{
    protected string displayLabel;
    public T target;

    public h2_TIObject(T target)
    {
        if (target == null) return;

        this.target = target;
        Scan4Children();
    }

    protected virtual void Scan4Children()
    {
    }

    protected override void Draw(Rect r)
    {
        if (target == null) return;

        if (string.IsNullOrEmpty(displayLabel))
            displayLabel = target.name + " (" + target.GetType().Name + ")";

        GUI.Label(r, displayLabel);
        r.xMin += r.width - 40f;
        GUI.Label(r, "" + children.Count);
    }
}