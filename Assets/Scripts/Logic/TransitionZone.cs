using UnityEngine;
using System.Collections;

namespace Logic
{
    public class TransitionZone : MonoBehaviour
    {
        [SerializeField]
        private GameObject _door;

        public void Open()
        {
            GetComponent<Collider2D>().enabled = false;
            _door.gameObject.SetActive(true);
        }
    }
}