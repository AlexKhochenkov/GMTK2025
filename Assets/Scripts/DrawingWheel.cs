using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D))]
public class TexturedRing : MonoBehaviour
{
    [SerializeField] private Sprite ringSprite;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float scaling = 2f;

    void Start()
    {
        if (ringSprite == null)
        {
            Debug.LogError($"Sprite not found");
            return;
        }

        // Настройка рендерера
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = ringSprite;
        renderer.drawMode = SpriteDrawMode.Simple; // Изменено на Simple

        // Генерация коллайдера
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        collider.pathCount = ringSprite.GetPhysicsShapeCount();

        List<Vector2> path = new();
        for (int i = 0; i < collider.pathCount; i++)
        {
            ringSprite.GetPhysicsShape(i, path);
            collider.SetPath(i, path.ToArray());
        }
        transform.localScale = new Vector3(transform.localScale.x * scaling, transform.localScale.y * scaling, transform.localScale.z * scaling);
    }

    void Update()
    {
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
}