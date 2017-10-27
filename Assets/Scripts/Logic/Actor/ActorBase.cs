using UnityEngine;
using System.Collections;

namespace Logic
{
    public class ActorBase : MonoBehaviour
    {
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

        public int Serial { get; private set; }

        protected virtual void Awake()
        {
            RigidBody = GetComponentInChildren<Rigidbody2D>();
        }

        public void Init(Sdb.ActorInfo actorInfo)
        {
            this.ActorInfo = actorInfo;

            this.Serial = ActorContainer.Instance.Add(this);
        }
    }
}