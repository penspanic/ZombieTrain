using UnityEngine;
using System.Collections;

namespace Sdb
{
    [CreateAssetMenu(fileName = "MeleeWeaponInfo", menuName = "MeleeWeaponInfo")]
    public class MeleeWeaponInfo : SdbIdentifiableBase
    {
        public float StartActivateTime;
        public float ActivateTime;
        public int Damage;
        public float PushPower;
    }
}