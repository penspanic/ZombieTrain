using UnityEngine;
using System.Collections;

namespace Logic
{
    public class MoveController
    {
        private ActorBase _actor;
        public bool IsGrounded { get; private set; }
        public MoveController(ActorBase actor)
        {
            this._actor = actor;
            this._actor.OnGroundEnter += OnGroundEnter;
        }

        public void ProcessJoystick(Vector2 dirVec)
        {
            dirVec.y = 0;
            if(dirVec == Vector2.zero)
            {
                return;
            }
            if(_actor.IsInvincible == true)
            {
                return;
            }

            Vector2 velocity = _actor.RigidBody.velocity;
            velocity.x = dirVec.x * _actor.ActorInfo.MoveSpeed;

            _actor.RigidBody.velocity = velocity;
        }

        public void ProcessJump()
        {
            if(IsGrounded == false)
            {
                return;
            }

            IsGrounded = false;
            _actor.RigidBody.velocity += Vector2.up * (_actor as Character).CharacterInfo.JumpPower;

            AppSound.instance.SE_jump.Play();
        }

        private void OnGroundEnter()
        {
            IsGrounded = true;
        }
    }
}