using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
        InitializeLevelLogic();
    }
    public void NextLevel()
    {
        CurrentLevel++;
        Debug.Log($"当前关卡: {CurrentLevel}");
        InitializeLevelLogic();
    }
    private void InitializeLevelLogic()//初始化关卡
    {   
        //初始化玩家
        player.InitializePlayer();
        
        //初始化颜料值
        player.SetBlackPaintValue(0);
        player.SetWhitePaintValue(0);

        //初始化陷阱
        trapLogic.StartTrapCycle();

        //初始化背包
        Inventory.Instance.InitializeInventory();

        switch (CurrentLevel)
        {
            //case 0:
            //    Debug.Log("��Ϸ��ʼ����ʼ���������߼�");
            //    //SceneManager.LoadScene("MainMenu");
                  //break;
            case 1:
                Debug.Log("初始化关卡一");
                //SceneManager.LoadScene("Level1");
                break;
            case 2:
                Debug.Log("初始化关卡二");
                //SceneManager.LoadScene("Level2");
                break;
            case 3:
                Debug.Log("初始化关卡三");
                //SceneManager.LoadScene("Level3"); 

                break;
            default:
                ////SceneManager.LoadScene("MainMenu");
                UIController.Instance.SetGameState(UIController.GameState.Win);
                break;
        }
    }
   
    public void RestartLevel()
    {
        Debug.Log($"重新开始当前关卡: {CurrentLevel}");
        InitializeLevelLogic();
    }
    public void StartGame()
    {
        UIController.Instance.SetGameState(UIController.GameState.InGame);
    }

    public void GameOver()
    {
        UIController.Instance.SetGameState(UIController.GameState.GameOver);
    }
}
