using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using System;
using System.Collections.Generic;

[InitializeOnLoad] public static class h2_Change
{	
	static h2_Change()
	{
		Undo.postprocessModifications -= OnChange;
		Undo.postprocessModifications += OnChange;
	}
	
	static UndoPropertyModification[] OnChange(UndoPropertyModification[] props)
	{
		foreach (var item in props)
		{
			#if UNITY_5_3_OR_NEWER
			var p = item.currentValue;
			#else
			var p = item.propertyModification;
			#endif
			
			var pType = p.target.GetType().FullName;
			
			switch (pType)
			{
				case "UnityEditor.TagManager"	: ProcessTagLayer(p); continue;
				//case "UnityEngine.Transform"	: ProcessTransform(p); continue;
				
				default: 
					#if H2_DEV
					Debug.LogWarning("Unsupported change : " + pType + "\n" + p.target + "." + p.propertyPath + ":" + p.value);
					#endif
				break;
			}
		}
		return props;
	}
	
	static void ProcessTagLayer(PropertyModification p)
	{
		var paths	= p.propertyPath.Split('.');
		var pName	= paths[0];
		
		#if H2_DEV
		Debug.Log(p.target + "." + p.propertyPath + ":" + p.value);
		#endif
		
		if (pName == "tags")
		{
			tags = null;
			return;
		}
		
		// Change occurs at index
		if (pName == "layers") 
		{
			var p2		= paths[2];
			if (!p2.StartsWith("data["))
			{
				return;	
			}
			
			var pIndex = int.Parse(p2.Substring(5, p2.Length-6));
			layers[pIndex] = null;
			return;
		}
	}
	
	// ------------------------------------- LAYER CHANGED -------------------------------------
	
	static string[] layers = new string[32];
	public static string GetLayerName(int l)
	{
		var v = layers[l];
		if (string.IsNullOrEmpty(v))
		{
			v = LayerMask.LayerToName(l);
			if (string.IsNullOrEmpty(v)) v = "Layer " + l;
			layers[l] = v;
		}
		return v;
	}
	
	public static string[] Layers
	{
		get 
		{
			for (var i = 0;i < layers.Length; i++)
			{
				if (string.IsNullOrEmpty(layers[i])) GetLayerName(i);
			}
			return layers;
		}
	}
	
	// ------------------------------------- TAG CHANGED -------------------------------------
	
	static string[] tags;
	public static int GetTag(GameObject go, out string tagName)
	{
		tagName = "Untagged";
		if (go == null) return -1;
		
		if (tags == null) tags = InternalEditorUtility.tags;
		
		for (var i = 0; i < tags.Length; i++)
		{
			tagName = tags[i];
			if (go.CompareTag(tagName)) return i;
		}
		
		return -1;
	}
	
	public static string[] Tags
	{
		get
		{
			if (tags == null) tags = InternalEditorUtility.tags;
			return tags;	
		}
	}
	
	// ------------------------------------- TAG CHANGED -------------------------------------
	
	//public static Action<Transform> OnParentChanged;
	
	//static void ProcessTransform(PropertyModification p)
	//{
		
	//	if (p.propertyPath == "m_RootOrder")
	//	{
	//		Debug.Log("Parent changed : " + p.target);
	//		if (OnParentChanged != null) OnParentChanged((Transform)p.target);
	//		return;
	//	}
		
	//	Debug.LogWarning("change : " + p.target + "." + p.propertyPath + ":" + p.value);
	//}
	
}