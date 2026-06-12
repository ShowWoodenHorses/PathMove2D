using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class PausePanelView : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button restartLevelButton;

        private LevelController levelController;
        private Action onClose;

        public void Initialize(LevelController levelController, Action onClose)
        {
            this.levelController = levelController;
            this.onClose = onClose;
            gameObject.SetActive(false);

            closeButton.onClick.AddListener(Close);
            restartLevelButton.onClick.AddListener(OnRestartLevel);
        }
        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            onClose?.Invoke();
        }

        private void OnRestartLevel()
        {
            levelController.RestartLevel();
        }
    }
}