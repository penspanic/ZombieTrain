using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Logic
{
    public class ItemFactory : SingletonBehaviour<ItemFactory>
    {
        public ItemBase Create(string weaponId = null)
        {
            bool isWeaponCreate = Random.value > 0.1f;

            ItemBase itemInstance = null;
            if(string.IsNullOrEmpty(weaponId) == false || isWeaponCreate == true)
            {
                GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Item/WeaponItem");
                itemInstance = Instantiate(itemPrefab).GetComponent<ItemBase>();

                WeaponItem weaponItem = itemInstance as WeaponItem;
                weaponItem.WeaponId = string.IsNullOrEmpty(weaponId) == false ? weaponId : GetRandomWeaponId();
            }
            else
            {
                GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Item/HpItem");
                itemInstance = Instantiate(itemPrefab).GetComponent<ItemBase>();
            }

            return itemInstance;
        }

        private string GetRandomWeaponId()
        {
            // 50, 30, 20
            List<Sdb.WeaponInfo> weaponInfos = SdbInstance<Sdb.WeaponInfo>.GetAll();
            Dictionary<Constants.WeaponGrade, List<Sdb.WeaponInfo>> infoByGrade = new Dictionary<Constants.WeaponGrade, List<Sdb.WeaponInfo>>();
            for(int i = 0; i < weaponInfos.Count; ++i)
            {
                if(infoByGrade.ContainsKey(weaponInfos[i].Grade) == false)
                {
                    infoByGrade.Add(weaponInfos[i].Grade, new List<Sdb.WeaponInfo>());
                }

                infoByGrade[weaponInfos[i].Grade].Add(weaponInfos[i]);
            }

            infoByGrade[Constants.WeaponGrade.Low].Remove(SdbInstance<Sdb.WeaponInfo>.Get("Basic_Weapon"));
            float random = Random.value;
            Constants.WeaponGrade selectedGrade = Constants.WeaponGrade.Undefined;
            if(random > 0.8f)
            {
                selectedGrade = Constants.WeaponGrade.High;
            }
            else if(random > 0.5f)
            {
                selectedGrade = Constants.WeaponGrade.Normal;
            }
            else
            {
                selectedGrade = Constants.WeaponGrade.Low;
            }

            int gradeWeaponsCount = infoByGrade[selectedGrade].Count;
            return infoByGrade[selectedGrade][Random.Range(0, gradeWeaponsCount)].Id;
        }
    }
}