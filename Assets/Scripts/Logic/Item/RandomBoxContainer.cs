using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Logic
{
    public class RandomBoxContainer : SingletonBehaviour<RandomBoxContainer>
    {
        private SerialIssuer _serialIssuer = new SerialIssuer();
        private Dictionary<int/*Serial*/, RandomBox> _randomBoxes = new Dictionary<int, RandomBox>();

        protected override void Awake()
        {
            base.Awake();
        }

        public void Add(RandomBox box)
        {
            if(_randomBoxes.ContainsKey(box.Serial) == true)
            {
                throw new UnityException("Already box " + box.Serial + " Added!");
            }

            int newSerial = _serialIssuer.Get();
            box.SetSerial(newSerial);

            _randomBoxes.Add(newSerial, box);
        }

        public void Remove(RandomBox box)
        {
            if(_randomBoxes.ContainsKey(box.Serial) == false)
            {
                Debug.LogError("Already box " + box.Serial + " Removed!");
                return;
            }

            _randomBoxes.Remove(box.Serial);
        }

        public bool IsInteractiveBoxExist()
        {
            return (from box in _randomBoxes.Values
             where box.IsInteractive == true
             select box).Count() > 0;
        }

        public void Use()
        {
            var interactiveBoxes = (from box in _randomBoxes.Values
                                    where box.IsInteractive == true
                                    orderby box.Serial descending
                                    select box);

            if(interactiveBoxes.Count() == 0)
            {
                return;
            }

            RandomBox boxToUse = interactiveBoxes.ToArray()[0];
            boxToUse.Use();
        }
    }
}
