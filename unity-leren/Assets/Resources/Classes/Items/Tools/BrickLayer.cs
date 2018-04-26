using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tools/BrickLayer")]
public class BrickLayer : Item
{
    public int maxDistance = 4;

    public override void Use()
    {
        MapBuilder mapBuilder = GameObject.Find("GameManager").GetComponent<MapBuilder>();
        Vector2 mousePos = GridSystem.GridToUnityCoord(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Vector2 holderPos = new Vector2(holder.xTilePos, holder.yTilePos);

        Vector2 distance = TileSystem.GridDistance(mousePos, holderPos);

        if (distance.x <= maxDistance && distance.y <= maxDistance)
        {
            mapBuilder.CreateTile(TileSystem.tiles["Tree"], Mathf.RoundToInt(mousePos.x) , Mathf.RoundToInt(mousePos.y));
        }
    }
}