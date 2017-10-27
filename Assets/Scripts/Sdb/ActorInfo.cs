using UnityEngine;
using System.Collections;

namespace Sdb
{
    [CreateAssetMenu(fileName = "ActorInfo", menuName = "ActorInfo")]
    public class ActorInfo : SdbIdentifiableBase
    {
        public Constants.ActorType Type;
        public int MaxHp;
        public float MoveSpeed;
    }
}