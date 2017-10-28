using UnityEngine;
using System.Collections;

namespace Logic
{
    public class SectorManager : SingletonBehaviour<SectorManager>
    {
        public Sdb.SectorInfo CurrentSectorInfo
        {
            get
            {
                return StageManager.Instance.CurrentStageInfo.SectorInfos[CurrentSectorIndex];
            }
        }

        public int CurrentSectorIndex
        {
            get; private set;
        }

        public Vector2 CurrentSectorRange
        {
            get
            {
                return GetSectorRange(CurrentSectorIndex);
            }
        }

        [SerializeField]
        private GameObject _randomBoxPrefab;

        protected override void Awake()
        {
            base.Awake();
            ActorContainer.Instance.OnActorRemoved += OnActorRemoved;
        }

        public void ChangeSector(int sectorIndex)
        {
            CurrentSectorIndex = sectorIndex;

            CreateZombies();
        }

        private Vector2 GetSectorRange(int index)
        {
            float totalLength = 0f;
            float lastSectorLength = 0f;
            for(int i = 0; i <= index; ++i)
            {
                Sdb.SectorInfo eachSector = StageManager.Instance.CurrentStageInfo.SectorInfos[i];
                totalLength += eachSector.Length * 12.8f;
                lastSectorLength = eachSector.Length;
            }

            return new Vector2(totalLength - lastSectorLength, totalLength);
        }

        private void CreateZombies()
        {
            for(int i = 0; i < CurrentSectorInfo.ZombieCount; ++i)
            {
                Sdb.ActorInfo zombieActorInfo = SdbInstance<Sdb.ActorInfo>.Get("Zombie");
                ActorBase zombieActor = ActorFactory.Instance.Create(zombieActorInfo);
                zombieActor.transform.position = GetZombieCreatePosition();
            }
        }

        private Vector2 GetZombieCreatePosition()
        {
            Vector2 sectorRangeX = CurrentSectorRange;
            return new Vector2(Random.Range(sectorRangeX.x, sectorRangeX.y), 1f);
        }

        private void OnActorRemoved(ActorBase actor)
        {
            if(actor.ActorInfo.Type != Constants.ActorType.Zombie)
            {
                return;
            }

            float randomBoxDropRate = (float)CurrentSectorInfo.BoxDropPercent * 0.01f;
            bool isDropBox = Random.value < randomBoxDropRate;
        }

        public void CreateRandomBox(float xPosition, string itemId)
        {
            RandomBox boxInstance = Instantiate<GameObject>(_randomBoxPrefab).GetComponent<RandomBox>();
            boxInstance.ItemId = itemId;

            Vector2 createPosition = new Vector2(xPosition, Random.Range(0.5f, 1f));
            boxInstance.transform.position = createPosition;

            //boxInstance.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10f);
        }
    }
}