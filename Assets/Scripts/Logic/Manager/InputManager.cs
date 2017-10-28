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
        [SerializeField]
        private Button _pickButton;

        private Character _character;

        protected override void Awake()
        {
            base.Awake();
            _attackButton.onClick.AddListener(ProcessAttack);
            _jumpButton.onClick.AddListener(ProcessJump);
            _pickButton.onClick.AddListener(ProcessPick);
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

            SetPickButtonInteractive();

            Vector2 dirVec = new Vector2(joystick.HorizontalAxis.Value, joystick.VerticalAxis.Value);
            _character.MoveController.ProcessJoystick(dirVec);

            if(Input.GetKeyDown(KeyCode.Alpha1) == true)
            {
                ProcessAttack();
            }
            if(Input.GetKeyDown(KeyCode.Alpha2) == true)
            {
                ProcessJump();
            }
            if(Input.GetKeyDown(KeyCode.Alpha3) == true)
            {
                ProcessPick();
            }
        }

        private void SetPickButtonInteractive()
        {
            _pickButton.interactable = RandomBoxContainer.Instance.IsInteractiveBoxExist();
        }

        private void ProcessAttack()
        {
            _character.AttackController.OnAttackCommand();
        }

        private void ProcessJump()
        {
            _character.MoveController.ProcessJump();
        }

        private void ProcessPick()
        {
            RandomBoxContainer.Instance.Use();
        }
    }
}