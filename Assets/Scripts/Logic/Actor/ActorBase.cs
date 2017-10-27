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

        public MoveController MoveController { get; private set; }
        protected virtual void Awake()
        {
            RigidBody = GetComponentInChildren<Rigidbody2D>();
            MoveController = new MoveController(this);
        }

        public virtual void Init(Sdb.ActorInfo actorInfo)
        {
            this.ActorInfo = actorInfo;

            this.Serial = ActorContainer.Instance.Add(this);
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
    }
}