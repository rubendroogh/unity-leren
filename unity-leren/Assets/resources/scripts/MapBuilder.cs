using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum tileType
{
    grass, wall, water, farmland, test, stonetile
}

public class MapBuilder : MonoBehaviour {
    
    public GameObject mapHolder;
    public Tile[] tiles;
    public Material pixelMaterial;

    [HideInInspector]
    public GameObject[,] tilesInMap;

    [Header("Map generation settings")]
    public int xSize;
    public int ySize;
    public int numberOfLakes;
    public string seed;
    public bool useRandomSeed;

    private float xSizeScaled;
    private float ySizeScaled;

    public void InitializeMap()
    {
        xSizeScaled = xSize * 0.16f;
        ySizeScaled = ySize * 0.16f;
        tilesInMap = new GameObject[xSize, ySize];
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
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                CreateTile(tiles[(int)tileType.grass], i, j);
            }
        }
    }

    public void CreateStructures()
    {
        for (int i = 0; i < numberOfLakes; i++)
        {
            Lake lake = new Lake();
            lake.GenerateLake(xSize, ySize, this);
        }
        Structure structure = new Structure(xSize, ySize, this);
    }

    public void CreateTile(Tile tile, int xCoord, int yCoord)
    {
        // scale the coords
        float xCoordScaled = xCoord * 0.16f;
        float yCoordScaled = yCoord * 0.16f;
        if (IsInsideBoundaries(xCoordScaled, yCoordScaled, true) && !TileAlreadyThere(xCoord, yCoord, tile.isGroundTile))
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
        }
    }

    public void CombineTilesInMap()
    {
        for (int x = 1; x < xSize; x++)
        {
            for (int y = 1; y < ySize; y++)
            {
                if (tilesInMap[x, y] != null)
                {
                    CombineTiles(tilesInMap[x, y], x, y);
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
    private bool IsInsideBoundaries(float xCoord, float yCoord, bool isScaled = false)
    {
        if (!isScaled)
        {
            xCoord = xCoord * 0.16f;
            yCoord = yCoord * 0.16f;
        }
        bool isInside = (xCoord < xSizeScaled - 0.16f && yCoord < ySizeScaled - 0.16f && xCoord > 0 && yCoord > 0) ? true : false;
        return isInside;
    }

    // check if a tile is there already, if you're adding a ground tile, overwrite old tile
    private bool TileAlreadyThere(int xCoord, int yCoord, bool newGroundTile = false)
    {
        bool isTileThere = (tilesInMap[xCoord, yCoord] != null && !tilesInMap[xCoord, yCoord].GetComponent<TileObject>().tile.isGroundTile && !newGroundTile) ? true : false;
        if (newGroundTile)
        {
            RemoveTile(xCoord, yCoord);
        }
        return isTileThere;
    }
    
    private void CombineTiles(GameObject tileToAdd, int xCoord, int yCoord)
    {
        Tile tile = tileToAdd.GetComponent<TileObject>().tile;
        SpriteRenderer SR = tileToAdd.GetComponent<SpriteRenderer>();
        if (tile.combinable)
        {
            List <Direction> td = new List<Direction>();
            if (IsInsideBoundaries(xCoord + 1, yCoord))
            {
                if (tilesInMap[xCoord + 1, yCoord].GetComponent<TileObject>().tile == tile)
                {
                    td.Add(Direction.right);
                }
            }
            if (IsInsideBoundaries(xCoord - 1, yCoord))
            {
                if (tilesInMap[xCoord - 1, yCoord].GetComponent<TileObject>().tile == tile)
                {
                    td.Add(Direction.left);
                }
            }
            if (IsInsideBoundaries(xCoord, yCoord + 1))
            {
                if (tilesInMap[xCoord, yCoord + 1].GetComponent<TileObject>().tile == tile)
                {
                    td.Add(Direction.top);
                }
            }
            if (IsInsideBoundaries(xCoord, yCoord - 1))
            {
                if (tilesInMap[xCoord, yCoord - 1].GetComponent<TileObject>().tile == tile)
                {
                    td.Add(Direction.bottom);
                }
            }
            if (IsInsideBoundaries(xCoord + 1, yCoord + 1))
            {
                if (tilesInMap[xCoord + 1, yCoord + 1].GetComponent<TileObject>().tile == tile)
                {
                    td.Add(Direction.topRight);
                }
            }
            if (IsInsideBoundaries(xCoord + 1, yCoord - 1))
            {
                if (tilesInMap[xCoord + 1, yCoord - 1].GetComponent<TileObject>().tile == tile)
                {
                    td.Add(Direction.bottomRight);
                }
            }
            if (IsInsideBoundaries(xCoord - 1, yCoord + 1))
            {
                if (tilesInMap[xCoord - 1, yCoord + 1].GetComponent<TileObject>().tile == tile)
                {
                    td.Add(Direction.topLeft);
                }
            }
            if (IsInsideBoundaries(xCoord - 1, yCoord - 1))
            {
                if (tilesInMap[xCoord - 1, yCoord - 1].GetComponent<TileObject>().tile == tile)
                {
                    td.Add(Direction.bottomLeft);
                }
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
