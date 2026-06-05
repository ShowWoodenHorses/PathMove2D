using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Configs;

namespace Assets.Scripts.Core
{
    public class LineManager : MonoBehaviour
    {
        private GameObject linePrefab;
        private float minDistanceBetweenPoints;
        private float lineWidth;

        private Color availableFuelColor;
        private Color unavailableFuelColor ;

        private LineRenderer availableLine;
        private LineRenderer unavailableLine;
        private List<Vector3> allPoints = new List<Vector3>();
        private List<Vector3> availablePoints = new List<Vector3>();
        private List<Vector3> unavailablePoints = new List<Vector3>();

        public List<Vector3> AllPoints => allPoints;
        public List<Vector3> AvailablePoints => availablePoints;
        public List<Vector3> UnavailablePoints => unavailablePoints;
        public float TotalLineLength { get; private set; }

        public void Initialize(DrawLineSetupConfig config)
        {
            linePrefab = config.linePrefab;
            minDistanceBetweenPoints = config.minDistanceBetweenPoints;
            lineWidth = config.lineWidth;

            availableFuelColor = config.availableFuelColor;
            unavailableFuelColor = config.unavailableFuelColor;

            CreateLineRenderers();
            SetLinesEnabled(false);

            if (availableLine != null) availableLine.transform.SetParent(transform);
            if (unavailableLine != null) unavailableLine.transform.SetParent(transform);
        }

        public void StartNewLine(Vector3 startPoint)
        {
            allPoints.Clear();
            availablePoints.Clear();
            unavailablePoints.Clear();
            TotalLineLength = 0f;
            SetLinesEnabled(true);
            ClearLines();
            AddPoint(startPoint);
        }

        public bool TryAddPoint(Vector3 newPoint)
        {
            if (allPoints.Count > 0 && Vector3.Distance(allPoints.Last(), newPoint) >= minDistanceBetweenPoints)
            {
                float newSegmentLength = Vector3.Distance(allPoints.Last(), newPoint);
                AddPoint(newPoint);
                TotalLineLength += newSegmentLength;
                return true;
            }
            return false;
        }

        public void SplitPointsByFuelAvailability(float maxFuelDistance)
        {
            availablePoints.Clear();
            unavailablePoints.Clear();

            if (allPoints.Count == 0) return;

            float accumulatedDistance = 0f;
            bool isFuelAvailable = true;

            for (int i = 0; i < allPoints.Count; i++)
            {
                if (isFuelAvailable)
                {
                    availablePoints.Add(allPoints[i]);

                    if (i < allPoints.Count - 1)
                    {
                        float nextSegmentLength = Vector3.Distance(allPoints[i], allPoints[i + 1]);
                        if (accumulatedDistance + nextSegmentLength > maxFuelDistance)
                        {
                            float remainingDistance = maxFuelDistance - accumulatedDistance;
                            if (remainingDistance > 0 && remainingDistance < nextSegmentLength)
                            {
                                Vector3 boundaryPoint = Vector3.MoveTowards(allPoints[i], allPoints[i + 1], remainingDistance);
                                availablePoints.Add(boundaryPoint);
                                unavailablePoints.Add(boundaryPoint);
                            }
                            isFuelAvailable = false;
                        }
                        accumulatedDistance += nextSegmentLength;
                    }
                }
                else
                {
                    unavailablePoints.Add(allPoints[i]);
                }
            }

            if (availablePoints.Count > 0 && unavailablePoints.Count > 0 && availablePoints.Last() != unavailablePoints.First())
            {
                unavailablePoints.Insert(0, availablePoints.Last());
            }
        }

        public void UpdateLines()
        {
            if (availableLine != null)
            {
                availableLine.positionCount = availablePoints.Count;
                if (availablePoints.Count > 0)
                    availableLine.SetPositions(availablePoints.ToArray());
            }

            if (unavailableLine != null)
            {
                unavailableLine.positionCount = unavailablePoints.Count;
                if (unavailablePoints.Count > 0)
                    unavailableLine.SetPositions(unavailablePoints.ToArray());
            }
        }

        public void SetPathForMovement(List<Vector3> path)
        {
            availablePoints.Clear();
            availablePoints.AddRange(path);
            unavailablePoints.Clear();
            UpdateLines();
        }

        public void ResetLine()
        {
            allPoints.Clear();
            availablePoints.Clear();
            unavailablePoints.Clear();
            TotalLineLength = 0f;
            SetLinesEnabled(false);
            ClearLines();
        }

        public void SetLinesEnabled(bool enabled)
        {
            if (availableLine != null) availableLine.enabled = enabled;
            if (unavailableLine != null) unavailableLine.enabled = enabled;
        }

        private void AddPoint(Vector3 point)
        {
            allPoints.Add(point);
        }

        private void ClearLines()
        {
            if (availableLine != null) availableLine.positionCount = 0;
            if (unavailableLine != null) unavailableLine.positionCount = 0;
        }

        private void CreateLineRenderers()
        {
            if (linePrefab != null)
            {
                GameObject availableObj = Instantiate(linePrefab);
                availableLine = availableObj.GetComponent<LineRenderer>();
                availableObj.transform.SetParent(transform);
                availableObj.name = "AvailableLine";
            }
            else
            {
                GameObject availableObj = new GameObject("AvailableLine");
                availableLine = availableObj.AddComponent<LineRenderer>();
                availableLine.startWidth = lineWidth;
                availableLine.endWidth = lineWidth;
                availableLine.material = new Material(Shader.Find("Sprites/Default"));
                availableLine.sortingOrder = 1;
            }
            availableLine.startColor = availableFuelColor;
            availableLine.endColor = availableFuelColor;
            availableLine.useWorldSpace = true;

            if (linePrefab != null)
            {
                GameObject unavailableObj = Instantiate(linePrefab);
                unavailableLine = unavailableObj.GetComponent<LineRenderer>();
                unavailableObj.name = "UnavailableLine";
            }
            else
            {
                GameObject unavailableObj = new GameObject("UnavailableLine");
                unavailableLine = unavailableObj.AddComponent<LineRenderer>();
                unavailableLine.startWidth = lineWidth;
                unavailableLine.endWidth = lineWidth;
                unavailableLine.material = new Material(Shader.Find("Sprites/Default"));
                unavailableLine.sortingOrder = 0;
            }
            unavailableLine.startColor = unavailableFuelColor;
            unavailableLine.endColor = unavailableFuelColor;
            unavailableLine.useWorldSpace = true;
        }
    }
}