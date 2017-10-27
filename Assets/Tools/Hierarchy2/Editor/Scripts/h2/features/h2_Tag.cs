using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

#if UNITY_5_5_OR_NEWER
using Profiler = UnityEngine.Profiling.Profiler;
#endif

namespace vietlabs.h2
{
    internal class h2_Tag : h2_Icon
    {
        public h2_Tag() : base("h2_tag") {}

        public override void RefreshSettings()
        {
            var t = h2_Setting.current.Tag;
            setting = t;
        }

        // ------------------------------ HIERARCHY ICON -----------------------------

        public override float Draw(Rect r, GameObject go)
        {
#if H2_DEV
		Profiler.BeginSample("h2_Tag.Draw");
#endif

            if ((go == null) || (setting == null) || !h2_Lazy.isRepaint)
            {
#if H2_DEV
			Profiler.EndSample();
#endif
                return 0;
            }

            var ss = setting as h2_TagSetting;
            var drawRect = new Rect(r.x + r.width - MaxWidth, r.y, MaxWidth, r.height);
            var ww = ss.DrawTag(drawRect, go);

            if ((MaxWidth < ww) || (ss.labelColor.maxWidth == 0))
            {
                MaxWidth = ww;
                ss.labelColor.maxWidth = ww;
            }

#if H2_DEV
		Profiler.EndSample();
#endif

            return ww;
        }

        // ----------------------------- SHORTCUT HANDLER -----------------------------

        protected override void RunCommand(string cmd)
        {
            var go = Selection.activeGameObject;
            if (go == null) return;

            switch (cmd)
            {
                case h2_TagSetting.CMD_ENABLE_TAG:
                {
                    setting.enableIcon = !setting.enableIcon;
                    EditorUtility.SetDirty(h2_Setting.current);
                    h2_Utils.DelaySaveAssetDatabase();
                    //AssetDatabase.SaveAssets();
                    return;
                }
                default:
                    Debug.Log("Unhandled command <" + cmd + ">");
                    break;
            }
        }
    }

    [Serializable]
    internal class h2_TagSetting : h2_FeatureSetting
    {
        internal const string CMD_ENABLE_TAG = "enable_tag";

        private const string TITLE = "TAG";
        private const string SHORTEN_TAG_LABEL = "Shorten Tag Name";

        private static readonly string[] SHORTCUTS =
        {
            "Show Tag in Hierarchy", CMD_ENABLE_TAG, "#%&T"
        };

        public h2_Label labelColor;

        // ------------------- INSTANCE -----------------------

        public override bool isReady
        {
            get { return labelColor != null; }
        }

        internal override void Reset()
        {
            enableIcon = false;
            enableShortcut = true;

            //shortenName = false;
            shortcuts = h2_Shortcut.FromStrings(SHORTCUTS);

            var arr = h2_Color.GetHSBColors();
            var colors = new Color[32];
            for (var i = 0; i < 32; i++)
                colors[i] = arr[i%arr.Length];

            labelColor = new h2_Label
            {
                align = 1,
                shortenName = false,
                style = h2_LabelStyle.Label_w_BgColor,
                lbColor = new Color32(0, 0, 0, 255),
                bgColor = new Color32(0, 128, 64, 128),
                colors = colors
            };

#if H2_DEV
		Debug.Log("RESET TAG");
#endif
        }

        internal float DrawTag(Rect r, GameObject go)
	    {   
            if (!h2_Lazy.isRepaint) return 0;

	        string tagName;
		    int tagIdx = h2_Change.GetTag(go, out tagName);
		    if (tagIdx <= 0) return 0;
	        
            var label = string.IsNullOrEmpty(tagName)
                ? "Undefined"
                : (labelColor.shortenName ? h2_Utils.GetShortenName(tagName) : tagName);

            return labelColor.DrawLabel(r, label, tagIdx);
        }

        internal void DrawInspector()
        {
            if (DrawBanner(TITLE, true, false))
            {
	            var tags = h2_Change.Tags;
                labelColor.DrawInspector(idx =>
                {
	                var s = tags[idx];
                    return labelColor.shortenName ? h2_Utils.GetShortenName(s) : s;
                }, tags.Length);
	            
                DrawShortcut();
            }
        }
    }
}