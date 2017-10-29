using UnityEngine;
using System.Collections;

namespace Logic
{
    public class ActorBase : MonoBehaviour
    {
        public int Serial { get; private set; }
        public Sdb.ActorInfo ActorInfo
        {
            get;
            private set;
        }

        public Rigidbody2D RigidBody
        {
            get;
            private set;
        }

        public Animator Animator { get; private set; }
        public MoveController MoveController { get; private set; }

        public int Hp { get; protected set; }
        public bool IsInvincible { get; protected set; }
        #region Events

        public event System.Action OnGroundEnter;
        public event System.Action<int> OnHpChanged;
        public event System.Action OnDamaged;
        public event System.Action OnDead;

        public event System.Action OnInvincibleStart;
        public event System.Action OnInvincibleEnd;

        #endregion
        protected virtual void Awake()
        {
            RigidBody = GetComponentInChildren<Rigidbody2D>();
            Animator = GetComponentInChildren<Animator>();
            MoveController = new MoveController(this);
        }

        public virtual void Init(Sdb.ActorInfo actorInfo)
        {
            this.ActorInfo = actorInfo;
            this.Hp = actorInfo.MaxHp;

            ActorContainer.Instance.Add(this);
        }

        public void SetSerial(int serial)
        {
            this.Serial = serial;
        }

        protected virtual void FixedUpdate()
        {
            if(Mathf.Abs(RigidBody.velocity.x) < 0.1f)
            {
                return;
            }

            if(RigidBody.velocity.x < 0)
            {
                this.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.CompareTag("Ground") == true)
            {
                OnGroundEnter?.Invoke();
            }
        }

        public bool GiveDamage(ActorBase attacker, int damage)
        {
            if(IsInvincible == true)
            {
                return false;
            }

            this.Hp -= damage;
            if(this.Hp <= 0)
            {
                this.Hp = 0;
                ActorContainer.Instance.Remove(this);
                Destroy(this.gameObject);
            }

            OnHpChanged?.Invoke(this.Hp);
            OnDamaged?.Invoke();

            Vector2 attackDirection = this.transform.position - attacker.transform.position;
            float pushPower = 10f;
            if(attacker is Character && (attacker as Character).Weapon is MeleeWeapon)
            {
                pushPower = ((attacker as Character).Weapon as MeleeWeapon).MeleeWeaponInfo.PushPower;
            }
            PushByHit(pushPower, attackDirection.normalized);

            StartCoroutine(InvincibleProcess());
            return true;
        }

        private IEnumerator InvincibleProcess()
        {
            IsInvincible = true;
            OnInvincibleStart?.Invoke();
            yield return new WaitForSeconds(SpecificSdb<Sdb.GeneralInfo>.Get().HitInvincibleTime);
            IsInvincible = false;
            OnInvincibleEnd?.Invoke();
        }

        private void PushByHit(float power, Vector2 direction)
        {
            if(direction.x < 0)
            {
                RigidBody.velocity = new Vector2(-1f, 1f) * power;
            }
            else
            {
                RigidBody.velocity = new Vector2(1f, 1f) * power;
            }
        }

        public void Heal(int healAmount)
        {
            this.Hp += healAmount;
            if(this.Hp > ActorInfo.MaxHp)
            {
                this.Hp = ActorInfo.MaxHp;
            }

            OnHpChanged?.Invoke(this.Hp);
        }
    }
}