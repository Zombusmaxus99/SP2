using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{

    public GameObject creditsPanel;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void AccessCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
