using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour {

    public Tile tile;
    public GameObject gm;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!tile.combinable)
        {
            spriteRenderer.sprite = tile.sprites[Random.Range(0, tile.sprites.Length)];
        }
        SetTileCollision();
        SetTileLayer();
    }

    private void SetTileCollision()
    {
        if (!tile.passable)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.16f, 0.16f);

        }
    }

    private void SetTileLayer()
    {
        spriteRenderer.sortingOrder = (tile.isGroundTile) ? 0 : 1;
    }
}
