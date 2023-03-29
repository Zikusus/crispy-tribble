using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInput : MonoBehaviour
{
    HotbarUI hotbarUI;
    InputManager inputManager;
    public GameObject itemLocation;
    public bool[] isEquipped;
    GameObject item;
    public GameObject droppedItems;
    public int key;
    public int lastKey;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        hotbarUI = GetComponent<HotbarUI>();
    }

    private void Update()
    {
        keyPressed();
        if (Input.GetKeyDown(key.ToString()) && hotbarUI.isStored[key-1]) // Slot 1,2,3,4 .... inputManager.equipItemOne
        {
            if (lastKey != key)
            {
                //if(itemOne!=null)
                //Destroy(itemOne.gameObject);
                Destroy(item);
                isEquipped[lastKey - 1] = false;
            }
            if (!isEquipped[key - 1])
            {
                item = (GameObject)Instantiate(hotbarUI.slots[key-1], itemLocation.transform.position, itemLocation.transform.rotation);
                item.transform.SetParent(itemLocation.transform); // the false argument lets the item keep its local rotation
                
                isEquipped[key - 1] = true;

                item.GetComponent<BoxCollider>().enabled = false; // false so you won't pick it again while you're holding it because the raycast will hit it
            }
            else
            {
                Destroy(item);
                isEquipped[key - 1] = false;
            }
            lastKey = key;
        }

        if(inputManager.dropItemInput && isEquipped[lastKey-1])
        {
            droppedItems = (GameObject)Instantiate(hotbarUI.slots[lastKey-1],itemLocation.transform.position,itemLocation.transform.rotation);
            // use the slots[] name so you can pick it up again using (GameObject)Resources.Load(hit.collider.gameObject.name, typeof(GameObject)) in the HotbarFunctionality
            droppedItems.name = hotbarUI.slots[lastKey-1].gameObject.name;
            Destroy(item);

            hotbarUI.images[lastKey-1].sprite = hotbarUI.slots[lastKey-1].GetComponent<Sprite>();
            hotbarUI.slots[lastKey - 1] = null;

            hotbarUI.isStored[lastKey - 1] = false;
            isEquipped[lastKey - 1] = false;
        }
    }

    private void keyPressed()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            key = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            key = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            key = 3;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            key = 4;
        }
    }
}
