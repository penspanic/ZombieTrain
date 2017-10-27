using UnityEngine;
using System.Collections;

namespace Logic
{
    public class MoveController
    {
        private ActorBase _actor;
        public MoveController(ActorBase actor)
        {
            this._actor = actor;
        }

        public void ProcessJoystick(Vector2 dirVec)
        {
            dirVec.y = 0;
            _actor.RigidBody.velocity = dirVec * _actor.ActorInfo.MoveSpeed;
        }

        public void ProcessJump()
        {
        }
    }
}