using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentLevel { get; private set; } = 0; // 当前关卡，默认为0，表示游戏开始前的状态

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保证 GameManager 在场景切换时不被销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()//加载下一个关卡
    {
        CurrentLevel++;
        Debug.Log($"当前关卡: {CurrentLevel}");
        InitializeLevelLogic();
    }









    public void SetLevel(int level)//设置当前关卡
    {
        CurrentLevel = level;
        Debug.Log($"当前关卡设置为: {CurrentLevel}");
    }

    public bool IsThirdLevel()//判断是否是第三关
    {
        return CurrentLevel == 3;
    }
    public void InitializeLevelLogic()//初始化关卡逻辑
    {
        switch (CurrentLevel)
        {
            case 0:
                Debug.Log("游戏开始，初始化主界面逻辑");
                SceneManager.LoadScene("MainMenu");
                // 在这里添加游戏开始时的初始化逻辑
                break;
            case 1:
                Debug.Log("初始化第一关逻辑");
                SceneManager.LoadScene("Level1");
                // 在这里添加第一关的初始化逻辑
                break;
            case 2:
                Debug.Log("初始化第二关逻辑");
                SceneManager.LoadScene("Level2");
                // 在这里添加第二关的初始化逻辑
                break;
            case 3:
                Debug.Log("初始化第三关逻辑");
                SceneManager.LoadScene("Level3"); // 加载第三关场景
                break;
            default:
                Debug.Log("游戏完成，加载主菜单或其他场景");//在这里加载主菜单或其他场景
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }
}
