using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour {
    
    public GameObject mapHolder;
    public Material pixelMaterial;

    [HideInInspector]
    public GameObject[,] tilesInMap;

    [Header("Map generation settings")]
    public Vector2Int unityMapSize;
    public int numberOfLakes;
    public string seed;
    public bool useRandomSeed;

    private Vector2 gridMapSize;

    public GameObject lastCreatedTile;

    public void InitializeMap()
    {
        gridMapSize = GridPoints.UnityToGridCoord(unityMapSize);
        tilesInMap = new GameObject[unityMapSize.x, unityMapSize.y];
        GetSeed();
    }

    void GetSeed()
    {
        if (useRandomSeed || seed == null)
        {
            seed = System.DateTime.Now.Ticks.ToString();
        }
        Random.InitState(seed.GetHashCode());
    }

    public void CreateBaseLayer()
    {
        for (int i = 0; i < unityMapSize.x; i++)
        {
            for (int j = 0; j < unityMapSize.y; j++)
            {
                CreateTile(TileSystem.tiles["Grass"], i, j);
            }
        }
    }

    public void CreateStructures()
    {
        for (int i = 0; i < numberOfLakes; i++)
        {
            Lake lake = new Lake();
            lake.GenerateLake(unityMapSize.x, unityMapSize.y, this);
        }
        Structure structure = new Structure(unityMapSize.x, unityMapSize.y, this);
    }

    public void CreateTile(Tile tile, int xCoord, int yCoord)
    {
        // scale the coords
        float xCoordScaled = xCoord * 0.16f;
        float yCoordScaled = yCoord * 0.16f;
        Vector2 coords = new Vector2(xCoord, yCoord);
        Vector2 gridCoords = GridPoints.UnityToGridCoord(coords);
        if (IsInsideBoundaries(coords) && !TileAlreadyThere(gridCoords, tile.isGroundTile))
        {
            GameObject tileToAdd = new GameObject("Tile", typeof(SpriteRenderer), typeof(TileObject));
            tileToAdd.transform.position = new Vector3(xCoordScaled, yCoordScaled, 0f);

            // to render the sprites correctly
            SpriteRenderer spriteRenderer = tileToAdd.GetComponent<SpriteRenderer>();
            spriteRenderer.material = pixelMaterial;

            TileObject tileData = tileToAdd.GetComponent<TileObject>();
            tileData.tile = tile;
            tileToAdd.transform.parent = mapHolder.transform;
            tilesInMap[xCoord, yCoord] = tileToAdd;
            lastCreatedTile = tileToAdd;
        }
    }

    public void CombineTilesInMap()
    {
        for (int x = 1; x < unityMapSize.x; x++)
        {
            for (int y = 1; y < unityMapSize.y; y++)
            {
                if (tilesInMap[x, y] != null)
                {
                    CombineTiles(tilesInMap[x, y], GridPoints.UnityToGridCoord(new Vector2(x, y)));
                }
            }
        }
    }

    public void RemoveTile(int xCoord, int yCoord, GameObject tile = null)
    {
        if (tile != null)
        {
            tilesInMap[xCoord, yCoord] = null;
            Destroy(tile);
        }
        else
        {
            GameObject tileToRemove = tilesInMap[xCoord, yCoord];
            tilesInMap[xCoord, yCoord] = null;
            Destroy(tileToRemove);
        }
    }

    // check if the tile to create is inside the map
    private bool IsInsideBoundaries(Vector2 gridCoords)
    {
        bool isInside = (gridCoords.x < unityMapSize.x - 0.16f && gridCoords.y < unityMapSize.y - 0.16f && gridCoords.x > 0 && gridCoords.y > 0) ? true : false;
        return isInside;
    }

    // check if a tile is there already, if you're adding a ground tile, overwrite old tile
    private bool TileAlreadyThere(Vector2 gridCoords, bool newGroundTile = false)
    {
        Vector2Int unityCoords = Vector2Int.RoundToInt(GridPoints.GridToUnityCoord(gridCoords));
        if (IsInsideBoundaries(unityCoords))
        {
            bool isTileThere = (tilesInMap[unityCoords.x, unityCoords.y] != null && !tilesInMap[unityCoords.x, unityCoords.y].GetComponent<TileObject>().tile.isGroundTile && !newGroundTile) ? true : false;
            if (newGroundTile)
            {
                RemoveTile(unityCoords.x, unityCoords.y);
            }
            return isTileThere;
        }

        return false;
    }
    
    public void CombineTiles(GameObject tileToChange, Vector2 gridCoords)
    {
        Tile tile = tileToChange.GetComponent<TileObject>().tile;
        SpriteRenderer SR = tileToChange.GetComponent<SpriteRenderer>();
        if (tile.combinable)
        {
            Vector2 unityCoords = GridPoints.GridToUnityCoord(gridCoords);
            Vector2[] coordsToCheck = new Vector2[]
            {
                new Vector2(unityCoords.x, unityCoords.y + 1),
                new Vector2(unityCoords.x + 1, unityCoords.y + 1),
                new Vector2(unityCoords.x + 1, unityCoords.y),
                new Vector2(unityCoords.x + 1, unityCoords.y - 1),
                new Vector2(unityCoords.x, unityCoords.y - 1),
                new Vector2(unityCoords.x - 1, unityCoords.y - 1),
                new Vector2(unityCoords.x - 1, unityCoords.y),
                new Vector2(unityCoords.x - 1, unityCoords.y + 1)
            };

            List<Direction> td = new List<Direction>();
            int i = 0;

            foreach(Vector2 coordToCheck in coordsToCheck)
            {
                if (IsInsideBoundaries(coordToCheck))
                {
                    float unityXCoord = coordToCheck.x;
                    float unityYCoord = coordToCheck.y;
                    if (tilesInMap[Mathf.RoundToInt(unityXCoord), Mathf.RoundToInt(unityYCoord)].GetComponent<TileObject>().tile == tile)
                    {
                        td.Add((Direction)i);
                    }
                }
                i++;
            }

            SR.sprite = tile.sprites[6];

            // top left
            if (!td.Contains(Direction.top) && !td.Contains(Direction.left) && td.Contains(Direction.bottom) && td.Contains(Direction.bottomRight) && td.Contains(Direction.right))
            {
                SR.sprite = tile.sprites[0];
            }

            // top
            if (!td.Contains(Direction.top) && td.Contains(Direction.left) && td.Contains(Direction.bottomLeft) && td.Contains(Direction.bottom) && td.Contains(Direction.bottomRight) && td.Contains(Direction.right))
            {
                SR.sprite = tile.sprites[1];
            }

            // top right
            if (!td.Contains(Direction.top) && !td.Contains(Direction.right) && td.Contains(Direction.bottom) && td.Contains(Direction.bottomLeft) && td.Contains(Direction.left))
            {
                SR.sprite = tile.sprites[2];
            }

            // left
            if (!td.Contains(Direction.left) && td.Contains(Direction.top) && td.Contains(Direction.topRight) && td.Contains(Direction.bottom) && td.Contains(Direction.bottomRight) && td.Contains(Direction.right))
            {
                SR.sprite = tile.sprites[5];
            }

            // right
            if (!td.Contains(Direction.right) && td.Contains(Direction.top) && td.Contains(Direction.topLeft) && td.Contains(Direction.bottom) && td.Contains(Direction.bottomLeft) && td.Contains(Direction.left))
            {
                SR.sprite = tile.sprites[7];
            }

            // bottom left
            if (!td.Contains(Direction.bottom) && !td.Contains(Direction.left) && td.Contains(Direction.top) && td.Contains(Direction.topRight) && td.Contains(Direction.right))
            {
                SR.sprite = tile.sprites[10];
            }

            //bottom
            if (!td.Contains(Direction.bottom) && td.Contains(Direction.left) && td.Contains(Direction.topLeft) && td.Contains(Direction.top) && td.Contains(Direction.topRight) && td.Contains(Direction.right))
            {
                SR.sprite = tile.sprites[11];
            }

            // bottom right
            if (!td.Contains(Direction.bottom) && !td.Contains(Direction.right) && td.Contains(Direction.top) && td.Contains(Direction.topLeft) && td.Contains(Direction.left))
            {
                SR.sprite = tile.sprites[12];
            }

            // inner top left
            if (td.Count == 7 && !td.Contains(Direction.topLeft))
            {
                SR.sprite = tile.sprites[17];
            }

            // inner top
            if (td.Count == 6 && !td.Contains(Direction.topRight) && !td.Contains(Direction.topLeft))
            {
                SR.sprite = tile.sprites[16];
            }

            // inner top right
            if (td.Count == 7 && !td.Contains(Direction.topRight))
            {
                SR.sprite = tile.sprites[15];
            }

            // inner left
            if (td.Count == 6 && !td.Contains(Direction.bottomLeft) && !td.Contains(Direction.topLeft))
            {
                SR.sprite = tile.sprites[23];
            }

            // inner right
            if (td.Count == 6 && !td.Contains(Direction.bottomRight) && !td.Contains(Direction.topRight))
            {
                SR.sprite = tile.sprites[18];
            }

            // inner bottom left
            if (td.Count == 7 && !td.Contains(Direction.bottomLeft))
            {
                SR.sprite = tile.sprites[20];
            }

            // inner bottom
            if (td.Count == 6 && !td.Contains(Direction.bottomRight) && !td.Contains(Direction.bottomLeft))
            {
                SR.sprite = tile.sprites[21];
            }

            // inner bottom right
            if (td.Count == 7 && !td.Contains(Direction.bottomRight))
            {
                SR.sprite = tile.sprites[22];
            }
            
            // single top
            if (td.Contains(Direction.bottom) && !td.Contains(Direction.left) && !td.Contains(Direction.right) && !td.Contains(Direction.top))
            {
                SR.sprite = tile.sprites[8];
            }
            
            // single left
            if (td.Contains(Direction.right) && !td.Contains(Direction.top) && !td.Contains(Direction.bottom) && !td.Contains(Direction.left))
            {
                SR.sprite = tile.sprites[9];
            }

            // single right
            if (td.Contains(Direction.left) && !td.Contains(Direction.top) && !td.Contains(Direction.bottom) && !td.Contains(Direction.right))
            {
                SR.sprite = tile.sprites[14];
            }
            
            // single bottom
            if (td.Contains(Direction.top) && !td.Contains(Direction.left) && !td.Contains(Direction.right) && !td.Contains(Direction.bottom))
            {
                SR.sprite = tile.sprites[13];
            }

            // horizontal
            if (td.Contains(Direction.left) && td.Contains(Direction.right) && !td.Contains(Direction.top) && !td.Contains(Direction.bottom))
            {
                SR.sprite = tile.sprites[4];
            }

            // vertical
            if (td.Contains(Direction.top) && td.Contains(Direction.bottom) && !td.Contains(Direction.left) && !td.Contains(Direction.right))
            {
                SR.sprite = tile.sprites[3];
            }
            /*
            // left bottom corner straight
            if (td.Contains(Direction.top) && td.Contains(Direction.right) && td.Contains(Direction.topRight) && td.Contains(Direction.bottom) && !td.Contains(Direction.left) && !td.Contains(Direction.bottomRight))
            {
                SR.sprite = tile.sprites[23];
            }

            // right bottom corner straight
            if (td.Contains(Direction.top) && td.Contains(Direction.left) && td.Contains(Direction.topLeft) && td.Contains(Direction.bottom) && !td.Contains(Direction.right) && !td.Contains(Direction.bottomLeft))
            {
                SR.sprite = tile.sprites[24];
            }

            // left top corner straight
            if (td.Contains(Direction.bottom) && td.Contains(Direction.right) && td.Contains(Direction.bottomRight) && td.Contains(Direction.top) && !td.Contains(Direction.left) && !td.Contains(Direction.topRight))
            {
                SR.sprite = tile.sprites[25];
            }

            // right top corner straight
            if (td.Contains(Direction.bottom) && td.Contains(Direction.left) && td.Contains(Direction.bottomLeft) && td.Contains(Direction.top) && !td.Contains(Direction.right) && !td.Contains(Direction.topLeft))
            {
                SR.sprite = tile.sprites[26];
            }*/

            // center
            if (td.Count == 8)
            {
                SR.sprite = tile.sprites[6];
            }

            // single tile
            if (!td.Contains(Direction.left) && !td.Contains(Direction.right) && !td.Contains(Direction.top) && !td.Contains(Direction.bottom))
            {
                SR.sprite = tile.sprites[19];
            }
        }
    }
}
