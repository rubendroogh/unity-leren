using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject {

    public bool canBePickedUp;
    public string itemGroup;
    public string itemName;
    public string description;
    public Sprite sprite;

    [SerializeField]
    protected AudioClip clip;

    [System.NonSerialized]
    public Player holder;

    public virtual void Use()
    {
        // do something
    }
}