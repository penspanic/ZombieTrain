using UnityEngine;
using System.Collections;

namespace Sdb
{
    [CreateAssetMenu(fileName = "WeaponInfo", menuName = "WeaponInfo")]
    public class WeaponInfo : SdbIdentifiableBase
    {
        public Constants.WeaponType Type;
        public Constants.WeaponGrade Grade;
        public float CoolTime;
        public string SoundEffectName;
    }
}