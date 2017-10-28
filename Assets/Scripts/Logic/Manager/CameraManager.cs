using UnityEngine;
using System.Collections;

namespace Logic
{
    public class CameraManager : SingletonBehaviour<CameraManager>
    {
        private CameraSmoothFollow _smoothFollow;

        protected override void Awake()
        {
            base.Awake();
            _smoothFollow = GameObject.FindObjectOfType<CameraSmoothFollow>();
        }

        public void SetCharacter(Character character)
        {
            _smoothFollow.SetTarget(character.gameObject);

        }
    }
}