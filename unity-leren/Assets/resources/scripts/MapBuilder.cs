using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour {

    public GameObject[] tiles;
    public float xPos = 0;

	void Start () {
        for (int yPos = 0; yPos < 3; yPos++)
        {
            Instantiate(tiles[0], new Vector3(xPos, yPos, 0), new Quaternion(0, 0, 0, 0));
            for (int xPos = 1; xPos < 4; xPos++)
            {
                Instantiate(tiles[0], new Vector3(xPos, yPos, 0), new Quaternion(0, 0, 0, 0));
            }
        }
    }
}
