using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class PickUpCargoZone : ZoneController
    {
        [SerializeField] private float startTime;

        private float currentTime;
        public override void Initialize(Transform player, Action onFinished)
        {
            base.Initialize(player, onFinished);
            currentTime = startTime;

        }
        protected override void DetectPlayer()
        {
            if (Vector3.Distance(transform.position, player.position) < 1f)
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0f && !isFinishSendAction)
                {
                    isFinishSendAction = true;
                    onFinished?.Invoke();
                    Destroy(gameObject);
                }

            }
            else
            {
                currentTime = startTime;
                isFinishSendAction = false;
                return;
            }
        }

    }
}