using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Logic
{
    public enum EffectType
    {
        Hit_Low,
        Hit_Normal,
        Hit_High,
        Item_Low,
        Item_Normal,
        Item_High,
        Explosion,
        End_Of_Effect,
    }

    public class EffectManager : SingletonBehaviour<EffectManager>
    {
        private Dictionary<EffectType, GameObject> _prefabs = new Dictionary<EffectType, GameObject>();
        protected override void Awake()
        {
            base.Awake();
            LoadPrefabs();
        }

        private void LoadPrefabs()
        {
            for(int i = 0;  i < (int)EffectType.End_Of_Effect; ++i)
            {
                _prefabs.Add((EffectType)i, Resources.Load<GameObject>("Prefabs/Effect/" + ((EffectType)i).ToString()));
            }
        }

        public void Show(EffectType type, Vector2 position)
        {
            Instantiate(_prefabs[type]).transform.position = position;
        }
    }
}