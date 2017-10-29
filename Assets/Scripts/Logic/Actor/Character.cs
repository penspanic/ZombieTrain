using UnityEngine;
using System.Collections;
using Sdb;

namespace Logic
{
    public class Character : ActorBase
    {
        public Sdb.CharacterInfo CharacterInfo { get; private set; }

        public AttackController AttackController { get; private set; }
        public WeaponBase Weapon { get; private set; }


        public event System.Action OnWeaponChanged;
        public event System.Action OnWeaponDurabilityChanged;

        private GameObject _weaponParent;

        protected override void Awake()
        {
            base.Awake();
            AttackController = new AttackController(this);
            _weaponParent = transform.FindRecursive("WeaponParent").gameObject;
        }

        public override void Init(ActorInfo actorInfo)
        {
            base.Init(actorInfo);

            CharacterInfo = SdbInstance<Sdb.CharacterInfo>.Get(actorInfo.Id);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            Animator.SetFloat("Velocity", Mathf.Abs(RigidBody.velocity.x));
            Animator.SetBool("IsGrounded", MoveController.IsGrounded);
        }

        public void EquipWeapon(WeaponBase weapon)
        {
            if(this.Weapon != null)
            {
                ThrowWeapon(this.Weapon);
            }

            this.Weapon = weapon;
            this.Weapon.transform.SetParent(_weaponParent.transform, false);
            this.Weapon.transform.localPosition = Vector2.zero;

            OnWeaponChanged?.Invoke();
        }

        private void ThrowWeapon(WeaponBase weapon)
        {
            Destroy(weapon.gameObject);
        }

        public void WeaponDurabilityChnaged()
        {
            OnWeaponDurabilityChanged?.Invoke();
        }
    }
}