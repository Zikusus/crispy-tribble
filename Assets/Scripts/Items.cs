using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "Create new Item")]
//[System.Serializable]
public class Items : MonoBehaviour //ScriptableObject
{
    public int id;
    public string itemName;

    [TextArea(2, 2)] public string description;

    public GameObject prefab;
    public Sprite icon;
}
