using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueChatBox;
    public GameObject dialogueSignal;
    public TMP_Text dialogueText;

    public List<string> dialogues;

    public float typingSpeed;

    private int index;
    private int cIndex; //the index character

    private bool started;
    private bool startNextSentence; // wait for next

    private void Awake()
    {
        ToggleIndicator(false);
        ToggleWindow(false);
    }

    private void Update()
    {
        if (!started)
            return;

        if (startNextSentence && Input.GetKeyDown(KeyCode.E))
        {
            startNextSentence = false;
            index++;

            if (index < dialogues.Count)
            {
                GetDialogue(index);
            }
            else
            {
                ToggleIndicator(true);
                EndDialogue();
            }
        }
    }

    private void ToggleWindow(bool value)
    {
        dialogueChatBox.SetActive(value);
    }
    public void ToggleIndicator(bool value)
    {
        dialogueSignal.SetActive(value);
    }

    public void StartDialogue()
    {
        if (started)
            return;

        started = true;

        ToggleWindow(true);
        ToggleIndicator(false);

        GetDialogue(0);   //Start with first dialogue
    }

    private void GetDialogue(int i)
    {
        index = i;  //start index at zero
        cIndex = 0;

        dialogueText.text = ""; // or = "" to clear the chat box

        StartCoroutine(Writing());
    }

    public void EndDialogue()
    {
        started = false; 
        startNextSentence = false; //Disable wait for next as well

        StopAllCoroutines();
        ToggleWindow(false);
    }

    IEnumerator Writing()
    {
        yield return new WaitForSeconds(typingSpeed);

        string currentDialogue = dialogues[index];
        dialogueText.text += currentDialogue[cIndex];

        cIndex++;

        if (cIndex < currentDialogue.Length)
        {
            StartCoroutine(Writing());
        }
        else
        {
            startNextSentence = true; //End this sentence and wait for the next one
        }
    }

}
