using UnityEngine;
using System.Collections;

namespace Sdb
{
    [CreateAssetMenu(fileName = "StageInfo", menuName = "StageInfo")]
    public class StageInfo : SdbIdentifiableBase
    {
        public SectorInfo[] SectorInfos;
    }

    [System.Serializable]
    public class SectorInfo
    {
        public float Length;
    }
}