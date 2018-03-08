using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure {

	public Structure(int xMapSize, int yMapSize, MapBuilder mapBuilder)
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                mapBuilder.CreateTile(mapBuilder.tiles[(int)tileType.stonetile], x, y);
            }
        }
    }
}
