using UnityEngine;
using System.Collections;

namespace Logic
{
    public class WeaponBase : MonoBehaviour
    {
        public Sdb.WeaponInfo WeaponInfo
        {
            get
            {
                return _weaponInfo;
            }
        }

        private Sdb.WeaponInfo _weaponInfo;

        public void Init(Sdb.WeaponInfo weaponInfo)
        {
            this._weaponInfo = weaponInfo;
        }
    }
}