using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tools/Drill")]
public class Drill : Item
{
    public int drillSpeed;

    public override void Use()
    {
        base.Use();
        AudioSource audio = GameObject.Find("GameManager").GetComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = 0.2f;
        MapBuilder mapBuilder = GameObject.Find("GameManager").GetComponent<MapBuilder>();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.x = mousePos.x * (1 / 0.16f);
        mousePos.y = mousePos.y * (1 / 0.16f);

        if (mapBuilder.tilesInMap[Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y)].GetComponent<TileObject>().tile == mapBuilder.tiles[(int)tileType.wall])
        {
            mapBuilder.RemoveTile(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
            audio.Play();
        }
    }
}
