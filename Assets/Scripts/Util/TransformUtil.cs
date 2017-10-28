using UnityEngine;
using System.Collections;

public static class TransformUtil
{
    //Breadth-first search
    public static Transform FindRecursive(this Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if(result != null)
            return result;
        foreach(Transform child in aParent)
        {
            result = child.FindRecursive(aName);
            if(result != null)
                return result;
        }
        return null;
    }
}
