using UnityEngine;
using System.Linq;
using Assets.Scripts.Configs;

namespace Assets.Scripts.Core
{
    public class DrawingHandler
    {
        private Transform player;
        private Camera mainCamera;
        private LineManager lineManager;
        private CollisionDetector collisionDetector;
        private DrawLineSetupConfig config;

        private bool isDrawing = false;
        private int frameCounter = 0;
        private bool hasStartedDrawingFromPlayer = false;

        public bool IsDrawing => isDrawing;
        public bool HasStartedFromPlayer => hasStartedDrawingFromPlayer;

        public void Initialize(Camera cam, Transform player, LineManager lineManager, CollisionDetector collisionDetector, DrawLineSetupConfig config)
        {
            mainCamera = cam;
            this.player = player;
            this.lineManager = lineManager;
            this.collisionDetector = collisionDetector;
            this.config = config;
        }

        public void TryStartDrawing()
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            float distanceToPlayer = Vector3.Distance(mousePosition, player.position);

            if (distanceToPlayer > config.startRadius)
            {
                Debug.Log("Рисование должно начинаться от игрока!");
                return;
            }

            isDrawing = true;
            hasStartedDrawingFromPlayer = true;
            frameCounter = 0;
            lineManager.StartNewLine(player.position);
        }

        public void UpdateDrawing(float maxFuelDistance)
        {
            if (!isDrawing) return;

            frameCounter++;
            if (frameCounter < config.frameStepForNewPoint) return;
            frameCounter = 0;

            Vector3 newPoint = GetMouseWorldPosition();

            if (collisionDetector.IsOutOfBounds(newPoint))
            {
                CancelDrawing();
                return;
            }

            if (collisionDetector.HasCollisionWithObstacle(newPoint))
            {
                CancelDrawing();
                return;
            }

            if (lineManager.AllPoints.Count > 0 && collisionDetector.DoesLineIntersectObstacle(lineManager.AllPoints.Last(), newPoint))
            {
                CancelDrawing();
                return;
            }

            if (lineManager.TryAddPoint(newPoint))
            {
                lineManager.SplitPointsByFuelAvailability(maxFuelDistance);
                lineManager.UpdateLines();
            }
        }

        public void EndDrawing()
        {
            isDrawing = false;
            hasStartedDrawingFromPlayer = false;

            if (lineManager.AllPoints.Count < 2)
            {
                lineManager.ResetLine();
            }
        }

        public void CancelDrawing()
        {
            isDrawing = false;
            hasStartedDrawingFromPlayer = false;
            lineManager.ResetLine();
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -mainCamera.transform.position.z;
            return mainCamera.ScreenToWorldPoint(mousePos);
        }
    }
}