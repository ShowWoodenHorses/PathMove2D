using System;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class ZoneController : MonoBehaviour
    {
        protected Transform player;
        protected Action onFinished;
        protected bool isFinishSendAction;

        public virtual void Initialize(Transform player, Action onFinished)
        {
            this.player = player;
            this.onFinished = onFinished;
        }

        protected void Update()
        {
            DetectPlayer();
        }

        protected virtual void DetectPlayer()
        {
            if (Vector3.Distance(transform.position, player.position) < 1f)
            {
                if (!isFinishSendAction)
                {
                    isFinishSendAction = true;
                    onFinished?.Invoke();
                }
            }
            else
            {
                isFinishSendAction = false;
                return;
            }
        }
    }
}