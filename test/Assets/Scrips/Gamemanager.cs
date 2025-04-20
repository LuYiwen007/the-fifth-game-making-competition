using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int CurrentLevel { get; private set; } = 1; // ��ǰ�ؿ���Ĭ��Ϊ0����ʾ��Ϸ��ʼǰ��״̬
    public TrapLogic trapLogic; // �󶨳����е� TrapLogic �ű�
    public GamePlayerLogic playerLogic;
    public UnityEvent OnDeath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ��֤ GameManager �ڳ����л�ʱ��������
        }
        else
        {
            Destroy(gameObject);
        }
        InitializeLevelLogic();
    }
    public void NextLevel()//������һ���ؿ�
    {
        CurrentLevel++;
        Debug.Log($"��ǰ�ؿ�: {CurrentLevel}");
        InitializeLevelLogic();
    }
    private void InitializeLevelLogic()//��ʼ���ؿ��߼�
    {   
        //��ʼ�����
        playerLogic.InitializePlayer();
        
        //��������ֵ
        playerLogic.SetBlackPaintValue(0);
        playerLogic.SetWhitePaintValue(0);

        //���������߼�
        trapLogic.StartTrapCycle();

        //��ʼ������
        Inventory.Instance.InitializeInventory();

        switch (CurrentLevel)
        {
            case 0:
                Debug.Log("��Ϸ��ʼ����ʼ���������߼�");
                //SceneManager.LoadScene("MainMenu");
                // �����������Ϸ��ʼʱ�ĳ�ʼ���߼�
                break;
            case 1:
                Debug.Log("��ʼ����һ���߼�");
                //SceneManager.LoadScene("Level1");
                // ��������ӵ�һ�صĳ�ʼ���߼�
                break;
            case 2:
                Debug.Log("��ʼ���ڶ����߼�");
                //SceneManager.LoadScene("Level2");
                // ��������ӵڶ��صĳ�ʼ���߼�
                break;
            case 3:
                Debug.Log("��ʼ���������߼�");
                //SceneManager.LoadScene("Level3"); 

                break;
            default:
                Debug.Log("��Ϸ��ɣ��������˵�����������");//������������˵�����������
                //SceneManager.LoadScene("MainMenu");
                break;
        }
    }

    // ��������
    public void Die()
    {
        Debug.Log("��ɫ����");
        // ���������¼�
        OnDeath?.Invoke();

        // ���ÿ�����
        enabled = false;

        //չʾ����ui
    }
    public void RestartLevel()//���¿�ʼ��ǰ�ؿ�
    {
        Debug.Log($"���¿�ʼ��ǰ�ؿ�: {CurrentLevel}");
        InitializeLevelLogic();
    }
}
