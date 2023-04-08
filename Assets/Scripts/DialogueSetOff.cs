using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSetOff : MonoBehaviour
{
    public DialogueSystem dialogue;
    private bool playerSpoted;

    private void Update()
    {
        Debug.Log(playerSpoted);
        if (playerSpoted && Input.GetKeyDown(KeyCode.E))
        {
            dialogue.StartDialogue();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Fox")
        {
            playerSpoted = true;
            dialogue.ToggleIndicator(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Fox")
        {
            playerSpoted = false;
            dialogue.ToggleIndicator(false);
            dialogue.EndDialogue();
        }
    }
}
