using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldScript : MonoBehaviour
{
    private Canvas _parent;
    private Material _material;
    private Sprite _blackCircleSprite;
    private Sprite _grayCircleSprite;
    private Sprite _p1Sprite;
    private Sprite _p2Sprite;
    private SpriteRenderer _playerSpriteRenderer;

    public List<Vector3> _activePoints = new();
    public LineRenderer _lineRenderer;
    public bool _player1 = true;

    [RuntimeInitializeOnLoadMethod]
    private void Start()
    {
        var blackCircleTexture = Resources.Load<Texture2D>("Sprites/black_circle");
        var grayCircleTexture = Resources.Load<Texture2D>("Sprites/gray_circle");
        var p1 = Resources.Load<Texture2D>("Sprites/p1");
        var p2 = Resources.Load<Texture2D>("Sprites/p2");

        _parent = GetComponent<Canvas>();
        _material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Particle.mat");
        _blackCircleSprite = Sprite.Create(
            blackCircleTexture,
            new Rect(0, 0, blackCircleTexture.width, blackCircleTexture.height),
            new Vector2(0.32f, 0.32f));
        _grayCircleSprite = Sprite.Create(
            grayCircleTexture,
            new Rect(0, 0, grayCircleTexture.width, grayCircleTexture.height),
            new Vector2(0.32f, 0.32f));
        _p1Sprite = Sprite.Create(
            p1,
            new Rect(0, 0, p1.width, p1.height),
            new Vector2(0.64f, 0.64f));
        _p2Sprite = Sprite.Create(
            p2,
            new Rect(0, 0, p2.width, p2.height),
            new Vector2(0.64f, 0.64f));

        DrawFieldDots();
        DrawFieldBorders();
        DrawPlayerSprite();
    }

    public void DrawPlayerSprite()
    {
        if (_playerSpriteRenderer == null)
        {
            var gameObject = new GameObject("player");
            _playerSpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            _playerSpriteRenderer.transform.position = new Vector3(-4.5f, 4.5f, 1);
        }

        _playerSpriteRenderer.sprite = _player1 ? _p1Sprite : _p2Sprite;
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

        CreateDot(0, 4);
        CreateDot(0, -4);
    }

    private void CreateDot(int x, int y)
    {
        var gameObject = new GameObject(x + ", " + y);
        gameObject.transform.SetParent(_parent.transform, true);

        gameObject.AddComponent<PointScript>();
        gameObject.AddComponent<CircleCollider2D>();
        gameObject.AddComponent<SpriteRenderer>();

        var sprite = _grayCircleSprite;
        if (x == 0 && y == 0)
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            _lineRenderer.material = _material;
            _lineRenderer.startColor = _lineRenderer.endColor = Color.blue;
            _lineRenderer.textureMode = LineTextureMode.RepeatPerSegment;
            _lineRenderer.startWidth = _lineRenderer.endWidth = 0.1f;

            sprite = _blackCircleSprite;
        }

        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.transform.position = new Vector3(x, y, 1);
    }

    private void DrawFieldBorders()
    {
        var borderDot = new GameObject("border");
        var lineRenderer = borderDot.AddComponent<LineRenderer>();

        lineRenderer.material = _material;
        lineRenderer.startColor = lineRenderer.endColor = Color.black;
        lineRenderer.textureMode = LineTextureMode.RepeatPerSegment;
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
