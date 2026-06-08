using Assets.Scripts.Configs;
using Assets.Scripts.Enemies;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelController : MonoBehaviour
    {
        private LevelConfig levelsConfig;
        private EnemyManager enemyManager;
        private Transform player;

        private GameObject levelInstance;
        private LevelHolder levelHolder;
        private int currentLevelIndex;

        public void Initialize(LevelConfig levelsConfig, EnemyManager enemyManager, Transform player)
        {
            this.levelsConfig = levelsConfig;
            this.enemyManager = enemyManager;
            this.player = player;

            StartLevel(0);
        }

        public void StartLevel(int index)
        {
            currentLevelIndex = index;

            RestartLevel();
        }

        public void RestartLevel()
        {
            CreateLevelPrefab();
        }

        private void OnFinishedLevel()
        {
            Debug.Log("finished level");
        }

        private void CreateLevelPrefab()
        {
            if (levelInstance != null)
            {
                Destroy(levelInstance.gameObject);
                levelInstance = null;
            }

            levelInstance = Instantiate(levelsConfig.Levels[currentLevelIndex].gameObject, transform);
            levelInstance.transform.position = Vector3.zero;

            levelHolder = levelInstance.GetComponent<LevelHolder>();
            if (levelHolder != null)
            {
                levelHolder.Initialize(player, OnFinishedLevel);
                enemyManager.Initialize(player, levelHolder.Enemies);
            }

        }
    }
}