using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using vietlabs.h2;

public class h2_HierarchyRef : h2_Ref<int, R_HInfo>
{
    // --------------------------- STATIC ---------------------

    private static h2_HierarchyRef _api;

    internal static h2_HierarchyRef Api
    {
        get { return _api ?? (_api = new h2_HierarchyRef()); }
    }

    internal override int Object2ID(Object obj)
    {
        return obj.GetInstanceID();
    }

    internal override Object ID2Object(int id)
    {
        return EditorUtility.InstanceIDToObject(id);
    }

    // --------------------------- INTERNAL ---------------------

    //string currentScene;

    internal h2_HierarchyRef Refresh()
    {
        QTotal = 0;
        rMap.Clear();
        refreshQueue.Clear();

        var allObjects = (GameObject[]) Resources.FindObjectsOfTypeAll(typeof(GameObject));
        for (var i = 0; i < allObjects.Length; i++)
        {
            var obj = allObjects[i];
            New(obj.GetInstanceID(), obj);
        }

        //currentScene = h2_Unity.ActiveScene;
        //EditorApplication.hierarchyWindowChanged -= hierarchyWindowChanged;
        //EditorApplication.hierarchyWindowChanged += hierarchyWindowChanged;

        EditorApplication.update -= QCheck;
        EditorApplication.update += QCheck;
        return this;
    }

    //		Api.Refresh();
    //		currentScene = h2_Unity.ActiveScene;
    //	if (currentScene != h2_Unity.ActiveScene) {
    //{

    //void hierarchyWindowChanged()
    //	}
    //}
}

public enum h3_HRType
{
    GameObject,
    Component,
    Asset
}

public class R_HInfo : h2_RefInfo<int>
{
    public string displayName;
    public h3_HRType type;

    internal override void Reset(int id, Object obj)
    {
        base.Reset(id, obj);
        type = obj is GameObject ? h3_HRType.GameObject : h3_HRType.Component;
    }

    internal override void Scan()
    {
        if (target == null) return;

        if (type == h3_HRType.GameObject)
        {
            displayName = target.name;
            ScanGameObject(target as GameObject);
        }
        else
        {
            displayName = target.GetType().FullName;
            ScanComponent(target as Component);
        }
    }

    private void ScanGameObject(GameObject go)
    {
        displayName = h2_Utils.GetHierarchyName(go.transform);

        var compList = go.GetComponents<Component>();
        for (var j = 0; j < compList.Length; j++)
        {
            var c = compList[j];
            if (c == null) continue;
	        refTo.Add(c.GetInstanceID());
        }
    }

    private void ScanComponent(Component c)
    {
        if (c == null) return;

        displayName = c.GetType().FullName;

        var props = h2_Utils.GetSerializedProperties(c, true);
        for (var k = 0; k < props.Count; k++)
        {
            if (props[k].propertyType != SerializedPropertyType.ObjectReference) continue;

	        var refObj = props[k].objectReferenceValue;
	        
            if (refObj == null) continue;
            if (refObj == c.gameObject) continue;

            var propId = refObj.GetInstanceID();
            if (propId == id) continue; //self-reference
            if (!refTo.Contains(propId)) refTo.Add(propId); //add-reference
        }
    }
}

public class h2_Serialized<T> where T : Object
{
    private static bool tried2Find;
    private static Texture2D iconShortcut;
    private static Texture2D iconBranch;
    //static Texture2D iconRecursive;

    private static readonly Color BLUE = new Color(0, 1f, 1f, 0.8f);
    private static readonly Color RED = new Color(1f, 0f, 0f, 0.5f);
    public bool expand;
    public string label;
    public List<SerializedProperty> refs;

    // ------------------------------------

    public T target;

    private readonly int total;

    public h2_TreeUI tree;
    public List<Object> usedBy;

    public h2_Serialized(T target)
    {
        this.target = target;
        expand = true;

        if (target is GameObject)
        {
            label = target.name;
        }
        else
        {
            label = target.GetType().ToString();
            label = label.Substring(label.LastIndexOf(".") + 1);
        }

        refs = h2_Utils.GetSerializedProperties(target, true);
        usedBy = h2_HierarchyRef.Api.GetUsageObjects(target.GetInstanceID());

        total = refs.Count + usedBy.Count;

        if (total > 0) tree = new h2_TreeUI();
    }

    private static void FindIcons()
    {
        if (tried2Find) return;
        tried2Find = true;
        iconShortcut = h2_Asset.FindAsset<Texture2D>("shortcuts.png");
        iconBranch = h2_Asset.FindAsset<Texture2D>("branch.png");
        //iconRecursive	= h2_Asset.FindAsset<Texture2D>("self.png");
    }

    public void Draw()
    {
        if (tree == null) return;
        if (!tried2Find) FindIcons();

        expand = h2_GUI.DrawBanner(expand, label, 0f, 0f, 14f);
        var rr = GUILayoutUtility.GetLastRect();

        // Draw Target Icon
        var tIcon = AssetPreview.GetMiniThumbnail(target);
        if (tIcon != null) GUI.DrawTexture(new Rect(4f, rr.y, 16, 16), tIcon);

        var ww = 48f;
        rr.y += 1;
        rr.xMax -= ww + 8f;
        h2_GUI.BigLabelIcon(new Rect(rr.xMax, rr.y, ww, ww), usedBy.Count.ToString(), iconShortcut);

        rr.xMax -= ww;
        h2_GUI.BigLabelIcon(new Rect(rr.xMax, rr.y, ww, ww), refs.Count.ToString(), iconBranch);

        if (expand)
        {
            GUILayout.Space(2f);
            tree.Draw(total + 1, DrawItem);
        }
        GUILayout.Space(2f);
    }


    public void DrawItem(int idx, Rect r)
    {
        r.xMin += 24f;

        if (idx == refs.Count) return;

        if (idx < refs.Count)
        {
            var v = refs[idx].objectReferenceValue;
            var c = GUI.backgroundColor;
            if (v == null)
                GUI.backgroundColor = RED;

            EditorGUI.PropertyField(r, refs[idx]);
            GUI.backgroundColor = c;
        }
        else
        {
            r.xMin += 24f;
            EditorGUI.ObjectField(r, usedBy[idx - refs.Count - 1], typeof(Object), true);

            if (iconShortcut != null)
            {
                r.x -= 18f;
                r.width = 16f;
                h2_GUI.TextureColor(r, iconShortcut, BLUE);
            }
        }
    }
}

public class h2_SerializedGO
{
    public List<h2_Serialized<Component>> components;
    public h2_Serialized<GameObject> gameObject;
    public GameObject go;

    public h2_SerializedGO(GameObject go)
    {
        this.go = go;

        gameObject = new h2_Serialized<GameObject>(go);

        var list = go.GetComponents<Component>();
        components = new List<h2_Serialized<Component>>();

        for (var i = 0; i < list.Length; i++)
        {
            if (list[i] == null) continue;
            components.Add(new h2_Serialized<Component>(list[i]));
        }
    }

    public void Draw()
    {
        gameObject.Draw();

	    for (var i = 0; i < components.Count; i++)
	    {
	    	components[i].Draw();
	    }
    }
}