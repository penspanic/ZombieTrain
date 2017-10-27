using UnityEngine;
using System.Collections;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                if(GameObject.FindObjectOfType<T>() != null)
                {
                    _instance = GameObject.FindObjectOfType<T>();
                    return _instance;
                }
                _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        Debug.Log(typeof(T).Name + " Created.");
    }
}

public interface IAccessValidatable
{
    bool ValidateAccess();
}

public class Singleton<T> where T : IAccessValidatable, new()
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new T();
                if(_instance.ValidateAccess() == false)
                {
                    throw new System.Exception("Access validation failed, Type : " + typeof(T).Name);
                }
            }

            return _instance;
        }
    }
}