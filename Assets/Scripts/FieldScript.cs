using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldScript : MonoBehaviour
{
    private Material _material;
    private Sprite _blackCircleSprite;
    private Sprite _grayCircleSprite;

    public List<Vector3> _activePoints = new();
    public LineRenderer _lineRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        var blackCircleTexture = Resources.Load<Texture2D>("Sprites/black_circle");
        var grayCircleTexture = Resources.Load<Texture2D>("Sprites/gray_circle");

        _material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Line.mat");
        _blackCircleSprite = Sprite.Create(
            blackCircleTexture,
            new Rect(0, 0, blackCircleTexture.width, blackCircleTexture.height),
            new Vector2(0.32f, 0.32f));
        _grayCircleSprite = Sprite.Create(
            grayCircleTexture,
            new Rect(0, 0, grayCircleTexture.width, grayCircleTexture.height),
            new Vector2(0.32f, 0.32f));

        DrawFieldDots();
        DrawFieldBorders();
    }

    private void DrawFieldDots()
    {
        _activePoints.Add(new Vector3(0.05f, 0.05f, 1));

        for (var x = -3; x < 4; x++)
        {
            for (var y = -3; y < 4; y++)
            {
                CreateDot(x, y);
            }
        }

        for (var x = -1; x < 2; x++)
        {
            foreach (var y in new int[] { 4, -4 })
            {
                CreateDot(x, y);
            }
        }
    }

    private void CreateDot(int x, int y)
    {
        var gameObject = new GameObject
        {
            name = x + ", " + y
        };

        gameObject.AddComponent<PointScript>();
        gameObject.AddComponent<CircleCollider2D>();
        gameObject.AddComponent<SpriteRenderer>();

        var sprite = _grayCircleSprite;
        if (x == 0 && y == 0)
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            _lineRenderer.material = _material;
            _lineRenderer.startColor = _lineRenderer.endColor = Color.blue;
            _lineRenderer.startWidth = _lineRenderer.endWidth = 0.1f;

            sprite = _blackCircleSprite;
        }

        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.transform.position = new Vector3(x, y, 1);
    }

    private void DrawFieldBorders()
    {
        var borderDot = GameObject.Find("-3, -3");
        var lineRenderer = borderDot.AddComponent<LineRenderer>();

        lineRenderer.material = _material;
        lineRenderer.startColor = lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 0;

        var index = 0;
        void DrawLine(int x, int y)
        {
            var point = new Vector3(x + 0.05f, y + 0.05f, 1);

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(index++, point);
        }

        var x = -3;
        var y = -3;

        for (; y <= 2; y++)
        {
            DrawLine(x, y);
        }

        for (; x <= -2; x++)
        {
            DrawLine(x, y);
        }

        for (; y <= 3; y++)
        {
            DrawLine(x, y);
        }

        for (; x <= 0; x++)
        {
            DrawLine(x, y);
        }

        for (; y >= 4; y--)
        {
            DrawLine(x, y);
        }

        for (; x <= 2; x++)
        {
            DrawLine(x, y);
        }

        for (; y >= -2; y--)
        {
            DrawLine(x, y);
        }

        for (; x >= 2; x--)
        {
            DrawLine(x, y);
        }

        for (; y >= -3; y--)
        {
            DrawLine(x, y);
        }

        for (; x >= 0; x--)
        {
            DrawLine(x, y);
        }

        for (; y <= -4; y++)
        {
            DrawLine(x, y);
        }

        for (; x >= -3; x--)
        {
            DrawLine(x, y);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
