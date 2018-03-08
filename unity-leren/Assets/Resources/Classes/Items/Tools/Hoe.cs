using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tools/Hoe")]
public class Hoe : Item
{
    public override void Use()
    {
        base.Use();
        AudioSource audio = GameObject.Find("GameManager").GetComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = 0.05f;
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
            audio.Play();
            mapBuilder.CreateTile(mapBuilder.tiles[(int)tileType.farmland], Mathf.RoundToInt(mousePos.x) , Mathf.RoundToInt(mousePos.y));
            mapBuilder.CombineTilesInMap();
        }
    }
}