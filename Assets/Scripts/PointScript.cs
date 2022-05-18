using System;
using System.Linq;
using UnityEngine;

public class PointScript : MonoBehaviour
{
    public FieldScript fieldScript;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fieldScript = GameObject
            .FindGameObjectWithTag("MainCamera")
            .GetComponent<FieldScript>();
    }

    private void OnMouseOver()
    {
        var texture = Resources.Load<Texture2D>("Sprites/black_circle");
        var sprite = Sprite.Create(
            texture,
            new Rect(0.0f, 0.0f, texture.width, texture.height),
            new Vector2(0.32f, 0.32f));

        spriteRenderer.sprite = sprite;
    }

    private void OnMouseExit()
    {
        var texture = Resources.Load<Texture2D>("Sprites/gray_circle");
        var sprite = Sprite.Create(
            texture,
            new Rect(0.0f, 0.0f, texture.width, texture.height),
            new Vector2(0.32f, 0.32f));

        spriteRenderer.sprite = sprite;
    }

    private void OnMouseDown()
    {
        var position = spriteRenderer.transform.position;
        var lastPosition = fieldScript._points.Last();

        if (Math.Abs(position.x - lastPosition.x) > 1
            || Math.Abs(position.y - lastPosition.y) > 1)
        {
            return;
        }

        fieldScript._points.Add(new Vector3(position.x + 0.05f, position.y + 0.05f, 1));
        fieldScript._lineRenderer.positionCount = fieldScript._points.Count;

        var index = 0;
        foreach (var point in fieldScript._points)
        {
            fieldScript._lineRenderer.SetPosition(index, point);
            index++;
        }
    }
}
