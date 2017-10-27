using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace vietlabs.h2
{
    internal class h2_GOIcon : h2_Icon
    {
        private static Dictionary<int, Texture2D> dict;

        public h2_GOIcon() : base("h2_go_icon")
        {
        }

        public override void RefreshSettings()
        {
            setting = h2_Setting.current.GOIcon;
            MaxWidth = 0f;
        }

        public override float Draw(Rect r, GameObject go)
        {
            if (go == null)
            {
                Debug.LogWarning("GO should not be null or empty !");
                return 0;
            }

            if (Event.current.type != EventType.repaint)
                return 0;


            if (dict == null)
                dict = new Dictionary<int, Texture2D>();

            var instId = go.GetInstanceID();
            Texture2D tex = null;

	        if (!dict.TryGetValue(instId, out tex) || (Selection.activeGameObject == go))
            {
                var so = new SerializedObject(go);
                so.Update();

                var property = so.FindProperty("m_Icon");
		        if (property != null)
		        {
		        	tex = (Texture2D) property.objectReferenceValue;
		        }
		        
		        if (!dict.ContainsKey(instId))
			    {
		        	dict.Add(instId, tex);
			    } else {
			    	dict[instId] = tex;
			    }
            }

            if (tex == null) return 0;
            var h = tex.height > 16f ? 16f : tex.height;
            var w = h*tex.width/tex.height;

            var x = r.x + r.width - MaxWidth + (MaxWidth - w)/2f;
	        GUI.DrawTexture(new Rect(x, r.y, w, h), tex);
	        
	        MaxWidth = 22f;
	        return w;
        }

        // ----------------------------- SHORTCUT HANDLER -----------------------------

        protected override void RunCommand(string cmd)
        {
            switch (cmd)
            {
                case h2_GOIconSetting.CMD_TOGGLE_ICO:
                {
                    setting.enableIcon = !setting.enableIcon;
                    EditorUtility.SetDirty(h2_Setting.current);
                    //AssetDatabase.SaveAssets();
                    h2_Utils.DelaySaveAssetDatabase();
                    return;
                }

                default:
                    Debug.Log("Unhandled command <" + cmd + ">");
                    break;
            }
        }
    }


    [Serializable]
    internal class h2_GOIconSetting : h2_FeatureSetting
    {
        internal const string CMD_TOGGLE_ICO = "toggle_ico";

        private const string TITLE = "GAMEOBJECT ICON";

        private static readonly string[] SHORTCUTS =
        {
            "Toggle GameObject icons", CMD_TOGGLE_ICO, "#%&I"
        };

        internal override void Reset()
        {
            enableIcon = false;
            enableShortcut = true;
            shortcuts = h2_Shortcut.FromStrings(SHORTCUTS);

#if H2_DEV
		Debug.Log("RESET GAMEOBJECT ICON");
#endif
        }

        internal void DrawInspector()
        {
            DrawBanner(TITLE, true, true);
        }
    }
}