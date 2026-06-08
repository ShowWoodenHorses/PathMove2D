using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObject/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public List<LevelHolder> Levels;
    }
}