using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointScript : MonoBehaviour
{
    private FieldScript _fieldScript;
    private SpriteRenderer _spriteRenderer;
    private MovementLogic _movementLogic;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _fieldScript = GameObject
            .FindGameObjectWithTag("GameController")
            .GetComponent<FieldScript>();
        _movementLogic = new MovementLogic();
    }

    private void OnMouseOver()
    {
        var currentPosition = _spriteRenderer.transform.position;
        var lastPosition = _fieldScript.Points.Any()
            ? _fieldScript.Points.Last()
            : new Vector3(0, 0, 1);

        if (!IsNearPoint(currentPosition, lastPosition))
        {
            return;
        }

        _spriteRenderer.sprite = _fieldScript.BallSprite;
    }

    private void OnMouseExit()
    {
        var currentPosition = _spriteRenderer.transform.position;
        ResetPoint(currentPosition);
    }

    private void ResetPoint(Vector3 position)
    {
        var lastPosition = _fieldScript.Points.Any()
            ? _fieldScript.Points.Last()
            : new Vector3(0, 0, 1);
        if (position == lastPosition)
        {
            return;
        }

        var pointToReset = GameObject.Find(position.x + ", " + position.y);
        var spriteRenderer = pointToReset.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = _fieldScript.GrayCircleSprite;
    }

    private void OnMouseDown()
    {
        var currentPosition = _spriteRenderer.transform.position;
        var lastPosition = _fieldScript.Points.Any()
            ? _fieldScript.Points.Last()
            : new Vector3(0, 0, 1);
        var newPosition = new Vector3(
            currentPosition.x + 0.05f,
            currentPosition.y + 0.05f,
            1);

        if (!IsNearPoint(currentPosition, lastPosition)
            || IsPointFinished(currentPosition)
            || IsLineExisted(lastPosition, currentPosition))
        {
            return;
        }

        _fieldScript.LinesDict[lastPosition].Add(currentPosition);
        _fieldScript.Points.Add(currentPosition);
        _fieldScript.LineRenderer.positionCount++;
        _fieldScript.LineRenderer.SetPosition(
            _fieldScript.LineRenderer.positionCount - 1,
            newPosition);

        _fieldScript.Player1 = !_fieldScript.Player1;
        _fieldScript.DrawPlayerSprite();

        ResetPoint(lastPosition);
    }

    private static bool IsNearPoint(
        Vector3 currentPosition,
        Vector3 lastPosition)
    {
        return Math.Abs(currentPosition.x - lastPosition.x) <= 1
            && Math.Abs(currentPosition.y - lastPosition.y) <= 1;
    }

    private bool IsPointFinished(Vector3 position)
    {
        var anglePositions = new List<Vector3>
        {
            new Vector3(-3, -3, 1),
            new Vector3(-3, 3, 1),
            new Vector3(3, 3, 1),
            new Vector3(3, -3, 1)
        };

        int availableLines;
        if (!_fieldScript.BorderPoints
            .Any(point => point.x == position.x && point.y == position.y))
        {
            // Inner points
            availableLines = 5;
        }
        else if (anglePositions.Contains(position))
        {
            // Angle points
            availableLines = 1;
        } 
        else
        {
            // Other border points
            availableLines = 3;
        }

        var usedLinesCount = _fieldScript.LinesDict[position].Count;

        return usedLinesCount + 1 >= availableLines;
    }

    private bool IsLineExisted(Vector3 oldPosition, Vector3 newPosition)
    {
        return _fieldScript.LinesDict[oldPosition].Contains(newPosition)
            || _fieldScript.LinesDict[newPosition].Contains(oldPosition);
    }
}
