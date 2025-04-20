using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int CurrentLevel { get; private set; } = 1; // 当前关卡，默认为0，表示游戏开始前的状态
    public TrapLogic trapLogic; // 绑定场景中的 TrapLogic 脚本
    public GamePlayerLogic playerLogic;
    public UnityEvent OnDeath;

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
        InitializeLevelLogic();
    }
    public void NextLevel()//加载下一个关卡
    {
        CurrentLevel++;
        Debug.Log($"当前关卡: {CurrentLevel}");
        InitializeLevelLogic();
    }
    private void InitializeLevelLogic()//初始化关卡逻辑
    {   
        //初始化玩家
        playerLogic.InitializePlayer();
        
        //重置颜料值
        playerLogic.SetBlackPaintValue(0);
        playerLogic.SetWhitePaintValue(0);

        //启动陷阱逻辑
        trapLogic.StartTrapCycle();

        //初始化背包
        Inventory.Instance.InitializeInventory();

        switch (CurrentLevel)
        {
            case 0:
                Debug.Log("游戏开始，初始化主界面逻辑");
                //SceneManager.LoadScene("MainMenu");
                // 在这里添加游戏开始时的初始化逻辑
                break;
            case 1:
                Debug.Log("初始化第一关逻辑");
                //SceneManager.LoadScene("Level1");
                // 在这里添加第一关的初始化逻辑
                break;
            case 2:
                Debug.Log("初始化第二关逻辑");
                //SceneManager.LoadScene("Level2");
                // 在这里添加第二关的初始化逻辑
                break;
            case 3:
                Debug.Log("初始化第三关逻辑");
                //SceneManager.LoadScene("Level3"); 

                break;
            default:
                Debug.Log("游戏完成，加载主菜单或其他场景");//在这里加载主菜单或其他场景
                //SceneManager.LoadScene("MainMenu");
                break;
        }
    }

    // 死亡方法
    public void Die()
    {
        Debug.Log("角色死亡");
        // 触发死亡事件
        OnDeath?.Invoke();

        // 禁用控制器
        enabled = false;

        //展示死亡ui
    }
    public void RestartLevel()//重新开始当前关卡
    {
        Debug.Log($"重新开始当前关卡: {CurrentLevel}");
        InitializeLevelLogic();
    }
}
