using System.Collections.Generic;

public class MovementLogic
{
    public HashSet<(float, float)> points;
    public HashSet<(float, float)> finishedPoints;
    public HashSet<(float, float)> goalPoints;

    public MovementLogic()
    {
        points = new HashSet<(float, float)>();
        finishedPoints = new HashSet<(float, float)>();
        goalPoints = new HashSet<(float, float)>();

        goalPoints.Add((-3, -3));
        goalPoints.Add((-4, -3));
        goalPoints.Add((-2, -3));

        goalPoints.Add((-3, 3));
        goalPoints.Add((-4, 3));
        goalPoints.Add((-2, 3));
    }

    public void AddPoint(float x, float y)
    {
        points.Add((x, y));
    }

    public bool NextMove(float x, float y)
    {
        if (IsGoal(x, y))
        {
            //new game
        }

        if (IsFinished(x, y))
        {
            //new game
        }

        if (points.Contains((x, y)))
        {
            return true;
        }

        AddPoint(x, y);
        return false;
    }

    public bool IsGoal(float x, float y)
    {
        if (goalPoints.Contains((x, y)))
        {
            return true;
        }

        return false;
    }

    public bool IsFinished(float x, float y)
    {
        if (finishedPoints.Contains((x, y)))
        {
            return true;
        }

        return false;
    }
}
