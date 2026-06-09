using UnityEngine;

namespace Assets.Scripts.Enemy
{

    [RequireComponent(typeof(SpriteRenderer))]
    public class DetectionVisual : MonoBehaviour
    {
        [Header("Visual Settings")]
        public Color fullCircleColor = new Color(1f, 0f, 0f, 0.2f);
        public Color coneColor = new Color(1f, 0f, 0f, 0.15f);
        public Color outlineColor = new Color(1f, 0f, 0f, 0.5f);
        public float updateInterval = 0.05f;

        private SpriteRenderer spriteRenderer;
        private EnemyController enemy;
        private float currentRadius;
        private float currentAngle;
        private Texture2D detectionTexture;
        private Material dynamicMaterial;
        private float lastUpdateTime;

        public void Initialize(EnemyController enemyRef)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }

            enemy = enemyRef;
            currentRadius = enemy.GetDetectionRadius();
            currentAngle = enemy.GetDetectionAngle();
            UpdateDetectionVisual();
        }

        void LateUpdate()
        {
            if (enemy == null)
            {
                Destroy(gameObject);
                return;
            }

            if (Time.time - lastUpdateTime >= updateInterval)
            {
                lastUpdateTime = Time.time;

                float newRadius = enemy.GetDetectionRadius();
                float newAngle = enemy.GetDetectionAngle();

                if (Mathf.Abs(currentRadius - newRadius) > 0.01f || Mathf.Abs(currentAngle - newAngle) > 0.01f)
                {
                    currentRadius = newRadius;
                    currentAngle = newAngle;
                    UpdateDetectionVisual();
                }

                transform.position = enemy.GetTransform().position;
                transform.rotation = enemy.GetTransform().rotation;
            }
        }

        void UpdateDetectionVisual()
        {
            if (enemy.IsUsingFullCircle())
            {
                CreateCircleSprite();
            }
            else
            {
                CreateConeSprite();
            }
        }

        void CreateCircleSprite()
        {
            int textureSize = Mathf.CeilToInt(currentRadius * 20f);
            textureSize = Mathf.Clamp(textureSize, 32, 512);

            detectionTexture = new Texture2D(textureSize, textureSize);
            Color transparent = new Color(0, 0, 0, 0);

            for (int x = 0; x < textureSize; x++)
            {
                for (int y = 0; y < textureSize; y++)
                {
                    detectionTexture.SetPixel(x, y, transparent);
                }
            }

            Vector2 center = new Vector2(textureSize / 2f, textureSize / 2f);
            float radiusInPixels = currentRadius * (textureSize / (currentRadius * 2f));

            for (int x = 0; x < textureSize; x++)
            {
                for (int y = 0; y < textureSize; y++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center);
                    if (distance <= radiusInPixels)
                    {
                        float alpha = fullCircleColor.a * (1f - (distance / radiusInPixels) * 0.5f);
                        Color pixelColor = new Color(fullCircleColor.r, fullCircleColor.g, fullCircleColor.b, alpha);
                        detectionTexture.SetPixel(x, y, pixelColor);

                        if (distance > radiusInPixels - 2f && distance <= radiusInPixels)
                        {
                            detectionTexture.SetPixel(x, y, outlineColor);
                        }
                    }
                }
            }

            detectionTexture.Apply();

            Sprite sprite = Sprite.Create(detectionTexture, new Rect(0, 0, textureSize, textureSize), new Vector2(0.5f, 0.5f), textureSize / (currentRadius * 2f));
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = Color.white;
        }

        void CreateConeSprite()
        {
            int textureSize = Mathf.CeilToInt(currentRadius * 20f);
            textureSize = Mathf.Clamp(textureSize, 64, 512);

            detectionTexture = new Texture2D(textureSize, textureSize);
            Color transparent = new Color(0, 0, 0, 0);

            for (int x = 0; x < textureSize; x++)
            {
                for (int y = 0; y < textureSize; y++)
                {
                    detectionTexture.SetPixel(x, y, transparent);
                }
            }

            Vector2 center = new Vector2(textureSize / 2f, textureSize / 2f);
            float radiusInPixels = currentRadius * (textureSize / (currentRadius * 2f));
            float halfAngleRad = currentAngle / 2f * Mathf.Deg2Rad;

            for (int x = 0; x < textureSize; x++)
            {
                for (int y = 0; y < textureSize; y++)
                {
                    Vector2 pixelPos = new Vector2(x, y) - center;
                    float distance = pixelPos.magnitude;

                    if (distance <= radiusInPixels && distance > 0)
                    {
                        float angle = Mathf.Atan2(pixelPos.y, pixelPos.x);

                        if (Mathf.Abs(angle) <= halfAngleRad)
                        {
                            float alpha = coneColor.a * (1f - (distance / radiusInPixels) * 0.5f);
                            Color pixelColor = new Color(coneColor.r, coneColor.g, coneColor.b, alpha);
                            detectionTexture.SetPixel(x, y, pixelColor);

                            if (distance > radiusInPixels - 2f || Mathf.Abs(Mathf.Abs(angle) - halfAngleRad) < 0.05f)
                            {
                                detectionTexture.SetPixel(x, y, outlineColor);
                            }
                        }
                    }
                }
            }

            detectionTexture.Apply();

            Sprite sprite = Sprite.Create(detectionTexture, new Rect(0, 0, textureSize, textureSize), new Vector2(0.5f, 0.5f), textureSize / (currentRadius * 2f));
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = Color.white;
        }

        void OnDestroy()
        {
            if (detectionTexture != null)
            {
                Destroy(detectionTexture);
            }
            if (dynamicMaterial != null)
            {
                Destroy(dynamicMaterial);
            }
        }
    }
}