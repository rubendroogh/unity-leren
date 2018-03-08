using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lake {

    public int startXCoord;
    public int startYCoord;

    private int xLakeSize;
    private int yLakeSize;
 
    public int minXLakeSize = 6;
    public int maxXLakeSize = 40;

    private int minYLakeSize;
    private int maxYLakeSize;

    public float[,] lakeMap;

    public void GenerateLake(int xMapSize, int yMapSize, MapBuilder mapBuilder)
    {
        // if no size is specified, make a random one
        xLakeSize = (xLakeSize == 0) ? Random.Range(minXLakeSize, maxXLakeSize) : xLakeSize;
        yLakeSize = xLakeSize + Random.Range(xLakeSize / -xLakeSize, xLakeSize / xLakeSize);

        // starting location of the lake, don't spawn too close to player spawn (at least 10 tiles difference)
        startXCoord = Random.Range(0, xMapSize);
        startYCoord = (startXCoord <= 10) ? Random.Range(10, yMapSize) : Random.Range(0, yMapSize);

        // build the rest of the lake tiles
        for (int x = 0; x < xLakeSize/2; x++)
        {
            for (int y = 0; y < yLakeSize/2; y++)
            {
                mapBuilder.CreateTile(mapBuilder.tiles[(int)tileType.water], startXCoord + x, startYCoord + y);
                // growthChance: 1 at the center, lower near the edges
                AddExtraLakeDetails(x, y, mapBuilder);
            }
        }
    }

    void AddExtraLakeDetails(int x, int y, MapBuilder mapBuilder)
    {
        float xGrowthChance = (x < 0) ? x * -1 : x;
        xGrowthChance = (xGrowthChance == 0) ? 1 : xGrowthChance;
        xGrowthChance = 1 / xGrowthChance;
        if (Random.Range(0.0f, 1.0f) < xGrowthChance * 3)
        {
            mapBuilder.CreateTile(mapBuilder.tiles[(int)tileType.water], startXCoord + x, startYCoord + y + 1);
            mapBuilder.CreateTile(mapBuilder.tiles[(int)tileType.water], startXCoord + x, startYCoord + y - 1);
        }

        float yGrowthChance = (y < 0) ? y * -1 : y;
        yGrowthChance = (yGrowthChance == 0) ? 1 : yGrowthChance;
        yGrowthChance = 1 / yGrowthChance;
        if (Random.Range(0.0f, 1.0f) < yGrowthChance * 3)
        {
            mapBuilder.CreateTile(mapBuilder.tiles[(int)tileType.water], startXCoord + x + 1, startYCoord + y);
            mapBuilder.CreateTile(mapBuilder.tiles[(int)tileType.water], startXCoord + x - 1, startYCoord + y);
        }
    }
}
