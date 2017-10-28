using UnityEngine;
using System.Collections;

namespace Sdb
{
    [CreateAssetMenu(fileName = "LauncherInfo", menuName = "LauncherInfo")]
    public class LauncherInfo : SdbIdentifiableBase
    {
        public Constants.LaunchType Type;
        public string ProjectileId;
        public float LaunchPower;
    }
}