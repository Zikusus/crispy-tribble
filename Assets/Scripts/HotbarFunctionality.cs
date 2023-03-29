using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HotbarFunctionality : MonoBehaviour
{
    public HotbarUI hotbarUI;
    public GameObject cameraCenter;
    public GameObject pickUpText;
    public int mask;

    InputManager inputManager;

    private void Awake()
    {
        mask = 1 << LayerMask.NameToLayer("Items"); // Learn more about this 

        hotbarUI = GetComponent<HotbarUI>();
        inputManager = GetComponent<InputManager>();
    }
    void Update()
    {
        //The local position of the cameraCenter. Mostly interested in the local z direction of the cameraCenter.
        Vector3 localForward = cameraCenter.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(cameraCenter.transform.position,localForward, out hit, 2f,mask))
        {
            pickUpText.SetActive(true);
            if (inputManager.pickUpInput)
            {
                for (int i = 0; i < hotbarUI.slots.Length; i++)
                {
                    if (hotbarUI.isStored[i] == false)
                    {
                        //hotbarUI.images[i].color = Color.red;
                        hotbarUI.images[i].sprite = hit.collider.gameObject.GetComponent<Items>().icon;
                        //hotbarUI.slots[i] = hit.collider.gameObject;
                        hotbarUI.slots[i] = (GameObject)Resources.Load(hit.collider.gameObject.name, typeof(GameObject));
                        hotbarUI.isStored[i] = true;

                        Destroy(hit.collider.gameObject);

                        break;
                    }
                }
            }
            // If the ray hits a collider, do something with the hit object
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
        else
        {
            pickUpText.SetActive(false);
        }
        Debug.DrawRay(cameraCenter.transform.position,localForward*2, Color.yellow);
    }
}
