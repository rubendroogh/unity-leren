using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tools/Hoe")]
public class Hoe : Item
{
    private int maxDistance = 4;

    public override void Use()
    {
        base.Use();
        AudioSource audio = GameObject.Find("GameManager").GetComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = 0.05f;
        MapBuilder mapBuilder = GameObject.Find("GameManager").GetComponent<MapBuilder>();
        Vector2 mousePos = GridSystem.GridToUnityCoord(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Vector2 holderPos = new Vector2(holder.xTilePos, holder.yTilePos);

        Vector2 distance = TileSystem.GridDistance(mousePos, holderPos);

        if (distance.x <= maxDistance && distance.y <= maxDistance)
        {
            mapBuilder.CreateTile(TileSystem.tiles["Farmland"], Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
            audio.Play();
            mapBuilder.CombineTiles(mapBuilder.lastCreatedTile, mousePos);
        }
    }
    
}