using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelHolder : MonoBehaviour
    {
        [SerializeField] Transform playerStartPos;
        [SerializeField] private FinishZone finishZone;
        [SerializeField] private List<EnemyHolder> enemies = new();

        private Transform player;
        private Action onFinishedLevel;

        public List<EnemyHolder> Enemies => enemies;
        public void Initialize(Transform player, Action onFinishedLevel)
        {
            Debug.Log("StartLevel");
            this.player = player;
            this.onFinishedLevel = onFinishedLevel;

            this.player.transform.position = playerStartPos.position;
            finishZone.Initialize(this.player, OnFinishedLevel);
        }

        private void OnFinishedLevel()
        {
            onFinishedLevel?.Invoke();
        }
    }

    [Serializable]
    public class EnemyHolder
    {
        public GameObject Enemy;
        public List<Transform> Waypoints;
    }
}