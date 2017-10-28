using UnityEngine;
using System.Collections;

namespace Sdb
{
    [CreateAssetMenu(fileName = "WeaponInfo", menuName = "WeaponInfo")]
    public class WeaponInfo : SdbIdentifiableBase
    {
        public Constants.WeaponType Type;
        public float CoolTime;
    }

    [CreateAssetMenu(fileName = "MeeleWeaponInfo", menuName = "MeeleWeaponInfo")]
    public class MeeleWeaponInfo : SdbIdentifiableBase
    {
        public float StartActivateTime;
        public float ActivateTime;
        public int Damage;
        public float PushPower;
    }

    [CreateAssetMenu(fileName = "LauncherInfo", menuName = "LauncherInfo")]
    public class LauncherInfo : SdbIdentifiableBase
    {
        public Constants.LaunchType Type;
        public string ProjectileId;
        public float LaunchPower;
    }

    [CreateAssetMenu(fileName = "ProjectileInfo", menuName = "ProjectileInfo")]
    public class ProjectileInfo : SdbIdentifiableBase
    {
        public Constants.LaunchType Type;
        public string ProjectileId;
    }
}