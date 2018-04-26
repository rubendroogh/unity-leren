using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileSystem {

    public static Dictionary<string, Tile> tiles = LoadAllTiles();

    public static Dictionary<string, Tile> LoadAllTiles()
    {
        Object[] allAssets = Resources.LoadAll("", typeof(Tile));
        Dictionary<string, Tile> allTiles = new Dictionary<string, Tile>();
        foreach (Object tile in allAssets)
        {
            Tile currentTile = (Tile)tile;
            allTiles.Add(currentTile.tileName, currentTile);
        }

        return allTiles;
    }

	public static void CreateTile()
    {

    }

    public static void RemoveTile()
    {

    }

    public static void CreateStructure()
    {

    }

    public static void CombineTile()
    {

    }

    public static Vector2 GridDistance(Vector2 coordsToMeasureFrom, Vector2 coordsToMeasureTo)
    {
        Vector2 gridDistance = new Vector2
        {
            x = coordsToMeasureTo.x - coordsToMeasureFrom.x,
            y = coordsToMeasureTo.y - coordsToMeasureFrom.y
        };

        gridDistance.x = Mathf.Abs(gridDistance.x);
        gridDistance.y = Mathf.Abs(gridDistance.y);

        return gridDistance;
    }
}
