using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    public void OnReStartButtonClick()//�������¿�ʼ��ťΪ����������
    {
        UIController.Instance.SetGameState(UIController.GameState.MainMenu);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
