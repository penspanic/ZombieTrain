using UnityEngine;
using System.Collections;

namespace Logic
{
    public class TransitionZone : MonoBehaviour
    {
        [SerializeField]
        private GameObject _door;

        private Collider2D _lockCollider;
        private bool _isUnlocked;
        private bool _isDoorOpened;
        private void Awake()
        {
            _lockCollider = GetComponent<Collider2D>();
        }

        public void Unlock()
        {
            _isUnlocked = true;
        }

        private void Update()
        {
            if(_isUnlocked == false || _isDoorOpened == true)
            {
                return;
            }

            if(this.transform.position.DistanceWith(ActorContainer.Instance.LocalCharacter.transform.position) < 5f)
            {
                _isDoorOpened = true;
                _door.GetComponent<Animator>().SetTrigger("Open");
                _lockCollider.enabled = false;

            }
        }
    }
}