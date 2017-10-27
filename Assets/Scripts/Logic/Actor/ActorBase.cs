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

        public void Init(Sdb.ActorInfo actorInfo)
        {
            this.ActorInfo = actorInfo;

            this.Serial = ActorContainer.Instance.Add(this);
        }
    }
}