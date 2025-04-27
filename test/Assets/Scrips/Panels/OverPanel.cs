using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GamePlayerLogic;

public class OverPanel : MonoBehaviour
{
    GamePlayerLogic player;
    public void OnReSpwamButtonClick()//��������ҴӼ�¼������
    {
        if (GameManager.Instance.hasrespwam&&player.currenthp!=0)
        {
            player.PlayerRespwam();
            UIController.Instance.SetGameState(UIController.GameState.InGame);
        }
        else
        {
            Debug.Log("Ѫ�����㣬�޷�����");
        }
    }
    public void OnReStartButtonClick()//������������¿�ʼʧ�ܹؿ�
    {
        GameManager.Instance.RestartLevel();
        UIController.Instance.SetGameState(UIController.GameState.InGame);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
