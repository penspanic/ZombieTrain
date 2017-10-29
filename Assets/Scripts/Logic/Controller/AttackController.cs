using UnityEngine;
using System.Collections;

namespace Logic
{
    public class AttackController
    {
        public bool CanAttack { get; private set; }
        private Character _owner;

        private Coroutine _coolTimeCoroutine = null;
        public AttackController(Character owner)
        {
            this._owner = owner;
            owner.OnWeaponChanged += OnWeaponChanged;
            CanAttack = true;
        }

        private void OnWeaponChanged()
        {
            CanAttack = true;
            if(_coolTimeCoroutine != null)
            {
                _owner.StopCoroutine(_coolTimeCoroutine);
                _coolTimeCoroutine = null;
            }
        }

        public void OnAttackCommand()
        {
            if(_owner.Weapon == null)
            {
                return;
            }
            if(CanAttack == false)
            {
                return;
            }

            PlayAttackAnimation();
            if(_owner.Weapon.WeaponInfo.Type == Constants.WeaponType.Melee)
            {
                (_owner.Weapon as MeleeWeapon).Activate();
            }

            if(_owner.Weapon.Durability == 0)
            {
                _owner.EquipWeapon(null);
            }

            _coolTimeCoroutine = _owner.StartCoroutine(CoolTimeProcess());
        }

        private void PlayAttackAnimation()
        {
            switch(_owner.Weapon.WeaponInfo.Type)
            {
                case Constants.WeaponType.Melee:
                    _owner.Animator.SetTrigger("Meele");
                    break;
                case Constants.WeaponType.RangeWeapon:
                    Sdb.LauncherInfo launcherInfo = SdbInstance<Sdb.LauncherInfo>.Get(_owner.Weapon.WeaponInfo.Id);
                    if(launcherInfo.Type == Constants.LaunchType.Parabolic)
                    {
                        _owner.Animator.SetTrigger("Throw");
                    }
                    else
                    {
                        _owner.Animator.SetTrigger("Launcher");
                    }

                    break;
                default:
                    break;
            }
        }

        private IEnumerator EnableMeeleWeaponCollider(float time)
        {
            _owner.Weapon.GetComponent<Collider2D>().enabled = true;
            yield return new WaitForSeconds(time);
            _owner.Weapon.GetComponent<Collider2D>().enabled = false;
        }

        private IEnumerator CoolTimeProcess()
        {
            CanAttack = false;
            yield return new WaitForSeconds(_owner.Weapon.WeaponInfo.CoolTime);
            CanAttack = true;
        }
    }
}