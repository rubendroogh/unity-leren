using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private Player player;
    private Text UIHealth;

    private void Start()
    {
        UIHealth = GameObject.Find("UIHealth").GetComponent<Text>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        UIHealth.text = player.health.ToString();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
