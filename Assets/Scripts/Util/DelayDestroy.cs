using UnityEngine;
using System.Collections;

public class DelayDestroy : MonoBehaviour
{
    [SerializeField]
    private float _destroyTime;
    private void Awake()
    {
        Invoke("Destroy", _destroyTime);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
