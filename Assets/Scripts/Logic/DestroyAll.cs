using UnityEngine;
using System.Collections;

public class DestroyAll : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Zombie"))
        {
            return;
        }

        Destroy(other.gameObject);
    }
}