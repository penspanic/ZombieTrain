using UnityEngine;
using System.Collections;
using Sdb;

namespace Logic
{
    public class Character : ActorBase
    {
        public Sdb.CharacterInfo CharacterInfo { get; private set; }
        public WeaponBase Weapon { get; private set; }
        protected override void Awake()
        {
            base.Awake();
        }

        public override void Init(ActorInfo actorInfo)
        {
            base.Init(actorInfo);

            CharacterInfo = SdbInstance<Sdb.CharacterInfo>.Get(actorInfo.Id);
        }

        public void SetWeapon(WeaponBase weapon)
        {
            this.Weapon = weapon;
        }
    }
}