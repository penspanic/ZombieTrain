using UnityEditor;
using UnityEngine;
using vietlabs.h2;

public class h2_HelperPanel : EditorWindow
{
    private static h2_HelperPanel window;
    //static bool lockSelection;
    internal static bool showSelfReference;
    private h2_SerializedGO s;

    private int tab;

    // -------------------------- SERIALIZED -------------------------

    private bool willRefresh;

    [MenuItem("Window/Hierarchy2 %F12", false)]
    public static void Initialize()
    {
        window = GetWindow<h2_HelperPanel>();
	    window.Show();
	    
	    window.tab = 0;
        h2_Unity.SetWindowTitle(window, "Hierarchy2");
        h2_HierarchyRef.Api.Refresh();
    }

    private void OnEnable()
    {
	    if (window != null)
	    {
	    	h2_Unity.SetWindowTitle(window, "Hierarchy2");
	    }
    }

    private void OnGUI()
    {
        tab = GUILayout.Toolbar(tab, new[]
        {
            "Refrences",
            "Filter",
            "Selection"
        });

        switch (tab)
        {
            case 0:
                DrawSerialized();
                break;
            case 1:
                DrawFilter();
                break;
            case 2:
                DrawSelection();
                break;
        }
    }

    private void OnSelectionChange()
    {
        if (!h2_HierarchyRef.Api.QReady) return;

        var go = Selection.activeGameObject;
        //ResetReferenceTarget(go);
        ResetSerializeTarget(go);
        Repaint();
    }

    private void ResetSerializeTarget(GameObject go)
    {
        if (go == null) return;
        s = new h2_SerializedGO(go);
    }

    private void DrawSerialized()
    {
        if (!h2_HierarchyRef.Api.QReady)
        {
            if (h2_HierarchyRef.Api.QTotal > 0)
            {
                var r = GUILayoutUtility.GetRect(1f, Screen.width, 16f, 16f);
                EditorGUI.ProgressBar(r, h2_HierarchyRef.Api.QProgress, "Refreshing ... ");
                willRefresh = true;
                Repaint();
                return;
            }
        }
        else
        {
            if (willRefresh) OnSelectionChange();

            if (s != null) s.Draw();
            GUILayout.FlexibleSpace();
        }

        if (GUILayout.Button("Refresh"))
            h2_HierarchyRef.Api.Refresh();
    }

    // -------------------------- SERIALIZED -------------------------

    private void DrawFilter()
    {
    }

    private void DrawSelection()
    {
    }
}