using System;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class FinishZone : MonoBehaviour
    {
        private Transform player;
        private Action onFinished;
        private bool isFinishSendAction;

        public void Initialize(Transform player, Action onFinished)
        {
            this.player = player;
            this.onFinished = onFinished;
        }

        private void Update()
        {
            if (isFinishSendAction)
                return;

            DetectPlayer();
        }

        private void DetectPlayer()
        {
            if (Vector3.Distance(transform.position, player.position) < 1f)
            {
                isFinishSendAction = true;
                onFinished?.Invoke();
            }
        }
    }
}