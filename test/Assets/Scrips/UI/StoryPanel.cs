using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Xml.Serialization;

public class StoryPanel : MonoBehaviour
{
    public TextMeshProUGUI storyText;
    public Image backgroundImage;
    public List<Sprite> backgrounds;
    private int currentBackgroundIndex = 0;
    private int currentLevel;
    private Dictionary<int, string[]> levelStories = new Dictionary<int, string[]>();
    private int currentStoryIndex = 0;

    private void Start()
    {
        // ��ʼ���ؿ�����
        levelStories.Add(1, new string[] {
            "��ϲ��ͨ����һ�أ�",
            "��ϲ��ͨ����һ�أ�",
            "��ϲ��ͨ����һ�أ�",
            "��ϲ��ͨ����һ�أ�"
        });

        levelStories.Add(2, new string[] {
            "��ϲ��ͨ���ڶ��أ�",
            "��ϲ��ͨ���ڶ��أ�",
            "��ϲ��ͨ���ڶ��أ�",
            "��ϲ��ͨ���ڶ��أ�",
        });

        levelStories.Add(3, new string[] {
            "��ϲ��ͨ�������أ�",
            "���Ѿ������������ս��"
        });
    }

    private void OnEnable()
    {
        currentLevel = GameManager.Instance.CurrentLevel; // ��ȷ��ֵ
        currentStoryIndex = 0; // ÿ�μ���ʱ���þ�������
        ShowCurrentStory();
        SetBackgroundByIndex(currentLevel - 1); // �ؿ�1��Ӧ����0
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            OnStoryClick();
        }
    }

    public void OnStoryClick()
    {
        currentStoryIndex++;
        ShowCurrentStory();
    }

    private void ShowCurrentStory()
    {
        if (/*levelStories.ContainsKey(currentLevel) &&*/ currentStoryIndex < levelStories[currentLevel].Length)
        {
            storyText.text = levelStories[currentLevel][currentStoryIndex];
        }
        else
        {
            EnterNextLevel();
        }
    }

    private void EnterNextLevel()
    {
        // ��������һ�أ�ֱ��ʤ��
        if (GameManager.Instance.CurrentLevel >= 3)
        {
            UIController.Instance.SetGameState(UIController.GameState.Win);
        }
        else
        {
            // ������һ��
            GameManager.Instance.NextLevel();
        }
    }

    // ���ñ���ͼƬ
    public void SetBackgroundByIndex(int index)
    {
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
}
