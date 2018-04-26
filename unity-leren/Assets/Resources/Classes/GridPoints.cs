using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridPoints {

    public static float multiplier = 0.16f;

    public static Vector2 UnityToGridCoord(Vector2 UnityCoords)
    {
        UnityCoords.x = UnityCoords.x * multiplier;
        UnityCoords.y = UnityCoords.y * multiplier;

        return UnityCoords;
    }

    public static Vector2 GridToUnityCoord(Vector2 GridCoords)
    {
        GridCoords.x = GridCoords.x / multiplier;
        GridCoords.y = GridCoords.y / multiplier;

        return GridCoords;
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
}
