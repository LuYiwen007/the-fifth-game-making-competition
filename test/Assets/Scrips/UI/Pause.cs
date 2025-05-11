using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GamePlayerLogic player;

    public void OnContinueButtonClick()//这里让玩家继续游戏
    {
        player.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        UIController.Instance.SetGameState(UIController.GameState.InGame);
    }

    public void OnReSpwamButtonClick()//这里让玩家从记录点重生
    {
        if (GameManager.Instance.hasrespwam)
        {
            player.gameObject.GetComponent<Rigidbody2D>().simulated = true;
            player.PlayerRespwam();
            UIController.Instance.SetGameState(UIController.GameState.InGame);
        }
        else
        {
            OnReStartButtonClick();
        }
    }
    public void OnReStartButtonClick()//这里让玩家重新开始该关卡
    {
        player.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        GameManager.Instance.RestartLevel();
        UIController.Instance.SetGameState(UIController.GameState.InGame);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
