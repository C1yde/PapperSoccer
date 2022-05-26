using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    private FieldScript _fieldScript;
    private SpriteRenderer _spriteRenderer;
    private List<(int x, int y)> _goalPoints;

    public (int, int) CurrentPosition { get; set; }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _fieldScript = GameObject
            .FindGameObjectWithTag("GameController")
            .GetComponent<FieldScript>();
        _goalPoints = new List<(int x, int y)>
        {
            (0, -4),
            (0, 4)
        };
    }

    private void OnMouseOver()
    {
        var lastPosition = _fieldScript.Points.Any()
            ? _fieldScript.Points.Last()
            : (0, 0);

        if (!IsNearPoint(CurrentPosition, lastPosition)
            || IsLineExisted(CurrentPosition, lastPosition)
            || (IsBorderLine(CurrentPosition, lastPosition) 
                && IsBorderLine(lastPosition, CurrentPosition))
            || IsPointFinished(CurrentPosition))
        {
            return;
        }

        _spriteRenderer.sprite = _fieldScript.BallSprite;
    }

    private void OnMouseExit()
    {
        ResetPoint(CurrentPosition);
    }

    private void OnMouseDown()
    {
        if (_goalPoints.Contains(CurrentPosition))
        {
            _fieldScript.DrawGoalSprite();
            return;
        }

        var lastPosition = _fieldScript.Points.Any()
            ? _fieldScript.Points.Last()
            : (0, 0);
        var newPosition = new Vector3(
            CurrentPosition.Item1 + 0.05f,
            CurrentPosition.Item2 + 0.05f);

        if (CurrentPosition == lastPosition
            || !IsNearPoint(CurrentPosition, lastPosition)
            || IsPointFinished(CurrentPosition)
            || IsLineExisted(lastPosition, CurrentPosition)
            || (IsBorderLine(CurrentPosition, lastPosition) 
                && IsBorderLine(lastPosition, CurrentPosition)))
        {
            return;
        }

        if (!IsPointChecked(CurrentPosition, lastPosition))
        {
            _fieldScript.Player1 = !_fieldScript.Player1;
            _fieldScript.DrawPlayerSprite();
        }

        _fieldScript.LinesDict[lastPosition].Add(CurrentPosition);
        _fieldScript.Points.Add(CurrentPosition);
        _fieldScript.LineRenderer.positionCount++;
        _fieldScript.LineRenderer.SetPosition(
            _fieldScript.LineRenderer.positionCount - 1,
            newPosition);

        ResetPoint(lastPosition);
    }

    private static bool IsNearPoint(
        (int, int) currentPosition,
        (int, int) lastPosition)
    {
        return Math.Abs(currentPosition.Item1 - lastPosition.Item1) <= 1
            && Math.Abs(currentPosition.Item2 - lastPosition.Item2) <= 1;
    }

    private bool IsPointFinished((int, int) position)
    {
        var anglePositions = new List<(int, int)>
        {
            (-3, -3),
            (-3, 3),
            (3, 3),
            (3, -3)
        };

        if (anglePositions.Contains(position))
        {
            // Angle points
            return true;
        }
        
        int availableLines;
        if (!_fieldScript.BorderPoints
            .Any(point => point.x == position.Item1 && point.y == position.Item2))
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

    private bool IsLineExisted((int, int) currentPosition, (int, int) lastPosition)
    {
        return _fieldScript.LinesDict[lastPosition].Contains(currentPosition)
            || _fieldScript.LinesDict[currentPosition].Contains(lastPosition);
    }

    private bool IsPointChecked((int, int) currentPosition, (int, int) lastPosition)
    {
        return _fieldScript.LinesDict[currentPosition].Any()
            || IsBorderLine(currentPosition, lastPosition);
    }

    private bool IsBorderLine((int, int) currentPosition, (int, int) lastPosition)
    {
        if (currentPosition.Item1 == 0 || lastPosition.Item1 == 0)
        {
            return false;
        }

        return ((lastPosition.Item1 == 3 || lastPosition.Item1 == -3) 
                && (currentPosition.Item1 == 3 || currentPosition.Item1 == -3))
            || ((lastPosition.Item2 == 3 || lastPosition.Item2 == -3) 
                && (currentPosition.Item2 == 3 || currentPosition.Item2 == -3));
    }

    private void ResetPoint((int, int) position)
    {
        var lastPosition = _fieldScript.Points.Any()
            ? _fieldScript.Points.Last()
            : (0, 0);
        if (position == lastPosition)
        {
            return;
        }

        var pointToReset = GameObject.Find(position.Item1 + ", " + position.Item2);
        var spriteRenderer = pointToReset.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = _fieldScript.GrayCircleSprite;
    }
}
