using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverPanel : MonoBehaviour
{
    public GamePlayerLogic player;
    public void OnReSpwamButtonClick()//这里让玩家从记录点重生
    {
        if (GameManager.Instance.hasrespwam && player.currenthp != 0) 
        {
            player.PlayerRespwam();
            UIController.Instance.SetGameState(UIController.GameState.InGame);
        }
        else
        {
            Debug.Log("血量不足或没有记录点，无法复活");
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
