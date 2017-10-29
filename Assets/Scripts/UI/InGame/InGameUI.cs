using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _heartObjects;
        [SerializeField]
        private Text _durabilityText;

        private void Start()
        {

            Logic.ActorContainer.Instance.LocalCharacter.OnHpChanged += OnCharacterHpChanged;
            Logic.ActorContainer.Instance.LocalCharacter.OnWeaponDurabilityChanged += OnCharacterWeaponChanged;

            OnCharacterWeaponChanged();
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

        private void OnCharacterWeaponChanged()
        {
            Logic.WeaponBase currentWeapon = Logic.ActorContainer.Instance.LocalCharacter.Weapon;
            int durability = 0;
            int maxDurability = 0;
            if(currentWeapon != null)
            {
                durability = currentWeapon.Durability;
                maxDurability = currentWeapon.WeaponInfo.DurabilityCount;
            }

            _durabilityText.text = durability.ToString() + " / " + maxDurability.ToString();
        }
    }
}