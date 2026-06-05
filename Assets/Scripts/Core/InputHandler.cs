using UnityEngine;

namespace Assets.Scripts.Core
{
    public class InputHandler
    {
        private PlayerMovement playerMovement;
        private DrawingHandler drawingHandler;
        private FuelSystem fuelSystem;

        public InputHandler(PlayerMovement playerMovement, DrawingHandler drawingHandler, FuelSystem fuelSystem)
        {
            this.playerMovement = playerMovement;
            this.drawingHandler = drawingHandler;
            this.fuelSystem = fuelSystem;
        }
        public void Update()
        {

            if (playerMovement.IsMoving) return;

            if (Input.GetMouseButtonDown(0))
            {
                drawingHandler.TryStartDrawing();
            }

            if (drawingHandler.IsDrawing && Input.GetMouseButton(0))
            {
                drawingHandler.UpdateDrawing(fuelSystem.MaxFuelDistance);
            }

            if (Input.GetMouseButtonUp(0))
            {
                drawingHandler.EndDrawing();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                playerMovement.TryStartMoving();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                fuelSystem.AddFuel(10f);
            }
        }
    }
}