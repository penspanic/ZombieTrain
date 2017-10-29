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
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Item/Weapons/" + WeaponId);
            if(prefab == null)
            {
                throw new UnityException(WeaponId + " Weapon prefab not found!");
            }
            WeaponBase weapon = Instantiate(prefab).GetComponent<WeaponBase>();
            Sdb.WeaponInfo weaponInfo = SdbInstance<Sdb.WeaponInfo>.Get(WeaponId);

            switch(weaponInfo.Grade)
            {
                case Constants.WeaponGrade.Low:
                    EffectManager.Instance.Show(EffectType.Item_Low, this.gameObject.transform.position);
                    break;
                case Constants.WeaponGrade.Normal:
                    EffectManager.Instance.Show(EffectType.Item_Normal, this.gameObject.transform.position);
                    break;
                case Constants.WeaponGrade.High:
                    EffectManager.Instance.Show(EffectType.Item_High, this.gameObject.transform.position);
                    break;
            }
            weapon.Init(weaponInfo);
            weapon.SetOwner(owner);
            ActorContainer.Instance.LocalCharacter.EquipWeapon(weapon);
        }
    }
}