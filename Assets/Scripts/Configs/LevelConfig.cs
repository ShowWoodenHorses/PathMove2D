using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObject/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public List<LevelSetup> Levels;
    }

    [Serializable]
    public class LevelSetup
    {
        public float MaxFuel;
        public LevelHolder LevelHolder;
    }
}