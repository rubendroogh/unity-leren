using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * GridPoints class is the coordinate system for tiles
 *   1 gridpoint = 0.16 unitycoordinate
 */

public class GridPoints {

    public static float multiplier = 0.16f;
    public float x;
    public float y;
    public int layer;

    public GridPoints(float x = 0, float y = 0, int layer = 0)
    {
        this.x = x;
        this.y = y;
        this.layer = layer;
    }

    public static GridPoints UnityToGridCoord(Vector2 unityCoords)
    {
        GridPoints gridPoints = new GridPoints
        {
            x = unityCoords.x * multiplier,
            y = unityCoords.y * multiplier
        };

        return gridPoints;
    }

    public static Vector2 GridToUnityCoord(GridPoints gridCoords)
    {
        Vector2 unityCoords = new Vector2
        {
            x = gridCoords.x / multiplier,
            y = gridCoords.y / multiplier
        };

        return unityCoords;
    }

    public static GridPoints MousePosToGridPoints(Vector2 mousePosition)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        GridPoints gridPoints = new GridPoints
        {
            x = mousePosition.x / multiplier,
            y = mousePosition.y / multiplier
        };

        return gridPoints;
    }

    public static float UnityToGridNum(float unityNum)
    {
        float gridNum = unityNum * multiplier;
        return gridNum;
    }

    public static float GridToUnityNum(float gridNum)
    {
        float unityNum = gridNum / multiplier;
        return unityNum;
    }

    public static GridPoints GridDistance(GridPoints coordsToMeasureFrom, GridPoints coordsToMeasureTo)
    {
        GridPoints gridDistance = new GridPoints
        {
            x = coordsToMeasureTo.x - coordsToMeasureFrom.x,
            y = coordsToMeasureTo.y - coordsToMeasureFrom.y
        };

        gridDistance.x = Mathf.Abs(gridDistance.x);
        gridDistance.y = Mathf.Abs(gridDistance.y);

        return gridDistance;
    }

    public static implicit operator GridPoints(Vector2 toConvert)
    {
        return new GridPoints(toConvert.x * multiplier, toConvert.y * multiplier);
    }

    public static implicit operator GridPoints(Vector3 toConvert)
    {
        return new GridPoints(toConvert.x * multiplier, toConvert.y * multiplier);
    }
}
