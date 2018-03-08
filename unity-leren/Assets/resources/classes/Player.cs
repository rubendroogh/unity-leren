using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public int health;
    public int xTilePos;
    public int yTilePos;

    public int maxHealth;
    public Inventory inventory;

    private void Update()
    {
        xTilePos = Mathf.RoundToInt(gameObject.transform.position.x * (1 / 0.16f));
        yTilePos = Mathf.RoundToInt(gameObject.transform.position.y * (1 / 0.16f));
    }

    public void Heal(int healthReturn)
    {
        if (health < maxHealth)
        {
            health += healthReturn;
            health = (health > maxHealth) ? maxHealth : health;
        }
    }
}

