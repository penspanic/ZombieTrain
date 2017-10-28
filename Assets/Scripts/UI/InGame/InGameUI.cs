using UnityEngine;
using System.Collections;

namespace UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _heartObjects;

        private void Awake()
        {
            Logic.ActorContainer.Instance.OnActorAdded += OnActorAdded;
        }

        private void OnActorAdded(Logic.ActorBase actor)
        {
            if((actor as Logic.Character) == false)
            {
                return;
            }

            actor.OnHpChanged += OnCharacterHpChanged;
            OnCharacterHpChanged(actor.Hp);
        }

        private void OnCharacterHpChanged(int hp)
        {
            for(int i = 0; i < _heartObjects.Length; ++i)
            {
                _heartObjects[i].SetActive(false);
            }

            for(int i = 0; i < hp; ++i)
            {
                _heartObjects[i].SetActive(true);
            }
        }
    }
}