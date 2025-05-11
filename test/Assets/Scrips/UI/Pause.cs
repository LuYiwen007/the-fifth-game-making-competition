using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GamePlayerLogic player;

    public void OnContinueButtonClick()//��������Ҽ�����Ϸ
    {
        player.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        UIController.Instance.SetGameState(UIController.GameState.InGame);
    }

    public void OnReSpwamButtonClick()//��������ҴӼ�¼������
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
    public void OnReStartButtonClick()//������������¿�ʼ�ùؿ�
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
