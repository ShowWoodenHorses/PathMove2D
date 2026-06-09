using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Configs;

namespace Assets.Scripts.Core
{
    public class PlayerMovement
    {
        private Transform player;
        private FuelSystem fuelSystem;
        private LineManager lineManager;

        private float moveSpeed;
        private bool rotateTowardsMovement = true;
        private float rotationSpeed = 360f;
        private Vector3 lastPosition;

        private bool isMoving = false;
        private int currentPointIndex = 0;
        private List<Vector3> currentPath = new List<Vector3>();

        public bool IsMoving => isMoving;

        public PlayerMovement(Transform player, FuelSystem fuelSystem, LineManager lineManager)
        {
            this.player = player;
            this.fuelSystem = fuelSystem;
            this.lineManager = lineManager;
        }

        public void Initialize(DrawLineSetupConfig config)
        {
            moveSpeed = config.moveSpeed;
            rotateTowardsMovement = config.rotateTowardsMovement;
            rotationSpeed = config.rotationSpeed;
        }

        public void TryStartMoving()
        {
            if (isMoving) return;

            List<Vector3> allPoints = lineManager.AllPoints;
            if (allPoints.Count < 2) return;

            float totalLineLength = lineManager.TotalLineLength;
            float requiredFuel = fuelSystem.GetRequiredFuelForDistance(totalLineLength);

            if (fuelSystem.HasEnoughFuelForDistance(totalLineLength))
            {
                StartMoving(allPoints);
            }
            else
            {
                Debug.Log($"Недостаточно топлива! Нужно: {requiredFuel}, Есть: {fuelSystem.Currentfuel}");
                float possibleDistance = fuelSystem.GetPossibleDistance();
                List<Vector3> trimmedPoints = TrimLineToDistance(allPoints, possibleDistance);
                StartMoving(trimmedPoints);
            }
        }

        public void StopMovement()
        {
            if (isMoving)
            {
                player.DOKill();
                isMoving = false;
                currentPath.Clear();
            }
        }

        private List<Vector3> TrimLineToDistance(List<Vector3> points, float maxDistance)
        {
            if (points.Count < 2) return new List<Vector3>();

            List<Vector3> trimmedPoints = new List<Vector3>();
            trimmedPoints.Add(points[0]);

            float accumulatedDistance = 0f;

            for (int i = 1; i < points.Count; i++)
            {
                float segmentDistance = Vector3.Distance(points[i - 1], points[i]);

                if (accumulatedDistance + segmentDistance <= maxDistance)
                {
                    trimmedPoints.Add(points[i]);
                    accumulatedDistance += segmentDistance;
                }
                else
                {
                    float remainingDistance = maxDistance - accumulatedDistance;
                    Vector3 partialPoint = Vector3.MoveTowards(points[i - 1], points[i], remainingDistance);
                    trimmedPoints.Add(partialPoint);
                    break;
                }
            }

            Debug.Log($"Линия обрезана до {maxDistance} единиц из-за нехватки топлива");
            return trimmedPoints;
        }

        private void StartMoving(List<Vector3> path)
        {
            if (path.Count < 2) return;

            isMoving = true;
            currentPath = path;
            currentPointIndex = 1;
            lastPosition = player.position;

            lineManager.SetPathForMovement(path);
            MoveToNextPoint();
        }

        private void MoveToNextPoint()
        {
            if (currentPointIndex >= currentPath.Count)
            {
                CompleteMovement();
                return;
            }

            Vector3 targetPoint = currentPath[currentPointIndex];
            float segmentDistance = Vector3.Distance(player.position, targetPoint);
            float fuelForSegment = fuelSystem.GetRequiredFuelForDistance(segmentDistance);

            if (fuelSystem.Currentfuel >= fuelForSegment)
            {
                float duration = segmentDistance / moveSpeed;

                // Сохраняем начальную позицию перед движением
                Vector3 startPosition = player.position;

                player.DOMove(targetPoint, duration).SetEase(Ease.Linear).OnUpdate(() =>
                {
                    if (rotateTowardsMovement)
                    {
                        RotateTowardsMovement(startPosition, targetPoint);
                    }
                }).OnComplete(() =>
                {
                    fuelSystem.ConsumeFuelForDistance(segmentDistance);
                    currentPointIndex++;
                    MoveToNextPoint();
                });
            }
            else
            {
                float possibleDistance = fuelSystem.GetPossibleDistance();
                Vector3 partialTarget = Vector3.MoveTowards(player.position, targetPoint, possibleDistance);
                float duration = possibleDistance / moveSpeed;

                Vector3 startPosition = player.position;

                player.DOMove(partialTarget, duration).OnUpdate(() =>
                {
                    if (rotateTowardsMovement)
                    {
                        RotateTowardsMovement(startPosition, partialTarget);
                    }
                }).OnComplete(() =>
                {
                    Debug.Log("Топливо закончилось!");
                    CompleteMovement();
                });
            }
        }

        private void RotateTowardsMovement(Vector3 startPosition, Vector3 targetPosition)
        {
            Vector3 movementDirection = targetPosition - startPosition;

            if (movementDirection.sqrMagnitude > 0.001f)
            {
                float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
                player.rotation = Quaternion.RotateTowards(player.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        private void CompleteMovement()
        {
            isMoving = false;
            currentPath.Clear();
            lineManager.ResetLine();
        }
    }
}