using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure {

	public Structure(int xMapSize, int yMapSize, MapController mapBuilder)
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                mapBuilder.CreateTile(TileSystem.tiles["Stone Tile"], x, y);
            }
        }
    }
}
