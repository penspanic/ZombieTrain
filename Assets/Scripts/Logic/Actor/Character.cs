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

        private GameObject _weaponParent;

        protected override void Awake()
        {
            base.Awake();
            AttackController = new AttackController(this);
            _weaponParent = transform.FindRecursive("WeaponParent").gameObject;

            base.OnDamaged += OnCharacterDamaged;
        }

        public override void Init(ActorInfo actorInfo)
        {
            base.Init(actorInfo);

            CharacterInfo = SdbInstance<Sdb.CharacterInfo>.Get(actorInfo.Id);
        }

        private void OnCharacterDamaged()
        {
            StartCoroutine(InvincibleProcess());
        }

        private IEnumerator InvincibleProcess()
        {
            IsInvincible = true;
            yield return new WaitForSeconds(SpecificSdb<Sdb.GeneralInfo>.Get().HitInvincibleTime);
            IsInvincible = false;
        }

        public void SetWeapon(WeaponBase weapon)
        {
            this.Weapon = weapon;
            this.Weapon.SetOwner(this);
            OnWeaponChanged?.Invoke();
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
            this._weaponParent.transform.localPosition = Vector2.zero;
        }

        private void ThrowWeapon(WeaponBase weapon)
        {
            Destroy(weapon);
        }
    }
}