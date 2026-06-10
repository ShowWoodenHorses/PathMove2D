using Assets.Scripts.Core;
using Assets.Scripts.Configs;
using UnityEngine;
using Assets.Scripts.Enemies;
using Assets.Scripts.UI;

namespace Assets.Scripts
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Transform player;
        [SerializeField] private DrawLineSetupConfig config;
        [SerializeField] private LineManager lineManager;
        [SerializeField] private DrawLineController drawLineController;

        [Space, Header("Level")]
        [SerializeField] private LevelConfig levelConfig;
        [SerializeField] private LevelController levelController;

        [Space, Header("Enemy")]
        [SerializeField] private EnemyManager enemyManager;

        [Space, Header("UI")]
        [SerializeField] private CommonPanelView commonPanelView;


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

            levelController.Initialize(levelConfig, enemyManager,fuelSystem, player);
            commonPanelView.Initialize(drawLineController, levelController, fuelSystem);
        }
    }
}