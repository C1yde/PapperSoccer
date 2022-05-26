using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

public class FieldScript : MonoBehaviour
{
    private GameObject _parent;
    private Material _material;
    private Sprite _p1Sprite;
    private Sprite _p2Sprite;
    private SpriteRenderer _playerSpriteRenderer;

    public bool Player1 { get; set; } = true;
    public LineRenderer LineRenderer { get; set; }
    public List<(int x, int y)> Points { get; set; } = new();
    public List<(int x, int y)> BorderPoints { get; set; } = new();
    public Dictionary<(int x, int y), List<(int x, int y)>> LinesDict { get; set; } = new();
    public Sprite BallSprite { get; set; }
    public Sprite GrayCircleSprite { get; set; }

    [RuntimeInitializeOnLoadMethod]
    private void Start()
    {
        var ballTexture = Resources.Load<Texture2D>("Sprites/ball");
        var grayCircleTexture = Resources.Load<Texture2D>("Sprites/gray_circle");
        var p1 = Resources.Load<Texture2D>("Sprites/p1");
        var p2 = Resources.Load<Texture2D>("Sprites/p2");

        _parent = this.gameObject;
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

        var playerObject = new GameObject("PlayerIcon");
        _playerSpriteRenderer = playerObject.AddComponent<SpriteRenderer>();
        _playerSpriteRenderer.transform.position = new Vector3(-2.5f, 4.5f);

        playerObject.transform.SetParent(_parent.transform, true);

        DrawFieldDots();
        DrawFieldBorders();
        DrawPlayerSprite();
    }

    public void DrawPlayerSprite()
    {
        _playerSpriteRenderer.sprite = Player1 ? _p1Sprite : _p2Sprite;
    }

    public void DrawGoalSprite()
    {
        var goal = Resources.Load<Texture2D>("Sprites/goal");
        var goalObject = new GameObject("GoalIcon");
        var goalSprite = Sprite.Create(
            goal,
            new Rect(0, 0, goal.width, goal.height),
            new Vector2(0.9f, 0.9f));
        var goalSpriteRenderer = goalObject.AddComponent<SpriteRenderer>();

        goalSpriteRenderer.transform.position = new Vector3(2, 1);
        goalSpriteRenderer.sprite = goalSprite;
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

        var script = gameObject.AddComponent<MovementScript>();
        script.CurrentPosition = (x, y);

        var collider = gameObject.AddComponent<CircleCollider2D>();
        collider.offset = new Vector2(0.05f, 0.05f);
        collider.radius = 0.40f;

        var sprite = GrayCircleSprite;
        var position = new Vector3(x, y);
        if (x == 0 && y == 0)
        {
            LineRenderer = gameObject.AddComponent<LineRenderer>();
            LineRenderer.material = _material;
            LineRenderer.startColor = LineRenderer.endColor = Color.blue;
            LineRenderer.textureMode = LineTextureMode.RepeatPerSegment;
            LineRenderer.startWidth = LineRenderer.endWidth = 0.1f;
            LineRenderer.numCornerVertices = 5;
            LineRenderer.positionCount = 1;
            LineRenderer.sortingOrder = 1;
            LineRenderer.SetPosition(0, new Vector3(0.05f, 0.05f));

            sprite = BallSprite;
        }

        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.transform.position = position;
        spriteRenderer.sortingOrder = 1;

        LinesDict.Add((x, y), new List<(int x, int y)>());
    }

    private void DrawFieldBorders()
    {
        var borderDot = new GameObject("BorderLine");
        var lineRenderer = borderDot.AddComponent<LineRenderer>();

        lineRenderer.material = _material;
        lineRenderer.startColor = lineRenderer.endColor = Color.black;
        lineRenderer.textureMode = LineTextureMode.RepeatPerSegment;
        lineRenderer.startWidth = lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 0;
        lineRenderer.sortingOrder = 1;
        lineRenderer.numCornerVertices = 5;

        var index = 0;
        void DrawLine(int x, int y)
        {
            var point = new Vector3(x + 0.05f, y + 0.05f);

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(index++, point);

            BorderPoints.Add((x, y));
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
