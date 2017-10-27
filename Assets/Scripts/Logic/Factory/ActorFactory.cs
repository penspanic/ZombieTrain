using UnityEngine;
using System.Collections;

namespace Logic
{
    public class ActorFactory : SingletonBehaviour<ActorFactory>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public ActorBase Create(Sdb.ActorInfo actorInfo)
        {
            GameObject model = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Models/" + actorInfo.Id));

            ActorBase actor = null;
            switch(actorInfo.Type)
            {
                case Constants.ActorType.Character:
                    actor = model.AddComponent<Character>();
                    break;
                case Constants.ActorType.Zombie:
                    actor = model.AddComponent<Zombie>();
                    break;
                default:
                    break;
            }

            actor.Init(actorInfo);
            return actor;
        }
    }
}
