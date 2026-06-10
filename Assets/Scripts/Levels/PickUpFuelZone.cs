using System;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class PickUpFuelZone : ZoneController
    {
        [SerializeField] private float countFuel;

        private Action<float> onPickUpFuel;
        public void Initialize(Transform player, Action<float> onPickUpFuel)
        {
            this.player = player;
            this.onPickUpFuel = onPickUpFuel;
        }
        protected override void DetectPlayer()
        {
            if (Vector3.Distance(transform.position, player.position) < 1f)
            {
                if (!isFinishSendAction)
                {
                    isFinishSendAction = true;
                    onPickUpFuel?.Invoke(countFuel);
                    Destroy(gameObject);
                }
            }
        }
    }
}