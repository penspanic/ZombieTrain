using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Logic
{
    public class ActorContainer : SingletonBehaviour<ActorContainer>
    {
        public Character LocalCharacter { get; private set; }

        private Dictionary<int/*Serial*/, ActorBase> _actors = new Dictionary<int, ActorBase>();
        private SerialIssuer _serialIssuer = new SerialIssuer();

        public event System.Action<ActorBase> OnActorAdded;
        public event System.Action<ActorBase> OnActorRemoved;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Add(ActorBase actor)
        {
            if(_actors.ContainsKey(actor.Serial) == true)
            {
                throw new UnityException("Already actor " + actor.Serial + " Added!");
            }

            int newSerial = _serialIssuer.Get();
            actor.SetSerial(newSerial);

            _actors.Add(newSerial, actor);

            if(actor is Character)
            {
                LocalCharacter = actor as Character;
            }

            OnActorAdded?.Invoke(actor);
        }

        public void Remove(ActorBase actor)
        {
            if(_actors.ContainsKey(actor.Serial) == false)
            {
                Debug.LogError("Already actor " + actor.Serial + " Removed!");
                return;
            }

            _actors.Remove(actor.Serial);
            OnActorRemoved?.Invoke(actor);
        }

        public int GetZombieCount()
        {
            return (from actor in _actors
                    where actor.Value.ActorInfo.Type == Constants.ActorType.Zombie
                    select actor.Value).Count();
        }
    }
}