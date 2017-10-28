using UnityEngine;
using System.Collections;

namespace Logic
{
    public class HpItem : ItemBase
    {
        public override void Use(Character owner)
        {
            base.Use(owner);
            owner.Heal(SpecificSdb<Sdb.GeneralInfo>.Get().HpItemHealValue);
            // 이펙트 출력

            Destroy(this.gameObject);
        }
    }
}