using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Xml.Serialization;

public class StoryPanel : MonoBehaviour
{
    public TextMeshProUGUI storyText; // ������ʾ�����ı�
    public Image backgroundImage; // �����л�����
    public List<Sprite> backgrounds; // ����ͼƬ�б�
    private int currentBackgroundIndex = 0; // ��ǰ��������

    private Dictionary<int, string[]> levelStories = new Dictionary<int, string[]>(); // �洢ÿ���ؿ��ľ���
    private int currentLevel = 1; // ��ǰ�ؿ�
    private int currentStoryIndex = 0; // ��ǰ��������

    private void Start()
    {
        // ��ʼ���ؿ�����
        levelStories.Add(1, new string[] {
            "��ϲ��ͨ����һ�أ�"
        });

        levelStories.Add(2, new string[] {
            "��ϲ��ͨ���ڶ��أ�"
        });

        // ��ʾ��һ�����
        ShowCurrentStory();

        // ��ʼ������ͼƬ
        UpdateBackground();
        SetBackgroundByIndex(currentLevel);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            OnStoryClick();
        }
    }

    private void ShowCurrentStory()
    {
        if (levelStories.ContainsKey(currentLevel) && currentStoryIndex < levelStories[currentLevel].Length)
        {
            // ��ʾ��ǰ�����ı�
            storyText.text = levelStories[currentLevel][currentStoryIndex];
        }
        else
        {
            // ���������������һ��
            EnterNextLevel();
        }
    }

    public void OnStoryClick()
    {
        // �������ʾ��һ�����
        currentStoryIndex++;
        
        ShowCurrentStory();
    }

    private void EnterNextLevel()
    {
        // ���þ�������
        currentStoryIndex = 0;

        // �л�����һ��
        currentLevel++;

        if (currentLevel <= 3)
        {
            GameManager.Instance.NextLevel();
        }
        else
        {
            UIController.Instance.SetGameState(UIController.GameState.Win);
        }
    }

    private void UpdateBackground()
    {
        if (backgrounds.Count > 0 && currentBackgroundIndex < backgrounds.Count)
        {
            backgroundImage.sprite = backgrounds[currentBackgroundIndex];
        }
        else
        {
            Debug.LogWarning("����ͼƬ�б�Ϊ�ջ�����������Χ��");
        }
    }

    public void NextBackground()
    {
        // �л�����һ�ű���ͼƬ
        currentBackgroundIndex = (currentBackgroundIndex + 1) % backgrounds.Count;
        UpdateBackground();
    }

    public void SetBackgroundByIndex(int index)
    {
        // ���������л�����ͼƬ
        if (index >= 0 && index < backgrounds.Count)
        {
            currentBackgroundIndex = index;
            UpdateBackground();
        }
        else
        {
            Debug.LogWarning("��������������Χ��");
        }
    }
}
