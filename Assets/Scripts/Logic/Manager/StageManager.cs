using UnityEngine;
using System.Collections;

namespace Logic
{
    public class StageManager : SingletonBehaviour<StageManager>
    {
        public Sdb.StageInfo CurrentStageInfo
        {
            get
            {
                return _currentStageInfo;
            }
        }

        private Sdb.StageInfo _currentStageInfo;
        protected override void Awake()
        {
            base.Awake();
            LoadStage();
        }


        private void LoadStage()
        {
            Application.targetFrameRate = 60;
            CreateCharacter();

            _currentStageInfo = SdbInstance<Sdb.StageInfo>.Get(DynamicInfo.DynamicInfo.SelectedStageId);
            SectorManager.Instance.ChangeSector(0);
        }

        private void CreateCharacter()
        {
            Character character = ActorFactory.Instance.Create(SdbInstance<Sdb.ActorInfo>.Get("Character1")) as Character;
            character.transform.position = new Vector3(-5f, -1.37f, 0f);

            SectorManager.Instance.CreateRandomBox(character.transform.position.x + 1f, "Basic_Weapon");
            InputManager.Instance.SetCharacter(character);
            CameraManager.Instance.SetCharacter(character);
        }
    }
}