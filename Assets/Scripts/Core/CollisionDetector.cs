using UnityEngine;

namespace Assets.Scripts.Core
{
    public class CollisionDetector
    {
        private LayerMask obstacleMask;
        private float collisionCheckRadius = 0.2f;
        private Vector2 boundsSize = new Vector2(20f, 12f);
        private Vector3 boundsCenter = Vector3.zero;
        private Bounds gameBounds;

        public CollisionDetector(DrawLineSetupConfig config)
        {
            obstacleMask = config.obstacleMask;
            collisionCheckRadius = config.collisionCheckRadius;
            boundsSize = config.boundsSize;
            boundsCenter = config.boundsCenter;

            gameBounds = new Bounds(boundsCenter, boundsSize);
        }

        public bool HasCollisionWithObstacle(Vector3 point)
        {
            return Physics2D.OverlapCircle(point, collisionCheckRadius, obstacleMask) != null;
        }

        public bool DoesLineIntersectObstacle(Vector3 from, Vector3 to)
        {
            Vector2 direction = to - from;
            float distance = direction.magnitude;
            RaycastHit2D hit = Physics2D.Raycast(from, direction, distance, obstacleMask);
            return hit.collider != null;
        }

        public bool IsOutOfBounds(Vector3 point)
        {
            return !gameBounds.Contains(point);
        }

        public bool IsValidPoint(Vector3 point)
        {
            return !HasCollisionWithObstacle(point) && !IsOutOfBounds(point);
        }
    }
}