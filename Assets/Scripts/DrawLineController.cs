using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core
{
    public class DrawLineController : MonoBehaviour
    {
        [Space, Header("UI Buttons")]
        public Button startButton;
        public Button resetButton;

        private LineManager lineManager;
        private DrawingHandler drawingHandler;
        private FuelSystem fuelSystem;
        private PlayerMovement playerMovement;
        private InputHandler inputHandler;

        public void Initialize(FuelSystem fuelSystem, PlayerMovement playerMovement, InputHandler inputHandler, LineManager lineManager, DrawingHandler drawingHandler)
        {
            this.fuelSystem = fuelSystem;
            this.playerMovement = playerMovement;
            this.inputHandler = inputHandler;
            this.lineManager = lineManager;
            this.drawingHandler = drawingHandler;

            fuelSystem.RegisterActionFuelChanged(OnFuelChanged);

            startButton.onClick.AddListener(() => playerMovement.TryStartMoving());
            resetButton.onClick.AddListener(ResetLine);
        }

        void Update()
        {
            inputHandler.Update();
        }

        private void OnFuelChanged(float currentFuel)
        {
            if (lineManager.AllPoints.Count > 0 && !playerMovement.IsMoving && !drawingHandler.IsDrawing)
            {
                lineManager.SplitPointsByFuelAvailability(fuelSystem.MaxFuelDistance);
                lineManager.UpdateLines();
            }
        }

        public void ResetLine()
        {
            playerMovement.StopMovement();
            drawingHandler.CancelDrawing();
            lineManager.ResetLine();
            Debug.Log("Линия сброшена");
        }
    }
}