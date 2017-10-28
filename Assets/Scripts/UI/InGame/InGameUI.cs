using UnityEngine;
using System.Collections;

namespace UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _heartObjects;

        private void Start()
        {

            Logic.ActorContainer.Instance.LocalCharacter.OnHpChanged += OnCharacterHpChanged;
            OnCharacterHpChanged(Logic.ActorContainer.Instance.LocalCharacter.Hp);
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
            Debug.Log("OnCharacterHpChanged");
        }
    }
}