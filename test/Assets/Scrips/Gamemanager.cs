using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int CurrentLevel { get; private set; } = 1;
    public TrapLogic trapLogic; 
    public GamePlayerLogic player;
    public bool hasrespwam=false;
    private GameObject[] keys;

    private void Awake()
    {
        keys=GameObject.FindGameObjectsWithTag("key");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
        UIController.Instance.SetGameState(UIController.GameState.MainMenu);
    }
    public void NextLevel()
    {
        // 检查是否是最后一关
        if (CurrentLevel >= 3)
        {
            // 显示胜利面板
            UIController.Instance.SetGameState(UIController.GameState.Win);
        }
        else
        {
            CurrentLevel++;
            Debug.Log($"当前关卡: {CurrentLevel}");
            InitializeLevelLogic();
        }
    }
    private void InitializeLevelLogic()//初始化关卡，用于开始游戏和重新开始关卡
    {
        {
            //初始化玩家
            player.InitializePlayer();
            player.currenthp = player.maxhp;

            //初始化颜料值
            player.SetBlackPaintValue(0);
            player.SetWhitePaintValue(0);

            //初始化陷阱
            trapLogic.StartTrapCycle();

            //初始化背包
            Inventory.Instance.InitializeInventory();

            switch (CurrentLevel)
            {
                case 1:
                    Debug.Log("初始化关卡一");
                    player.TransfornToLevel(1);
                    //SceneManager.LoadScene("Level1");
                    break;
                case 2:
                    Debug.Log("初始化关卡二");
                    player.TransfornToLevel(2);
                    //SceneManager.LoadScene("Level2");
                    break;
                case 3:
                    Debug.Log("初始化关卡三");
                    player.TransfornToLevel(3);
                    //SceneManager.LoadScene("Level3"); 

                    break;
                default:
                    ////SceneManager.LoadScene("MainMenu");
                    UIController.Instance.SetGameState(UIController.GameState.Win);
                    break;
            }
        }
    }
   
    public void StartGame()
    {
        UIController.Instance.SetGameState(UIController.GameState.InGame);
        InitializeLevelLogic();
    }
    public void RestartLevel()
    {
        foreach (GameObject key in keys)
        {
            key.SetActive(true);
        }
        Debug.Log($"重新开始当前关卡: {CurrentLevel}");
        InitializeLevelLogic();
        hasrespwam = false;
    }
}
