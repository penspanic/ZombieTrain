using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Logic
{
    public class InputManager : SingletonBehaviour<InputManager>
    {
        [SerializeField]
        private Joystick joystick;
        [SerializeField]
        private Button _attackButton;
        [SerializeField]
        private Button _jumpButton;

        private Character _character;

        protected override void Awake()
        {
            base.Awake();
            _attackButton.onClick.AddListener(ProcessAttack);
            _jumpButton.onClick.AddListener(ProcessJump);
        }

        public void SetCharacter(Character character)
        {
            this._character = character;
        }

        private void Update()
        {
            if(_character == null)
            {
                return;
            }

            Vector2 dirVec = new Vector2(joystick.HorizontalAxis.Value, joystick.VerticalAxis.Value);
            Debug.Log(dirVec.ToPreciseString());
            _character.MoveController.ProcessJoystick(dirVec);

            if(Input.GetKeyDown(KeyCode.Alpha1) == true)
            {
                ProcessAttack();
            }
            if(Input.GetKeyDown(KeyCode.Alpha2) == true)
            {
                ProcessJump();
            }
        }

        private void ProcessAttack()
        {

        }

        private void ProcessJump()
        {
            _character.MoveController.ProcessJump();
        }
    }
}