using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "DrawLineSetupConfig", menuName = "ScriptableObject/DrawLineSetupConfig")]
    public class DrawLineSetupConfig : ScriptableObject
    {
        [Header("Line Settings")]
        public GameObject linePrefab;
        public float minDistanceBetweenPoints = 0.1f;
        public int frameStepForNewPoint = 5;
        public float lineWidth = 0.1f;

        [Header("Colors")]
        public Color availableFuelColor = Color.green;
        public Color unavailableFuelColor = Color.red;

        [Header("Fuel System")]
        public float maxFuel = 100f;
        public float fuelConsumptionRate = 1f;
        public Image fuelBar;

        [Header("Collision Settings")]
        public LayerMask obstacleMask;
        public float collisionCheckRadius = 0.2f;
        public Vector2 boundsSize = new Vector2(20f, 12f);
        public Vector3 boundsCenter = Vector3.zero;

        [Header("Movement Settings")]
        public float moveSpeed = 5f;

        [Header("Drawing Settings")]
        public float startRadius = 0.5f;
    }
}