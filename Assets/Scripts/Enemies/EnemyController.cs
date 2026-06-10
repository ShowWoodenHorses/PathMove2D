using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public List<Transform> waypoints;
        public float moveSpeed = 3f;
        public float waypointReachDistance = 0.1f;
        public bool loopPath = true;
        public bool rotateTowardsMovement = true;
        public float rotationSpeed = 360f;

        [Header("Detection Settings")]
        public float detectionRadius = 5f;
        public float detectionAngle = 360f;
        public GameObject detectionVisualPrefab;
        public LayerMask playerLayer;
        public LayerMask obstacleMask;
        public bool requireLineOfSight = true;

        [Header("Visual Settings")]
        public Color detectionGizmoColor = new Color(1f, 0f, 0f, 0.3f);
        public bool showGizmos = true;

        private int currentWaypointIndex = 0;
        private Transform player;
        private bool hasDetectedPlayer = false;
        private bool canMovePatrol = true;
        private DetectionVisual detectionVisual;
        private Vector3 lastPosition;

        private Action<EnemyController> onPlayerDetected;

        public void Initialize(Transform player, List<Transform> points, Action<EnemyController> onPlayerDetected)
        {
            this.player = player;
            this.waypoints = points;
            this.onPlayerDetected = onPlayerDetected;

            if (waypoints.Count == 0)
            {
                canMovePatrol = false;
            }
            else
            {
                transform.position = waypoints[0].position;
                currentWaypointIndex = 1;
                lastPosition = transform.position;
            }

            InitializeDetectionVisual();
        }

        void InitializeDetectionVisual()
        {
            if (detectionVisualPrefab != null)
            {
                GameObject visualObj = Instantiate(detectionVisualPrefab, transform);
                detectionVisual = visualObj.GetComponent<DetectionVisual>();
                if (detectionVisual != null)
                {
                    detectionVisual.Initialize(this);
                }
            }
        }

        void Update()
        {
            if (canMovePatrol)
            {
                PatrolMovement();
            }

            if (player != null && !hasDetectedPlayer)
            {
                DetectPlayer();
            }
        }

        void PatrolMovement()
        {
            if (waypoints.Count == 0) return;

            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            Vector3 previousPosition = transform.position;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (rotateTowardsMovement)
            {
                RotateTowardsMovement(previousPosition);
            }

            if (Vector3.Distance(transform.position, targetPosition) <= waypointReachDistance)
            {
                UpdateNextWaypoint();
            }
        }

        void RotateTowardsMovement(Vector3 previousPosition)
        {
            Vector3 movementDirection = transform.position - previousPosition;

            if (movementDirection.sqrMagnitude > 0.001f)
            {
                float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;

                Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
            }
        }

        void UpdateNextWaypoint()
        {
            if (currentWaypointIndex + 1 < waypoints.Count)
            {
                currentWaypointIndex++;
            }
            else if (loopPath)
            {
                currentWaypointIndex = 0;
            }
        }

        void DetectPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > detectionRadius)
                return;

            if (detectionAngle < 360f)
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                float angleToPlayer = Vector3.Angle(transform.right, directionToPlayer);

                if (angleToPlayer > detectionAngle / 2f)
                    return;
            }

            if (requireLineOfSight)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                float distance = distanceToPlayer;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleMask);

                if (hit.collider != null)
                    return;
            }

            hasDetectedPlayer = true;
            onPlayerDetected?.Invoke(this);
            Debug.Log($"[Enemy] {name} обнаружил игрока!");
        }

        public float GetDetectionRadius() => detectionRadius;
        public float GetDetectionAngle() => detectionAngle;
        public bool IsUsingFullCircle() => detectionAngle >= 360f;
        public Transform GetTransform() => transform;

        public void ResetDetection()
        {
            hasDetectedPlayer = false;
        }

        void OnDrawGizmos()
        {
            if (!showGizmos) return;

            if (detectionAngle >= 360f)
            {
                Gizmos.color = detectionGizmoColor;
                Gizmos.DrawWireSphere(transform.position, detectionRadius);
            }
            else
            {
                DrawDetectionCone();
            }
        }

        void DrawDetectionCone()
        {
            float halfAngle = detectionAngle / 2f;
            float startAngle = transform.eulerAngles.z - halfAngle;
            float endAngle = transform.eulerAngles.z + halfAngle;

            Vector3 startDir = AngleToDirection(startAngle);
            Vector3 endDir = AngleToDirection(endAngle);

            Vector3 leftEdge = transform.position + startDir * detectionRadius;
            Vector3 rightEdge = transform.position + endDir * detectionRadius;

            Gizmos.color = detectionGizmoColor;
            Gizmos.DrawLine(transform.position, leftEdge);
            Gizmos.DrawLine(transform.position, rightEdge);

            int segments = 20;
            Vector3 prevPoint = transform.position + startDir * detectionRadius;
            for (int i = 1; i <= segments; i++)
            {
                float angle = startAngle + (detectionAngle * i / segments);
                Vector3 dir = AngleToDirection(angle);
                Vector3 point = transform.position + dir * detectionRadius;
                Gizmos.DrawLine(prevPoint, point);
                prevPoint = point;
            }
        }

        Vector3 AngleToDirection(float angleDegrees)
        {
            float angleRad = angleDegrees * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0);
        }
    }
}