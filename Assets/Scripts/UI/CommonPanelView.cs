using System.Collections;
using Assets.Scripts.Configs;
using Assets.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class CommonPanelView : MonoBehaviour
    {
        [Space, Header("Win Panel")]
        [SerializeField] private GameObject winPanel;

        [Space, Header("Lose Panel")]
        [SerializeField] private GameObject losePanel;

        [Space, Header("Pause Panel")]
        [SerializeField] private GameObject pausePanel;

        [Space, Header("Fuel Panel")]
        [SerializeField] private GameObject fuelPanel;
        [SerializeField] private Slider fuelSlider;

        [Space, Header("Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button resetButton;

        private DrawLineSetupConfig config;
        private DrawLineController drawLineController;
        private LevelController levelController;
        private FuelSystem fuelSystem;

        public void Initialize(DrawLineSetupConfig config, DrawLineController drawLineController, LevelController levelController, FuelSystem fuelSystem)
        {
            this.config = config;
            this.drawLineController = drawLineController;
            this.levelController = levelController;
            this.fuelSystem = fuelSystem;

            winPanel.SetActive(false);
            losePanel.SetActive(false);
            pausePanel.SetActive(false);
            fuelPanel.SetActive(false);

            this.fuelSystem.RegisterActionFuelChanged(this.OnFuelChanged);
            this.levelController.RegisterActionLevelController(OnPickUpCargo, OnFinishedLevel);
            startButton.onClick.AddListener(OnStartButton);
            resetButton.onClick.AddListener(OnResetButton);

            SetFuelView();
        }

        private void SetFuelView()
        {
            fuelSlider.maxValue = config.maxFuel;
            fuelSlider.value = config.maxFuel;

            fuelPanel.SetActive(true);
        }

        private void OnStartButton()
        {
            drawLineController.StartMoving();
        }
        private void OnResetButton()
        {
            drawLineController.ResetLine();
        }

        private void OnFuelChanged(float currentFuel)
        {
            fuelSlider.value = currentFuel;
        }

        private void OnFinishedLevel()
        {
            winPanel.SetActive(true);
        }
        private void OnPickUpCargo()
        {

        }

        private void OnDestroy()
        {
            fuelSystem.UnregisterActionFuelChanged(OnFuelChanged);
        }
    }
}