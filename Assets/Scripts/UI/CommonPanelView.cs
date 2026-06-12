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
        [SerializeField] private WinPanelView winPanelView;

        [Space, Header("Lose Panel")]
        [SerializeField] private GameObject losePanel;

        [Space, Header("Pause Panel")]
        [SerializeField] private PausePanelView pausePanelView;

        [Space, Header("Fuel Panel")]
        [SerializeField] private GameObject fuelPanel;
        [SerializeField] private Slider fuelSlider;

        [Space, Header("Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button pauseButton;

        private DrawLineController drawLineController;
        private LevelController levelController;
        private FuelSystem fuelSystem;

        public void Initialize(DrawLineController drawLineController, LevelController levelController, FuelSystem fuelSystem)
        {
            this.drawLineController = drawLineController;
            this.levelController = levelController;
            this.fuelSystem = fuelSystem;

            pausePanelView.Initialize(this.levelController, OnClosePausePanel);
            winPanelView.Initialize(this.levelController);

            this.fuelSystem.RegisterActionFuelChanged(this.OnFuelChanged);
            this.levelController.RegisterActionLevelController(OnPickUpCargo, OnStartLevel, OnFinishedLevel);
            startButton.onClick.AddListener(OnStartButton);
            resetButton.onClick.AddListener(OnResetButton);
            pauseButton.onClick.AddListener(OnPauseButton);

            Setup();
        }

        private void Setup()
        {
            winPanelView.Close();
            losePanel.SetActive(false);
            pausePanelView.Close();
        }

        private void OnStartButton()
        {
            drawLineController.StartMoving();
        }
        private void OnResetButton()
        {
            drawLineController.ResetLine();
        }
        private void OnPauseButton()
        {
            pauseButton.gameObject.SetActive(false);
            pausePanelView.Open();
        }

        private void OnFuelChanged(float currentFuel)
        {
            if (currentFuel > fuelSlider.maxValue)
            {
                fuelSlider.maxValue = currentFuel;
            }

            fuelSlider.value = currentFuel;
        }

        private void OnStartLevel(LevelSetup levelSetup)
        {
            Setup();
            fuelSlider.maxValue = levelSetup.MaxFuel;
            fuelSlider.value = levelSetup.MaxFuel;
        }

        private void OnFinishedLevel()
        {
            winPanelView.Open();
        }
        private void OnPickUpCargo()
        {

        }

        private void OnClosePausePanel()
        {
            pauseButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            fuelSystem.UnregisterActionFuelChanged(OnFuelChanged);
        }
    }
}