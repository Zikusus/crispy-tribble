using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
   // InputManager inputManager;

    public GameObject pauseMenu;
    public bool isPaused;

    /*private void Awake()
    {
        inputManager = GetComponent<InputManager>();
    }*/

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            isPaused = !isPaused;
            ResumeOrPause();
        }
    }

    public void ResumeOrPause()
    {
        pauseMenu.SetActive(isPaused);
        Cursor.visible = isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }
}
