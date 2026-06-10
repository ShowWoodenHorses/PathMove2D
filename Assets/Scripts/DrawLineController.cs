using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core
{
    public class DrawLineController : MonoBehaviour
    {
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
        }

        void Update()
        {
            inputHandler.Update();
        }

        public void StartMoving()
        {
            playerMovement.TryStartMoving();
        }

        public void ResetLine()
        {
            playerMovement.StopMovement();
            drawingHandler.CancelDrawing();
            lineManager.ResetLine();
            Debug.Log("Линия сброшена");
        }

        private void OnFuelChanged(float currentFuel)
        {
            if (lineManager.AllPoints.Count > 0 && !playerMovement.IsMoving && !drawingHandler.IsDrawing)
            {
                lineManager.SplitPointsByFuelAvailability(fuelSystem.MaxFuelDistance);
                lineManager.UpdateLines();
            }
        }

        private void OnDestroy()
        {
            fuelSystem.UnregisterActionFuelChanged(OnFuelChanged);
        }
    }
}