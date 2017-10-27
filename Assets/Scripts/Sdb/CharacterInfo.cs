using UnityEngine;
using System.Collections;

namespace Sdb
{
    [CreateAssetMenu(fileName = "CharacterInfo", menuName = "CharacterInfo")]
    public class CharacterInfo : SdbIdentifiableBase
    {
        public int JumpCoolTime;
        public float JumpPower;
    }
}