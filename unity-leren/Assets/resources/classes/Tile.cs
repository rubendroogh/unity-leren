using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Tile : ScriptableObject {

    public string tileName;
    public Sprite[] sprites;
    public bool passable;
    public bool isGroundTile;
    public bool combinable;
    public int layer = 0;

    private SpriteRenderer spriteRenderer;
}
