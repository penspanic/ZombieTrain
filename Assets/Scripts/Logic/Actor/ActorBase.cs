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

        #region Events

        public event System.Action OnGroundEnter;

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

        public void Heal(int healAmount)
        {
            this.Hp += healAmount;
            if(this.Hp > ActorInfo.MaxHp)
            {
                this.Hp = ActorInfo.MaxHp;
            }
        }
    }
}