using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Camera mainCamera;
    public float speed = 1;

    private Vector3 mousePos;

    void Update()
    {
        mainCamera.transform.position = (transform.position - new Vector3(0, 0, 5));
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKey("w"))
        {
            transform.position += new Vector3(0 , 0.03f * speed , 0);
        }
        if (Input.GetKey("s"))
        {
            transform.position += new Vector3(0, -0.03f * speed, 0);
        }
        if (Input.GetKey("a"))
        {
            transform.position += new Vector3(-0.03f * speed, 0, 0);
        }
        if (Input.GetKey("d"))
        {
            transform.position += new Vector3(0.03f * speed, 0, 0);
        }

        float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        // Get Angle in Degrees
        float angleDeg = (180 / Mathf.PI) * angleRad;
        // Rotate Object
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }
}
