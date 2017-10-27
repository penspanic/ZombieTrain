using UnityEngine;
using System.Collections;

namespace Logic
{
    public class SectorManager : SingletonBehaviour<SectorManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void ChangeSector(int sectorIndex)
        {
            Sdb.SectorInfo nextSectorInfo = StageManager.Instance.CurrentStageInfo.SectorInfos[sectorIndex];
        }
    }
}