using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverPanel : MonoBehaviour
{
    public void OnReStartButtonClick()//������������¿�ʼʧ�ܹؿ�
    {
        GameManager.Instance.RestartLevel();
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
