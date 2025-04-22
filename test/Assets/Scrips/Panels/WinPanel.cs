using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    public void OnReStartButtonClick()//这里重新开始按钮为返回主界面
    {
        UIController.Instance.SetGameState(UIController.GameState.MainMenu);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
