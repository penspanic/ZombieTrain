using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
        private GameObject _trainParent;
        [SerializeField]
        private GameObject _trainPrefab;
        [SerializeField]
        private GameObject _transitionZonePrefab;
        [SerializeField]
        private GameObject _randomBoxPrefab;
        [SerializeField]
        private Text _scoreText;

        private List<TransitionZone> transitionZones = new List<TransitionZone>();

        protected override void Awake()
        {
            base.Awake();
            ActorContainer.Instance.OnActorRemoved += OnActorRemoved;
        }

        public void CreateSectors()
        {
            transitionZones.Add(GameObject.FindObjectOfType<TransitionZone>());

            float createPositionX = 6.4f;
            for(int i = 0; i < StageManager.Instance.CurrentStageInfo.SectorInfos.Length; ++i)
            {
                for(int j = 0; j < StageManager.Instance.CurrentStageInfo.SectorInfos[i].Length; ++j)
                {
                    GameObject newTrain = Instantiate(_trainPrefab);
                    newTrain.transform.SetParent(_trainParent.transform, false);
                    newTrain.transform.localPosition = new Vector2(createPositionX, 0f);

                    createPositionX += 12.8f;
                }

                TransitionZone newTransitionZone = Instantiate(_transitionZonePrefab).GetComponent<TransitionZone>();
                newTransitionZone.transform.SetParent(_trainParent.transform, false);
                newTransitionZone.transform.localPosition = new Vector2(createPositionX - 4.8f, 0f);

                transitionZones.Add(newTransitionZone);

                createPositionX += 3.2f;
            }
        }

        public void ChangeSector(int sectorIndex)
        {
            CurrentSectorIndex = sectorIndex;
            transitionZones[sectorIndex].Unlock();

            CreateZombies();
        }

        private Vector2 GetSectorRange(int index)
        {
            float totalLength = 0f;
            float lastSectorPosition = 0f;
            for(int i = 0; i <= index; ++i)
            {
                Sdb.SectorInfo eachSector = StageManager.Instance.CurrentStageInfo.SectorInfos[i];
                lastSectorPosition = totalLength;
                totalLength += eachSector.Length * 12.8f;
            }

            return new Vector2(lastSectorPosition + 1f, totalLength - 1f);
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
                if (DynamicInfo.DynamicInfo.CurrentScore > DynamicInfo.DynamicInfo.HighScore)
                {
                    PlayerPrefs.SetInt("HighScore", DynamicInfo.DynamicInfo.CurrentScore);
                }
                GameResultManager.instance.GameOver();
                return;
            }

            ++DynamicInfo.DynamicInfo.CurrentScore;
            _scoreText.text = "KILLS:" + DynamicInfo.DynamicInfo.CurrentScore;

            float randomBoxDropRate = (float)CurrentSectorInfo.BoxDropPercent * 0.01f;
            bool isDropBox = Random.value < randomBoxDropRate;

            CreateRandomBox(actor.transform.position.x, string.Empty);

            if(ActorContainer.Instance.GetZombieCount() == 0)
            {
                ChangeSector(++CurrentSectorIndex);
            }
        }

        public void CreateRandomBox(float xPosition, string itemId)
        {
            RandomBox boxInstance = Instantiate<GameObject>(_randomBoxPrefab).GetComponent<RandomBox>();
            boxInstance.ItemId = itemId;

            Vector2 createPosition = new Vector2(xPosition, Random.Range(0.5f, 1f));
            createPosition.y = -1.8f;
            boxInstance.transform.position = createPosition;

            //boxInstance.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10f);
        }
    }
}