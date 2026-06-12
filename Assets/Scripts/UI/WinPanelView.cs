using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class WinPanelView : MonoBehaviour
    {
        [SerializeField] private Button restartLevelButton;

        private LevelController levelController;

        public void Initialize(LevelController levelController)
        {
            this.levelController = levelController;
            gameObject.SetActive(false);

            restartLevelButton.onClick.AddListener(OnRestartLevel);
        }
        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnRestartLevel()
        {
            levelController.RestartLevel();
        }
    }
}