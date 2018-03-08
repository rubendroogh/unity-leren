using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public int maxItems;
    public Player player;
    public GameObject itemSlot;
    public GameObject itemListContainer;
    public GameObject itemDescription;
    public Image itemDescImage;
    public Text itemDescNameText;
    public Text itemDescText;
    public GameObject[] hotbarSlots;
    public Image[] hotbarSlotImages;
    public Item[] hotbarItems;
    public int hotbarSelectNum;

    private GameObject[] itemSlots;
    private Text[] itemListNames;
    private Item[] items;
    private GameObject hotbarSelect;
    private Image hotbarSelectImage;

    private void Start()
    {
        itemSlots = new GameObject[maxItems];
        itemListNames = new Text[itemSlots.Length];
        items = new Item[itemSlots.Length];
        hotbarItems = new Item[3];

        hotbarSelect = hotbarSlots[0];
        hotbarSelectImage = hotbarSlotImages[0];
    }

    public void AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                itemSlots[i] = AddItemSlot(i);
                itemListNames[i] = itemSlots[i].transform.Find("ItemListName").GetComponent<Text>();
                items[i] = itemToAdd;
                itemListNames[i].text = itemToAdd.name;
                itemToAdd.holder = player;
                return;
            }
        }
    }

    public void RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemToRemove)
            {
                RemoveItemSlot(i);
                items[i] = null;
                itemListNames[i].text = null;
                return;
            }
        }
    }

    private GameObject AddItemSlot(int i)
    {
        itemSlots[i] = Instantiate(itemSlot);
        itemSlots[i].transform.SetParent(itemListContainer.transform);
        itemSlots[i].transform.localScale = new Vector3(1, 1, 1);
        itemSlots[i].transform.localPosition = new Vector3(0, 0, 0);
        itemSlots[i].name = i.ToString();
        return itemSlots[i];
    }

    private void RemoveItemSlot(int i)
    {
        Destroy(itemSlots[i]);
    }

    private void AddToHotbar(Item itemToAdd)
    {
        if (hotbarItems[hotbarSelectNum] != null)
        {
            RemoveFromHotbar(hotbarSelectNum, true);
        }
        hotbarSelectImage.enabled = true;
        hotbarSelectImage.sprite = itemToAdd.sprite;
        RemoveItem(itemToAdd);
        hotbarItems[hotbarSelectNum] = itemToAdd;
    }

    public void RemoveFromHotbar(int HotbarSlot, bool backToInventory = false)
    {
        hotbarSelectImage.enabled = false;
        if (backToInventory)
        {
            AddItem(hotbarItems[HotbarSlot]);
        }
        hotbarItems[hotbarSelectNum] = null;
    }

    private void Update()
    {
        // shows the item description when hovering over item
        RaycastHit2D rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (rayHit)
        {
            if (rayHit.collider.tag == "ListItem")
            {
                Item itemToShow = items[Int32.Parse(rayHit.collider.name)];
                itemDescImage.enabled = true;
                itemDescImage.sprite = itemToShow.sprite;
                itemDescNameText.text = itemToShow.name;
                itemDescText.text = itemToShow.description;

                if (Input.GetMouseButtonDown(0))
                {
                    AddToHotbar(itemToShow);
                }
            }
        }

        // controls for hotbar
        if (Input.GetKeyDown("1"))
        {
            hotbarSlots[0].GetComponent<Image>().color = Color.red;
            hotbarSlots[1].GetComponent<Image>().color = Color.white;
            hotbarSlots[2].GetComponent<Image>().color = Color.white;
            hotbarSelect = hotbarSlots[0];
            hotbarSelectImage = hotbarSlotImages[0];
            hotbarSelectNum = 0;
        }
        else if (Input.GetKeyDown("2"))
        {
            hotbarSlots[0].GetComponent<Image>().color = Color.white;
            hotbarSlots[1].GetComponent<Image>().color = Color.red;
            hotbarSlots[2].GetComponent<Image>().color = Color.white;
            hotbarSelect = hotbarSlots[1];
            hotbarSelectImage = hotbarSlotImages[1];
            hotbarSelectNum = 1;
        }
        else if (Input.GetKeyDown("3"))
        {
            hotbarSlots[0].GetComponent<Image>().color = Color.white;
            hotbarSlots[1].GetComponent<Image>().color = Color.white;
            hotbarSlots[2].GetComponent<Image>().color = Color.red;
            hotbarSelect = hotbarSlots[2];
            hotbarSelectImage = hotbarSlotImages[2];
            hotbarSelectNum = 2;
        }
    }
}
