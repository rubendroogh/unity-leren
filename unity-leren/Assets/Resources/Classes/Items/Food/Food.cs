using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Food")]
public class Food : Item {

    [SerializeField]
    private int healthReturn;
    private Player player;
    
    public Food()
    {
        itemGroup = "Food";
    }

    public override void Use()
    {
        base.Use();
        AudioSource audio = GameObject.Find("GameManager").GetComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = 0.2f;
        player = GameObject.Find("Player").GetComponent<Player>();
        player.Heal(healthReturn);
        player.inventory.RemoveFromHotbar(player.inventory.hotbarSelectNum);
        audio.Play();
    }
}
