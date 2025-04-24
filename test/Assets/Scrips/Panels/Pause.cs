using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GamePlayerLogic;

public class Pause : MonoBehaviour
{
    GamePlayerLogic player;
    public void OnReSpwamButtonClick()//��������ҴӼ�¼������
    {
        if (GameManager.Instance.hasrespwam)
        {
            player.RESpwam();
            UIController.Instance.SetGameState(UIController.GameState.InGame);
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
