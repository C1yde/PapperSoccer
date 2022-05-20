using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldScript : MonoBehaviour
{
    private Canvas _parent;
    private Material _material;
    private Sprite _p1Sprite;
    private Sprite _p2Sprite;
    private SpriteRenderer _playerSpriteRenderer;

    public bool Player1 { get; set; } = true;
    public LineRenderer LineRenderer { get; set; }
    public List<Vector3> Points { get; set; } = new();
    public List<Vector3> BorderPoints { get; set; } = new();
    public Dictionary<Vector3, List<Vector3>> LinesDict { get; set; } = new();
    public Sprite BallSprite { get; set; }
    public Sprite GrayCircleSprite { get; set; }

    [RuntimeInitializeOnLoadMethod]
    private void Start()
    {
        var ballTexture = Resources.Load<Texture2D>("Sprites/ball");
        var grayCircleTexture = Resources.Load<Texture2D>("Sprites/gray_circle");
        var p1 = Resources.Load<Texture2D>("Sprites/p1");
        var p2 = Resources.Load<Texture2D>("Sprites/p2");

        _parent = GetComponent<Canvas>();
        _material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Particle.mat");
        BallSprite = Sprite.Create(
            ballTexture,
            new Rect(0, 0, ballTexture.width, ballTexture.height),
            new Vector2(0.32f, 0.32f));
        GrayCircleSprite = Sprite.Create(
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

        _playerSpriteRenderer.sprite = Player1 ? _p1Sprite : _p2Sprite;
    }

    private void DrawFieldDots()
    {
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
        gameObject.AddComponent<SpriteRenderer>();

        var collider = gameObject.AddComponent<CircleCollider2D>();
        collider.offset = new Vector2(0.05f, 0.05f);
        collider.radius = 0.40f;

        var sprite = GrayCircleSprite;
        var position = new Vector3(x, y, 1);
        if (x == 0 && y == 0)
        {
            LineRenderer = gameObject.AddComponent<LineRenderer>();
            LineRenderer.material = _material;
            LineRenderer.startColor = LineRenderer.endColor = Color.blue;
            LineRenderer.textureMode = LineTextureMode.RepeatPerSegment;
            LineRenderer.startWidth = LineRenderer.endWidth = 0.1f;
            LineRenderer.numCornerVertices = 5;
            LineRenderer.positionCount = 1;
            LineRenderer.SetPosition(0, position);

            sprite = BallSprite;
        }

        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.transform.position = position;

        LinesDict.Add(position, new List<Vector3>());
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
        lineRenderer.numCornerVertices = 5;

        var index = 0;
        void DrawLine(int x, int y)
        {
            var point = new Vector3(x + 0.05f, y + 0.05f, 1);

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(index++, point);

            BorderPoints.Add(point);
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
