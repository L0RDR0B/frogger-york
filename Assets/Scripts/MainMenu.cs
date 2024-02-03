using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject controls;

    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading " + sceneName + "...");
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

    public void OpenControls()
    {
        controls.SetActive(true);
    }

    public void CloseControls()
    {
        controls.SetActive(false);
    }
}
