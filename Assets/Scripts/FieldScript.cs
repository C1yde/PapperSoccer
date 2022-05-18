using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldScript : MonoBehaviour
{
    public List<Vector3> _points = new();
    public HashSet<Vector3> _activePoints = new();
    public LineRenderer _lineRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        var grayCircleTexture = Resources.Load<Texture2D>("Sprites/gray_circle");
        var blackCircleTexture = Resources.Load<Texture2D>("Sprites/black_circle");

        var points = new List<GameObject>();

        _points.Add(new Vector3(0.05f, 0.05f, 1));
        _activePoints.Add(new Vector3(0.05f, 0.05f, 1));

        for (var x = -3; x < 4; x++)
        {
            for (var y = -3; y < 4; y++)
            {
                var texture = grayCircleTexture;
                if (x == 0 && y == 0)
                {
                    texture = blackCircleTexture;
                }

                var sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.32f, 0.32f));

                var gameObject = new GameObject
                {
                    name = x + ", " + y
                };

                gameObject.AddComponent<PointScript>();
                gameObject.AddComponent<CircleCollider2D>();

                _lineRenderer = gameObject.AddComponent<LineRenderer>();
                _lineRenderer.material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Line.mat");
                _lineRenderer.startColor = Color.blue;
                _lineRenderer.endColor = Color.blue;
                _lineRenderer.startWidth = 0.1f;
                _lineRenderer.endWidth = 0.1f;

                gameObject.AddComponent<SpriteRenderer>();

                var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;
                spriteRenderer.transform.position = new Vector3(x, y, 1);

                points.Add(gameObject);
            }
        }

        for (var x = -1; x < 2; x++)
        {
            foreach (var y in new int[] { 4, -4 })
            {
                var sprite = Sprite.Create(
                    grayCircleTexture,
                    new Rect(0.0f, 0.0f, grayCircleTexture.width, grayCircleTexture.height),
                    new Vector2(0.32f, 0.32f));

                var gameObject = new GameObject
                {
                    name = x + ", " + y
                };
                gameObject.AddComponent<SpriteRenderer>();

                var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;
                spriteRenderer.sortingOrder = 1;
                spriteRenderer.transform.position = new Vector3(x, y);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
