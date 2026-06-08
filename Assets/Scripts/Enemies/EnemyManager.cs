using System.Collections.Generic;
using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        private List<EnemyController> enemies = new List<EnemyController>();

        public void Initialize(Transform player, List<EnemyHolder> enemyHolders)
        {
            ClearEnemy();

            foreach (EnemyHolder enemyHolder in enemyHolders)
            {
                GameObject newEnemy = Instantiate(enemyHolder.Enemy, transform);
                EnemyController enemyController = newEnemy.GetComponent<EnemyController>();

                if (enemyController != null)
                {
                    enemies.Add(enemyController);
                    enemyController.Initialize(player, enemyHolder.Waypoints, OnHandlePlayerDetected);
                }

            }
        }

        private void ClearEnemy()
        {
            if (enemies.Count > 0)
            {
                foreach(EnemyController enemy in enemies)
                {
                    GameObject.Destroy(enemy.gameObject);
                }

                enemies.Clear();
            }
        }

        private void OnHandlePlayerDetected(EnemyController enemy)
        {
            Debug.Log($"[EnemyManager] Игрок обнаружен врагом: {enemy.name}");
        }
    }
}