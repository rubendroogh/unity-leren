using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileCreatable {

    void PlaceTile(Tile tileToPlace);
    Dictionary<string, Tile> GetAllTiles();

}
