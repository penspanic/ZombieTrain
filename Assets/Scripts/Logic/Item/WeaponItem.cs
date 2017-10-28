using UnityEngine;
using System.Collections;

namespace Logic
{
    public class WeaponItem : ItemBase
    {
        public string WeaponId { get; set; }
        public override void Use(Character owner)
        {
            base.Use(owner);

            WeaponBase weapon = Instantiate(Resources.Load<GameObject>("Prefabs/Item/Weapons/" + WeaponId)).GetComponent<WeaponBase>();
            weapon.Init(SdbInstance<Sdb.WeaponInfo>.Get(WeaponId));
            ActorContainer.Instance.LocalCharacter.EquipWeapon(weapon);
        }
    }
}