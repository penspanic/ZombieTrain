﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Logic
{
    public class Zombie : ActorBase
    {
        protected override void Awake()
        {
            base.Awake();
            base.OnDamaged += OnZombieDamaged;
            base.OnDead += OnZombieDead;
        }

        private void Update()
        {
            Character character = ActorContainer.Instance.LocalCharacter;
            if(character == null)
            {
                return;
            }
            if(base.MoveController.IsGrounded == false || base.IsInvincible == true)
            {
                return;
            }
            if(character.transform.position.DistanceWith(this.transform.position) > SpecificSdb<Sdb.GeneralInfo>.Get().ZombieDetactionRange)
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

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Animator.SetFloat("Velocity", Mathf.Abs(RigidBody.velocity.x));
        }

        protected override void OnCollisionEnter2D(Collision2D other)
        {
            base.OnCollisionEnter2D(other);

            if(other.gameObject.layer == LayerMask.NameToLayer("Character") && this.IsInvincible == false)
            {
                other.gameObject.GetComponent<Character>().GiveDamage(this, 1);
            }
        }

        protected void OnCollisionStay2D(Collision2D other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Character") && this.IsInvincible == false)
            {
                other.gameObject.GetComponent<Character>().GiveDamage(this, 1);
            }
        }

        private void OnZombieDamaged()
        {
            AppSound.instance.SE_zombie_hit.Play();
            EffectType randomEffect = (EffectType)Random.Range((int)EffectType.Zombie_Arm, (int)EffectType.Zombie_Leg + 1);
            EffectManager.Instance.Show(randomEffect, this.transform.position);
        }

        private void OnZombieDead()
        {
            AppSound.instance.SE_zombie_die.Play();
        }
    }
}