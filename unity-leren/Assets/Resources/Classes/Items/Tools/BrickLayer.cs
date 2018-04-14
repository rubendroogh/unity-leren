using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tools/BrickLayer")]
public class BrickLayer : Item
{
    public int maxDistance = 4;

    public override void Use()
    {
        base.Use();
        MapBuilder mapBuilder = GameObject.Find("GameManager").GetComponent<MapBuilder>();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // convert to coords
        mousePos.x = mousePos.x * (1/0.16f);
        mousePos.y = mousePos.y * (1/0.16f);

        // calc distance between mousepos and player
        float xPosDiff = mousePos.x - holder.xTilePos;
        float yPosDiff = mousePos.y - holder.yTilePos;

        // make positive number
        xPosDiff = (xPosDiff < 0) ? xPosDiff * -1 : xPosDiff;
        yPosDiff = (yPosDiff < 0) ? yPosDiff * -1 : yPosDiff;

        if (xPosDiff <= maxDistance && yPosDiff <= maxDistance)
        {
            mapBuilder.CreateTile(mapBuilder.tiles[(int)tileType.wall], Mathf.RoundToInt(mousePos.x) , Mathf.RoundToInt(mousePos.y));
        }
    }
}