using UnityEngine;
using System.Collections;

namespace Constants
{
    public enum WeaponType
    {
        Undefined = 0,
        Melee,
        RangeWeapon,
    }

    public enum WeaponGrade
    {
        Undefined = 0,
        Low,
        Normal,
        High,
    }

    public enum LaunchType
    {
        Undefined = 0,
        Straight,
        Parabolic,
    }
}