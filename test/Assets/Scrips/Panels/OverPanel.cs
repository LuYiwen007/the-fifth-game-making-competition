using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverPanel : MonoBehaviour
{
    public GamePlayerLogic player;
    public void OnReSpwamButtonClick()//��������ҴӼ�¼������
    {
        if (GameManager.Instance.hasrespwam && player.currenthp != 0) 
        {
            player.PlayerRespwam();
            UIController.Instance.SetGameState(UIController.GameState.InGame);
        }
        else
        {
            Debug.Log("Ѫ�������û�м�¼�㣬�޷�����");
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
