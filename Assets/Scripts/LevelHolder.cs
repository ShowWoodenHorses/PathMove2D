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
        [SerializeField] private PickUpCargoZone cargoZone;
        [SerializeField] private FinishZone finishZone;
        [SerializeField] private List<PickUpFuelZone> fuelZones;
        [SerializeField] private List<EnemyHolder> enemies = new();

        private Transform player;
        private Action onPickUpCargo;
        private Action<float> onPickUpFuel;
        private Action onFinishedLevel;

        public List<EnemyHolder> Enemies => enemies;
        public void Initialize(Transform player, Action onPickUpCargo, Action<float> onPickUpFuel, Action onFinishedLevel)
        {
            Debug.Log("StartLevel");
            this.player = player;
            this.onPickUpCargo = onPickUpCargo;
            this.onPickUpFuel = onPickUpFuel;
            this.onFinishedLevel = onFinishedLevel;

            this.player.transform.position = playerStartPos.position;
            finishZone.Initialize(this.player, OnFinishedLevel);
            cargoZone.Initialize(this.player, OnPickUpCargo);
            
            if (fuelZones.Count > 0)
            {
                fuelZones.ForEach(f => f.Initialize(this.player, OnPickUpFuel));
            }
        }

        private void OnFinishedLevel()
        {
            onFinishedLevel?.Invoke();
        }
        private void OnPickUpCargo()
        {
            onPickUpCargo?.Invoke();
        }
        private void OnPickUpFuel(float countFuel)
        {
            onPickUpFuel?.Invoke(countFuel);
        }
    }

    [Serializable]
    public class EnemyHolder
    {
        public GameObject Enemy;
        public List<Transform> Waypoints;
    }
}