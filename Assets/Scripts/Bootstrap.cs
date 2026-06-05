using Assets.Scripts.Core;
using Assets.Scripts.Configs;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private DrawLineSetupConfig config;
        [SerializeField] private LineManager lineManager;
        [SerializeField] private DrawLineController drawLineController;

        [Space, Header("References")]
        [SerializeField] private Transform player;

        private Camera mainCamera;
        private FuelSystem fuelSystem;
        private CollisionDetector collisionDetector;
        private PlayerMovement playerMovement;
        private InputHandler inputHandler;
        private DrawingHandler drawingHandler;

        private void Start()
        {
            mainCamera = Camera.main;

            if (fuelSystem == null)
            {
                fuelSystem = new FuelSystem();
            }

            lineManager.Initialize(config);
            fuelSystem.Initialize(config);

            if (playerMovement == null)
            {
                playerMovement = new PlayerMovement(player, fuelSystem, lineManager);
            }

            if (collisionDetector == null)
            {
                collisionDetector = new CollisionDetector(config);
            }

            if (drawingHandler == null)
            {
                drawingHandler = new DrawingHandler();
            }

            playerMovement.Initialize(config);
            drawingHandler.Initialize(mainCamera, player, lineManager, collisionDetector, config);

            if (inputHandler == null)
            {
                inputHandler = new InputHandler(playerMovement, drawingHandler, fuelSystem);
            }

            drawLineController.Initialize(fuelSystem, playerMovement, inputHandler, lineManager, drawingHandler);
        }
    }
}