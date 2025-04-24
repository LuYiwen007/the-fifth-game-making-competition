using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GamePlayerLogic;

public class Pause : MonoBehaviour
{
    GamePlayerLogic player;
    public void OnReSpwamButtonClick()//这里让玩家从记录点重生
    {
        if (GameManager.Instance.hasrespwam)
        {
            player.RESpwam();
            UIController.Instance.SetGameState(UIController.GameState.InGame);
        }
    }
    public void OnReStartButtonClick()//这里让玩家重新开始该关卡
    {
        GameManager.Instance.RestartLevel();
        UIController.Instance.SetGameState(UIController.GameState.InGame);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
