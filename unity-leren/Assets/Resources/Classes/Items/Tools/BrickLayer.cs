using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tools/BrickLayer")]
public class BrickLayer : Item
{
    public int maxDistance = 4;

    public override void Use()
    {
        MapController mapBuilder = GameObject.Find("GameManager").GetComponent<MapController>();
        GridPoints mousePos = GridPoints.MousePosToGridPoints(Input.mousePosition);
        GridPoints holderPos = new GridPoints(holder.xTilePos, holder.yTilePos);
        GridPoints distance = GridPoints.GridDistance(mousePos, holderPos);
        
        if (distance.x <= maxDistance && distance.y <= maxDistance)
        {
            mapBuilder.CreateTile(TileSystem.tiles["Wall"], Mathf.RoundToInt(mousePos.x) , Mathf.RoundToInt(mousePos.y));
        }
    }
}