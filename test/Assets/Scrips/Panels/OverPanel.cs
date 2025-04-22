using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverPanel : MonoBehaviour
{
    public void OnReStartButtonClick()//这里让玩家重新开始失败关卡
    {
        GameManager.Instance.RestartLevel();
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
