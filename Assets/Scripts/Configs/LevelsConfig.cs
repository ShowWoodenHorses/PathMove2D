using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "ScriptableObject/LevelsConfig")]
    public class LevelsConfig : ScriptableObject
    {
        [SerializeField] private List<LevelSetupConfig> levels;
    }
}