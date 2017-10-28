using UnityEngine;
using System.Collections;

namespace Logic
{
    public class Zombie : ActorBase
    {
        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            Character character = ActorContainer.Instance.LocalCharacter;
            if(character == null)
            {
                return;
            }
            if(base.MoveController.IsGrounded == false)
            {
                return;
            }

            Vector2 diffPosition = character.transform.position - this.transform.position;
            diffPosition.y = 0;
            Vector2 direction = diffPosition.normalized;

            Vector2 velocity = RigidBody.velocity;
            velocity.x = direction.x * ActorInfo.MoveSpeed;

            RigidBody.velocity = velocity;
        }
    }
}