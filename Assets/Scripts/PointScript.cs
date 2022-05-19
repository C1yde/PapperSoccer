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
        var currentPosition = spriteRenderer.transform.position;
        var lastPosition = fieldScript._activePoints.Last();

        if (Math.Abs(currentPosition.x - lastPosition.x) > 1.05
            || Math.Abs(currentPosition.y - lastPosition.y) > 1.05)
        {
            return;
        }

        var texture = Resources.Load<Texture2D>("Sprites/black_circle");
        var sprite = Sprite.Create(
            texture,
            new Rect(0.0f, 0.0f, texture.width, texture.height),
            new Vector2(0.32f, 0.32f));

        spriteRenderer.sprite = sprite;
    }

    private void OnMouseExit()
    {
        var currentPosition = spriteRenderer.transform.position;
        var middlePosition = new Vector3(0, 0, 1);

        if (middlePosition == currentPosition)
        {
            return;
        }

        var texture = Resources.Load<Texture2D>("Sprites/gray_circle");
        var sprite = Sprite.Create(
            texture,
            new Rect(0.0f, 0.0f, texture.width, texture.height),
            new Vector2(0.32f, 0.32f));

        spriteRenderer.sprite = sprite;
    }

    private void OnMouseDown()
    {
        var currentPosition = spriteRenderer.transform.position;
        var lastPosition = fieldScript._activePoints.Last();
        var newPosition = new Vector3(currentPosition.x + 0.05f, currentPosition.y + 0.05f, 1);

        if (Math.Abs(currentPosition.x - lastPosition.x) > 1.05
            || Math.Abs(currentPosition.y - lastPosition.y) > 1.05)
        {
            return;
        }

        fieldScript._activePoints.Add(newPosition);
        fieldScript._lineRenderer.positionCount = fieldScript._activePoints.Count;

        var index = 0;
        foreach (var point in fieldScript._activePoints)
        {
            fieldScript._lineRenderer.SetPosition(index, point);
            index++;
        }
    }
}
