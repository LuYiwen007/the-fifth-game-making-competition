using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

//�߼�����Ϸ������Զ������ã�����ű������л���Ϸ����֮�ϵ�UI
public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    [Header("��������")]
    [SerializeField] private Transform _mainInterface;
    [SerializeField] private Transform _gameInterface;
    [SerializeField] private Transform _overInterface;
    [SerializeField] private Transform _winInterface;
    [SerializeField] private Transform _pauseInterface;
    [SerializeField] private Transform _storyInterface;

    public enum GameState
    {
        MainMenu,
        InGame,
        Pause,
        Story,
        GameOver,
        Win
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        SetGameState(GameState.MainMenu);
    }

    public void SetGameState(GameState newState)//�����л���
    {
        // �������н���
        SetAllInterfacesActive(false);

        // �����Ӧ����
        switch (newState)
        {
            case GameState.MainMenu:
                _mainInterface.gameObject.SetActive(true);
                break;
            case GameState.InGame:
                _gameInterface.gameObject.SetActive(true);
                break;
            case GameState.GameOver:
                _overInterface.gameObject.SetActive(true);
                break;
            case GameState.Win:
                _winInterface.gameObject.SetActive(true);
                break;
            case GameState.Pause:
                _pauseInterface.gameObject.SetActive(true);
                break;
            case GameState.Story:
                _storyInterface.gameObject.SetActive(true);
                break;
        }
    }

    private void SetAllInterfacesActive(bool active)
    {
        _mainInterface.gameObject.SetActive(active);
        _gameInterface.gameObject.SetActive(active);
        _overInterface.gameObject.SetActive(active);
        _winInterface.gameObject.SetActive(active);
        _pauseInterface.gameObject.SetActive(active);
        _storyInterface.gameObject.SetActive(active);
    }

    public string Currentstate()//��ȡ��ǰ����״̬
    {
        
        {
            if (_mainInterface.gameObject.activeSelf)
            {
                return "MainMenu";
            }
            else if (_gameInterface.gameObject.activeSelf)
            {
                return "InGame";
            }
            else if (_overInterface.gameObject.activeSelf)
            {
                return "GameOver";
            }
            else if (_winInterface.gameObject.activeSelf)
            {
                return "Win";
            }
            else if (_pauseInterface.gameObject.activeSelf)
            {
                return "Pause";
            }
            else if(_storyInterface.gameObject.activeSelf)
            {
                return "Story";
            }
            else
            {
                return null;
            }
        }
    }
}
