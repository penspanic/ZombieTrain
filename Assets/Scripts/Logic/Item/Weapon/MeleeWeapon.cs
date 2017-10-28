using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sdb;

namespace Logic
{
    public class MeleeWeapon : WeaponBase
    {
        public Sdb.MeeleWeaponInfo MeeleWeaponInfo { get; private set; }
        private Collider2D _hitCollider;
        private List<int> _hittedSerials = new List<int>();
        protected override void Awake()
        {
            base.Awake();
            _hitCollider = GetComponent<Collider2D>();
        }

        public override void Init(WeaponInfo weaponInfo)
        {
            base.Init(weaponInfo);
            MeeleWeaponInfo = SdbInstance<Sdb.MeeleWeaponInfo>.Get(weaponInfo.Id);
        }

        public void Activate()
        {
            _hittedSerials.Clear();
            StartCoroutine(ActivateProcess());
        }

        private IEnumerator ActivateProcess()
        {
            yield return new WaitForSeconds(MeeleWeaponInfo.StartActivateTime);
            _hitCollider.enabled = true;
            yield return new WaitForSeconds(MeeleWeaponInfo.ActivateTime);
            _hitCollider.enabled = false;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.layer != LayerMask.NameToLayer("Zombie"))
            {
                return;
            }

            Zombie zombie = other.GetComponent<Zombie>();
            if(zombie == null || _hittedSerials.Contains(zombie.Serial) == true)
            {
                return;
            }

            _hittedSerials.Add(zombie.Serial);
            zombie.GiveDamage(MeeleWeaponInfo.Damage);
        }
    }
}