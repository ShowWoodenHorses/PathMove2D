using System;
using Assets.Scripts.Configs;
using Assets.Scripts.Core;
using Assets.Scripts.Enemies;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelController : MonoBehaviour
    {
        private LevelConfig levelsConfig;
        private EnemyManager enemyManager;
        private FuelSystem fuelSystem;
        private Transform player;

        private GameObject levelInstance;
        private LevelHolder levelHolder;
        private int currentLevelIndex;

        private bool hasCargo;

        private Action onPickUpCargo;
        private Action<LevelSetup> onStartLevel;
        private Action onFinishedLevel;

        public void Initialize(LevelConfig levelsConfig, EnemyManager enemyManager, FuelSystem fuelSystem, Transform player)
        {
            this.levelsConfig = levelsConfig;
            this.enemyManager = enemyManager;
            this.fuelSystem = fuelSystem;
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
            hasCargo = false;
            CreateLevelPrefab();
        }

        public void RegisterActionLevelController(Action onPickUpCargo, Action<LevelSetup> onStartLevel, Action onFinishedLevel)
        {
            this.onPickUpCargo = onPickUpCargo;
            this.onStartLevel = onStartLevel;
            this.onFinishedLevel = onFinishedLevel;
        }

        private void OnFinishedLevel()
        {
            if (hasCargo)
            {
                onFinishedLevel?.Invoke();
                Debug.Log("finished level");
            }
            else
            {
                Debug.Log("don't have cargo");
            }
        }
        private void OnPickUpCargo()
        {
            hasCargo = true;
            onPickUpCargo?.Invoke();
            Debug.Log($"cargo pick up {hasCargo}");
        }
        private void OnPickUpFuel(float countFuel)
        {
            fuelSystem.AddFuel(countFuel);
        }

        private void CreateLevelPrefab()
        {
            if (levelInstance != null)
            {
                Destroy(levelInstance.gameObject);
                levelInstance = null;
            }
            LevelSetup levelSetup = levelsConfig.Levels[currentLevelIndex];

            levelInstance = Instantiate(levelSetup.LevelHolder.gameObject, transform);
            levelInstance.transform.position = Vector3.zero;

            levelHolder = levelInstance.GetComponent<LevelHolder>();
            if (levelHolder != null)
            {
                levelHolder.Initialize(player, OnPickUpCargo, OnPickUpFuel, OnFinishedLevel);
                enemyManager.Initialize(player, levelHolder.Enemies);
                fuelSystem.SetMaxFuel(levelSetup.MaxFuel);
                onStartLevel?.Invoke(levelSetup);
            }

        }
    }
}