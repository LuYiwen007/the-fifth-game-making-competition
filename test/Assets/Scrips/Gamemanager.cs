using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentLevel { get; private set; } = 0; // ��ǰ�ؿ���Ĭ��Ϊ0����ʾ��Ϸ��ʼǰ��״̬

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
    }

    public void NextLevel()//������һ���ؿ�
    {
        CurrentLevel++;
        Debug.Log($"��ǰ�ؿ�: {CurrentLevel}");
        InitializeLevelLogic();
    }









    public void SetLevel(int level)//���õ�ǰ�ؿ�
    {
        CurrentLevel = level;
        Debug.Log($"��ǰ�ؿ�����Ϊ: {CurrentLevel}");
    }

    public bool IsThirdLevel()//�ж��Ƿ��ǵ�����
    {
        return CurrentLevel == 3;
    }
    public void InitializeLevelLogic()//��ʼ���ؿ��߼�
    {
        switch (CurrentLevel)
        {
            case 0:
                Debug.Log("��Ϸ��ʼ����ʼ���������߼�");
                SceneManager.LoadScene("MainMenu");
                // �����������Ϸ��ʼʱ�ĳ�ʼ���߼�
                break;
            case 1:
                Debug.Log("��ʼ����һ���߼�");
                SceneManager.LoadScene("Level1");
                // ��������ӵ�һ�صĳ�ʼ���߼�
                break;
            case 2:
                Debug.Log("��ʼ���ڶ����߼�");
                SceneManager.LoadScene("Level2");
                // ��������ӵڶ��صĳ�ʼ���߼�
                break;
            case 3:
                Debug.Log("��ʼ���������߼�");
                SceneManager.LoadScene("Level3"); // ���ص����س���
                break;
            default:
                Debug.Log("��Ϸ��ɣ��������˵�����������");//������������˵�����������
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }
}
