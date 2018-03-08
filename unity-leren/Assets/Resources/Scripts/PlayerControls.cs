using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    top, topRight, right, bottomRight, bottom, bottomLeft, left, topLeft
}

public class PlayerControls : MonoBehaviour {

    public Camera mainCamera;
    public Inventory inventory;
    public Sprite[] playerSprites;
    public float speed = 1;
    public bool isWalking = false;
    public Direction walkDirection = Direction.bottom;

    private Animator anim;
    private SpriteRenderer sprite;
    private Rigidbody2D body;
    private Player player;
    private Vector3 mousePos;
    private GameObject inventoryPanel;
    private GameObject inventoryDescription;
    private MapBuilder mapBuilder;

    [SerializeField]
    private GameObject exitButton;

    private float camSize;
    private float minCamSize = .9f;
    private float maxCamSize = 2.5f;
    private float camSizeSensitivity = 2f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        inventoryPanel = GameObject.Find("Inventory");
        inventoryDescription = GameObject.Find("InventoryDescription");
        exitButton.SetActive(false);
        inventoryPanel.SetActive(false);
        inventoryDescription.SetActive(false);

        sprite = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        mapBuilder = GameObject.Find("GameManager").GetComponent<MapBuilder>();
    }

    void Update()
    {
        CameraControls();
        ControlKeys();
        PlayerLooksAtMouse();
        UseItem();

        if (body.velocity != new Vector2(0, 0))
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        anim.SetBool("isWalking", isWalking);
        anim.SetInteger("walkDirection", (int)walkDirection);
    }

    void CameraControls()
    {
        mainCamera.transform.position = (transform.position - new Vector3(0, 0, 5));
        camSize = Camera.main.orthographicSize;
        camSize -= Input.GetAxis("Mouse ScrollWheel") * camSizeSensitivity;
        camSize = Mathf.Clamp(camSize, minCamSize, maxCamSize);
        Camera.main.orthographicSize = camSize;
    }

    void ControlKeys()
    {
        if (Input.GetKey("w"))
        {
            body.velocity += new Vector2(0, 3f * speed);
        }
        if (Input.GetKey("s"))
        {
            body.velocity += new Vector2(0, -3f * speed);
        }
        if (Input.GetKey("a"))
        {
            body.velocity += new Vector2(-3f * speed, 0);
        }
        if (Input.GetKey("d"))
        {
            body.velocity += new Vector2(3f * speed, 0);
        }
        if (Input.GetKeyDown("w"))
        {
            walkDirection = Direction.top;
        }
        if (Input.GetKeyDown("s"))
        {
            walkDirection = Direction.bottom;
        }
        if (Input.GetKeyDown("a"))
        {
            walkDirection = Direction.left;
        }
        if (Input.GetKeyDown("d"))
        {
            walkDirection = Direction.right;
        }
        if (Input.GetKey("e"))
        {
            PickUpItem();
        }
        if (Input.GetKeyDown("q"))
        {
            OpenInventory(inventoryPanel.activeSelf);
        }
        if (Input.GetKeyDown("r"))
        {
            mapBuilder.CreateTile(mapBuilder.tiles[(int)tileType.wall], player.xTilePos, player.yTilePos);
            mapBuilder.CombineTilesInMap();
        }
    }

    void PlayerLooksAtMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get angles
        float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        float angleDeg = (180 / Mathf.PI) * angleRad;

        // if it's downwards
        if (angleDeg < -45 && angleDeg > -135)
        {
            sprite.sprite = playerSprites[0];
        }
        // if it's to the left
        if (angleDeg < -135 || angleDeg > 135)
        {
            sprite.sprite = playerSprites[2];
        }
        // if it's upwards
        if (angleDeg > 45 && angleDeg < 135)
        {
            sprite.sprite = playerSprites[1];
        }
        // if it's to the right
        if (angleDeg < 45 && angleDeg > -45)
        {
            sprite.sprite = playerSprites[3];
        }
    }

    void PickUpItem()
    {
        RaycastHit2D itemHit = Physics2D.Raycast(this.transform.position, Vector2.zero);
        if (itemHit)
        {
            if (itemHit.collider.CompareTag("Item"))
            {
                inventory.AddItem(itemHit.collider.GetComponent<PlaceableItem>().item);
                Destroy(itemHit.collider.gameObject);
            }
        }
    }

    void UseItem()
    {
        if (Input.GetMouseButtonDown(1) && inventory.hotbarItems[inventory.hotbarSelectNum])
        {
            inventory.hotbarItems[inventory.hotbarSelectNum].Use();
        }
    }

    void OpenInventory(bool state)
    {
        inventoryPanel.SetActive(!state);
        inventoryDescription.SetActive(!state);
        exitButton.SetActive(!state);
    }
}
