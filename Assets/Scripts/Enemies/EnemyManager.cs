using System.Collections.Generic;
using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyManager : MonoBehaviour
    {

        [Header("References")]
        public List<EnemyController> enemies = new List<EnemyController>();

        [Header("Settings")]
        public bool autoDetectEnemies = true;
        public LayerMask enemyLayer;

        void Start()
        {
            if (autoDetectEnemies)
            {
                DetectAllEnemies();
            }

            foreach (EnemyController enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.OnPlayerDetected += HandlePlayerDetected;
                }
            }
        }

        void OnDestroy()
        {
            foreach (EnemyController enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.OnPlayerDetected -= HandlePlayerDetected;
                }
            }
        }

        void HandlePlayerDetected(EnemyController enemy)
        {
            Debug.Log($"[EnemyManager] Игрок обнаружен врагом: {enemy.name}");
        }

        public void DetectAllEnemies()
        {
            enemies.Clear();

            Collider2D[] hits = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);

            foreach (Collider2D hit in hits)
            {
                if (enemyLayer == (enemyLayer | (1 << hit.gameObject.layer)))
                {
                    EnemyController enemy = hit.GetComponent<EnemyController>();
                    if (enemy != null && !enemies.Contains(enemy))
                    {
                        enemies.Add(enemy);
                    }
                }
            }

            Debug.Log($"[EnemyManager] Найдено врагов: {enemies.Count}");
        }

        public void AddEnemy(EnemyController enemy)
        {
            if (!enemies.Contains(enemy))
            {
                enemies.Add(enemy);
                enemy.OnPlayerDetected += HandlePlayerDetected;
            }
        }

        public void RemoveEnemy(EnemyController enemy)
        {
            if (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
                enemy.OnPlayerDetected -= HandlePlayerDetected;
            }
        }

        public void ResetAllEnemies()
        {
            foreach (EnemyController enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.ResetDetection();
                }
            }
        }
    }
}