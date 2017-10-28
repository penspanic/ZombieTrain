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
        }
    }
}