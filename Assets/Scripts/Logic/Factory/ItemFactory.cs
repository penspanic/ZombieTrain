using UnityEngine;
using System.Collections;

namespace Logic
{
    public class ItemFactory : SingletonBehaviour<ItemFactory>
    {
        public ItemBase Create(string weaponId = null)
        {
            bool isWeaponCreate = Random.value > 0.2f;

            ItemBase itemInstance = null;
            if(string.IsNullOrEmpty(weaponId) == false || isWeaponCreate == true)
            {
                GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Item/WeaponItem");
                itemInstance = Instantiate(itemPrefab).GetComponent<ItemBase>();

                WeaponItem weaponItem = itemInstance as WeaponItem;
                weaponItem.WeaponId = string.IsNullOrEmpty(weaponId) == false ? weaponId : "1312";
            }
            else
            {
                GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Item/HpItem");
                itemInstance = Instantiate(itemPrefab).GetComponent<ItemBase>();
            }

            return itemInstance;
        }
    }
}