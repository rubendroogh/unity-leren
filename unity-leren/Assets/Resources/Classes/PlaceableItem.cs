using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableItem : MonoBehaviour {

    public Item item;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.sprite;

        if (item.canBePickedUp)
        {
            BoxCollider2D colliderBox = gameObject.AddComponent<BoxCollider2D>();
            colliderBox.isTrigger = true;
        }
    }
}
