using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MainMenu : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        GameManager.Instance.StartGame();
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
