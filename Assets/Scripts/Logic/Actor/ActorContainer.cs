using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Logic
{
    public class ActorContainer : SingletonBehaviour<ActorContainer>
    {
        private Dictionary<int/*Serial*/, ActorBase> _actors = new Dictionary<int, ActorBase>();
        private SerialIssuer _serialIssuer = new SerialIssuer();

        protected override void Awake()
        {
            base.Awake();
        }

        public int Add(ActorBase actor)
        {
            if(_actors.ContainsKey(actor.Serial) == true)
            {
                throw new UnityException("Already actor " + actor.Serial + " Added!");
            }

            int newSerial = _serialIssuer.Get();
            _actors.Add(newSerial, actor);

            return newSerial;
        }

        public void Remove(ActorBase actor)
        {
            if(_actors.ContainsKey(actor.Serial) == false)
            {
                Debug.LogError("Already actor " + actor.Serial + " Removed!");
                return;
            }

            _actors.Remove(actor.Serial);
        }
    }
}