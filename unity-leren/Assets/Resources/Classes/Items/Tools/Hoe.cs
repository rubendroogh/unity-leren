using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tools/Hoe")]
public class Hoe : Item
{
    private int maxDistance = 4;

    public override void Use()
    {
        MapController mapBuilder = GameObject.Find("GameManager").GetComponent<MapController>();
        GridPoints mousePos = GridPoints.MousePosToGridPoints(Input.mousePosition);
        GridPoints holderPos = new GridPoints(holder.xTilePos, holder.yTilePos);
        GridPoints distance = GridPoints.GridDistance(mousePos, holderPos);

        if (distance.x <= maxDistance && distance.y <= maxDistance)
        {
            mapBuilder.CreateTile(TileSystem.tiles["Farmland"], Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
            PlayAudio(clip, 0.05f);
        }
    }
}