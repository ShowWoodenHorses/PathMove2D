using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core
{
    public class DrawLineController : MonoBehaviour
    {
        [SerializeField] private DrawLineSetupConfig config;
        [SerializeField] private LineManager lineManager;
        [SerializeField] private FuelSystem fuelSystem;
        [SerializeField] private CollisionDetector collisionDetector;
        [SerializeField] private DrawingHandler drawingHandler;
        [SerializeField] private PlayerMovement playerMovement;

        [Space, Header("References")]
        [SerializeField] private Transform player;

        [Space, Header("UI Buttons")]
        public Button startButton;
        public Button resetButton;

        private Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;

            lineManager.Initialize(config);
            fuelSystem.Initialize(config, OnFuelChanged);
            collisionDetector.Initialize(config);
            playerMovement.Initialize(player, fuelSystem, lineManager, config);
            drawingHandler.Initialize(mainCamera, player, lineManager, collisionDetector, config);

            startButton.onClick.AddListener(() => playerMovement.TryStartMoving());
            resetButton.onClick.AddListener(ResetLine);
        }

        void Update()
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

            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetLine();
            }
        }

        private void OnFuelChanged(float currentFuel)
        {
            if (lineManager.AllPoints.Count > 0 && !playerMovement.IsMoving && !drawingHandler.IsDrawing)
            {
                lineManager.SplitPointsByFuelAvailability(fuelSystem.MaxFuelDistance);
                lineManager.UpdateLines();
            }
        }

        private void ResetLine()
        {
            playerMovement.StopMovement();
            drawingHandler.CancelDrawing();
            lineManager.ResetLine();
            Debug.Log("Линия сброшена");
        }
    }
}