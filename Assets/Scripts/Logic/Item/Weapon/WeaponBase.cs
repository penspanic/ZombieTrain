using UnityEngine;
using System.Collections;

namespace Logic
{
    public class WeaponBase : MonoBehaviour
    {
        public Sdb.WeaponInfo WeaponInfo { get; private set; }
        public Character Owner { get; private set; }

        public int Durability { get; protected set; }

        protected virtual void Awake()
        {

        }

        public virtual void Init(Sdb.WeaponInfo weaponInfo)
        {
            this.WeaponInfo = weaponInfo;
            this.Durability = weaponInfo.DurabilityCount;
        }

        public void SetOwner(Character owner)
        {
            this.Owner = owner;
        }
    }
}