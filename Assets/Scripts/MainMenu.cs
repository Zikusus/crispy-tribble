using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsScreen;
    public GameObject graphicsScreen;
    public GameObject audioScreen;
    public GameObject mouseKeyboardScreen;

    public void StartButton()
    {
        SceneManager.LoadScene("Testing");
    }

    public void SettingsButtonOpen()
    {
        settingsScreen.SetActive(true);
    }

    public void SettingsButtonClose()
    {
        settingsScreen.SetActive(false);
    }

    public void GraphicsButtonOpen()
    {
        graphicsScreen.SetActive(true);
    }

    public void GraphicsButtonClose()
    {
        graphicsScreen.SetActive(false);
    }

    public void AudioButtonOpen()
    {
        audioScreen.SetActive(true);
    }

    public void AudioButtonClose()
    {
        audioScreen.SetActive(false);
    }

    public void MouseKeyboardButtonOpen()
    {
        mouseKeyboardScreen.SetActive(true);
    }

    public void MouseKeyboardButtonClose()
    {
        mouseKeyboardScreen.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
