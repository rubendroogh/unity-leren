using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileSystem {

    private static MapController map = GameObject.Find("GameManager").GetComponent<MapController>();
    private static Material pixelMaterial = (Material)Resources.Load("PixelStandard", typeof(Material));

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

	public static void CreateTile(Tile tile, GridPoints gridPoints)
    {
        Vector2 unityCoords = GridPoints.GridToUnityCoord(gridPoints);

        if (map.IsInsideBoundaries(unityCoords) && !map.TileAlreadyThere(gridPoints, tile.isGroundTile))
        {
            GameObject tileToAdd = new GameObject("Tile", typeof(SpriteRenderer), typeof(TileObject));
            tileToAdd.transform.position = GridPoints.GridToUnityCoord(gridPoints); //TODO gridpoints to vector3 implicit;

            // to render the sprites correctly
            SpriteRenderer spriteRenderer = tileToAdd.GetComponent<SpriteRenderer>();
            spriteRenderer.material = pixelMaterial;

            TileObject tileData = tileToAdd.GetComponent<TileObject>();
            tileData.tile = tile;
            tileToAdd.transform.parent = map.mapHolder.transform;
            map.tilesInMap[gridPoints.intX, gridPoints.intY] = tileToAdd;
        }
        // TODO: MOET FATSOENLIJK GEIMPLEMENTEERD WORDEN
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
}
