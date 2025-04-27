using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GamePlayerLogic;

public class OverPanel : MonoBehaviour
{
    GamePlayerLogic player;
    public void OnReSpwamButtonClick()//这里让玩家从记录点重生
    {
        if (GameManager.Instance.hasrespwam&&player.currenthp!=0)
        {
            player.PlayerRespwam();
            UIController.Instance.SetGameState(UIController.GameState.InGame);
        }
        else
        {
            Debug.Log("血量不足，无法复活");
        }
    }
    public void OnReStartButtonClick()//这里让玩家重新开始失败关卡
    {
        GameManager.Instance.RestartLevel();
        UIController.Instance.SetGameState(UIController.GameState.InGame);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
