using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tools/BrickLayer")]
public class BrickLayer : Item
{
    public override void Use()
    {
        base.Use();
        MapBuilder mapBuilder = GameObject.Find("GameManager").GetComponent<MapBuilder>();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.x = mousePos.x * (1/0.16f);
        mousePos.y = mousePos.y * (1/0.16f);

        float xPosDiff = mousePos.x - holder.xTilePos;
        float yPosDiff = mousePos.y - holder.yTilePos;

        xPosDiff = (xPosDiff < 0) ? xPosDiff * -1 : xPosDiff;
        yPosDiff = (yPosDiff < 0) ? yPosDiff * -1 : yPosDiff;

        if (xPosDiff <= 4 && yPosDiff <= 4)
        {
            mapBuilder.CreateTile(mapBuilder.tiles[(int)tileType.wall], Mathf.RoundToInt(mousePos.x) , Mathf.RoundToInt(mousePos.y));
            mapBuilder.CombineTilesInMap();
        }
    }
}