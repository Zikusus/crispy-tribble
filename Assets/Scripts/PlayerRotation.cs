using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float lockPos;
    public Transform Protation;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(lockPos, Protation.rotation.eulerAngles.y, lockPos);
    }
}
