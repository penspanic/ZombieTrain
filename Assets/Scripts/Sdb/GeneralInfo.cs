﻿using UnityEngine;
using System.Collections;

namespace Sdb
{
    [CreateAssetMenu(fileName = "GeneralInfo", menuName = "GeneralInfo")]
    public class GeneralInfo : ScriptableObject
    {
        public int ThrowWeaponDamage;
        public int HpItemHealValue;
        public float HitInvincibleTime;
        public float ZombieDetactionRange;
    }
}