using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointScript : MonoBehaviour
{
    private FieldScript _fieldScript;
    private SpriteRenderer _spriteRenderer;
    private MovementLogic _movementLogic;
    private List<Vector3> _goalPoints;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _fieldScript = GameObject
            .FindGameObjectWithTag("GameController")
            .GetComponent<FieldScript>();
        _movementLogic = new MovementLogic();

        _goalPoints = new List<Vector3>
        {
            new Vector3(0, -4),
            new Vector3(0, 4)
        };
    }

    private void OnMouseOver()
    {
        var currentPosition = _spriteRenderer.transform.position;
        var lastPosition = _fieldScript.Points.Any()
            ? _fieldScript.Points.Last()
            : new Vector3(0, 0);

        if (!IsNearPoint(currentPosition, lastPosition)
            || IsLineExisted(currentPosition, lastPosition)
            || (IsBorderLine(currentPosition, lastPosition) && IsBorderLine(lastPosition, currentPosition))
            || IsPointFinished(currentPosition))
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
            : new Vector3(0, 0);
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

        if (_goalPoints.Contains(currentPosition))
        {
            _fieldScript.DrawGoalSprite();
            return;
        }

        var lastPosition = _fieldScript.Points.Any()
            ? _fieldScript.Points.Last()
            : new Vector3(0, 0);
        var newPosition = new Vector3(
            currentPosition.x + 0.05f,
            currentPosition.y + 0.05f);

        if (currentPosition == lastPosition
            || !IsNearPoint(currentPosition, lastPosition)
            || IsPointFinished(currentPosition)
            || IsLineExisted(lastPosition, currentPosition)
            || (IsBorderLine(currentPosition, lastPosition) 
                && IsBorderLine(lastPosition, currentPosition)))
        {
            return;
        }

        if (!IsPointChecked(currentPosition, lastPosition))
        {
            _fieldScript.Player1 = !_fieldScript.Player1;
            _fieldScript.DrawPlayerSprite();
        }

        _fieldScript.LinesDict[lastPosition].Add(currentPosition);
        _fieldScript.Points.Add(currentPosition);
        _fieldScript.LineRenderer.positionCount++;
        _fieldScript.LineRenderer.SetPosition(
            _fieldScript.LineRenderer.positionCount - 1,
            newPosition);

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
            new Vector3(-3, -3),
            new Vector3(-3, 3),
            new Vector3(3, 3),
            new Vector3(3, -3)
        };

        if (anglePositions.Contains(position))
        {
            // Angle points
            return true;
        }
        
        int availableLines;
        if (!_fieldScript.BorderPoints
            .Any(point => point.x == position.x && point.y == position.y))
        {
            // Inner points
            availableLines = 5;
        }
        else
        {
            // Other border points
            availableLines = 3;
        }

        var usedLinesCount = _fieldScript.LinesDict[position].Count;

        return usedLinesCount + 1 >= availableLines;
    }

    private bool IsLineExisted(Vector3 currentPosition, Vector3 lastPosition)
    {
        return _fieldScript.LinesDict[lastPosition].Contains(currentPosition)
            || _fieldScript.LinesDict[currentPosition].Contains(lastPosition);
    }

    private bool IsPointChecked(Vector3 currentPosition, Vector3 lastPosition)
    {
        return _fieldScript.LinesDict[currentPosition].Any()
            || IsBorderLine(currentPosition, lastPosition);
    }

    private bool IsBorderLine(Vector3 currentPosition, Vector3 lastPosition)
    {
        if (currentPosition.x == 0 || lastPosition.x == 0)
        {
            return false;
        }

        return ((lastPosition.x == 3 || lastPosition.x == -3) 
                && (currentPosition.x == 3 || currentPosition.x == -3))
            || ((lastPosition.y == 3 || lastPosition.y == -3) 
                && (currentPosition.y == 3 || currentPosition.y == -3));
    }
}
