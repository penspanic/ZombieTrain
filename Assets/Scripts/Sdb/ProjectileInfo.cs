using UnityEngine;
using System.Collections;

namespace Sdb
{
    [CreateAssetMenu(fileName = "ProjectileInfo", menuName = "ProjectileInfo")]
    public class ProjectileInfo : SdbIdentifiableBase
    {
        public Constants.LaunchType Type;
        public float MoveTime;
        public int Damage;
    }
}