using UnityEngine;
using System.Collections;

namespace Logic
{
    public class InputManager : SingletonBehaviour<InputManager>
    {
        [SerializeField]
        private Joystick joystick;
        private Character _character;

        protected override void Awake()
        {
            base.Awake();
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
        }
    }
}