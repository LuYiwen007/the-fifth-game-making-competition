using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
            "��ӭ������һ�أ�",
            "������һ��������ս�����硣",
            "׼����ӭ��ð������"
        });

        levelStories.Add(2, new string[] {
            "�ڶ��ؿ�ʼ�ˣ�",
            "���˱�ø�ǿ�ˡ�",
            "С��ǰ�������壡"
        });

        // ��ʾ��һ�����
        ShowCurrentStory();
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

        // �л�����������б�����
        if (backgrounds.Count > 0)
        {
            currentBackgroundIndex = (currentBackgroundIndex + 1) % backgrounds.Count;
            backgroundImage.sprite = backgrounds[currentBackgroundIndex];
        }

        ShowCurrentStory();
    }

    private void EnterNextLevel()
    {
        // ���þ�������
        currentStoryIndex = 0;

        // �л�����һ��
        currentLevel++;

        if (levelStories.ContainsKey(currentLevel))
        {
            // ��ʾ��һ�ؾ���
            ShowCurrentStory();
        }
        else
        {
            // ���û�и���ؿ������ؾ������
            Debug.Log("���йؿ������Ѳ�����ϣ�");
            gameObject.SetActive(false);

            // ���� GameManager ������һ��
            GameManager.Instance.NextLevel();
        }
    }
}
