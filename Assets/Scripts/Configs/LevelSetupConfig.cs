using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "LevelSetupConfig", menuName = "ScriptableObject/LevelSetupConfig")]
    public class LevelSetupConfig : ScriptableObject
    {
        [Header("References")]
        public GameObject LevelPrefab;
    }
}