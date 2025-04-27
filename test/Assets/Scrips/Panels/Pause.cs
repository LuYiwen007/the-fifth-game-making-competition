using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GamePlayerLogic player;
    public void OnReSpwamButtonClick()//��������ҴӼ�¼������
    {
        if (GameManager.Instance.hasrespwam)
        {
            player.PlayerRespwam();
            UIController.Instance.SetGameState(UIController.GameState.InGame);
        }
        else
        {
            OnReStartButtonClick();
        }
    }
    public void OnReStartButtonClick()//������������¿�ʼ�ùؿ�
    {
        GameManager.Instance.RestartLevel();
        UIController.Instance.SetGameState(UIController.GameState.InGame);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
