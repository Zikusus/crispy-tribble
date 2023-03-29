using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    public bool[] isStored;
    public GameObject[] slots;
    //public Sprite[] sprites;
    public Image[] images;

    private void Awake()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            images[i] = slots[i].GetComponent<Image>();
        }
    }
}
