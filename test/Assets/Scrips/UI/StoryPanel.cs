using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Xml.Serialization;

public class StoryPanel : MonoBehaviour
{
    public TextMeshProUGUI storyText; // 用于显示剧情文本
    public Image backgroundImage; // 用于切换背景
    public List<Sprite> backgrounds; // 背景图片列表
    private int currentBackgroundIndex = 0; // 当前背景索引

    private Dictionary<int, string[]> levelStories = new Dictionary<int, string[]>(); // 存储每个关卡的剧情
    private int currentLevel = 1; // 当前关卡
    private int currentStoryIndex = 0; // 当前剧情索引

    private void Start()
    {
        // 初始化关卡剧情
        levelStories.Add(1, new string[] {
            "恭喜你通过第一关！"
        });

        levelStories.Add(2, new string[] {
            "恭喜你通过第二关！"
        });

        // 显示第一句剧情
        ShowCurrentStory();

        // 初始化背景图片
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
            // 显示当前剧情文本
            storyText.text = levelStories[currentLevel][currentStoryIndex];
        }
        else
        {
            // 剧情结束，进入下一关
            EnterNextLevel();
        }
    }

    public void OnStoryClick()
    {
        // 点击后显示下一句剧情
        currentStoryIndex++;
        
        ShowCurrentStory();
    }

    private void EnterNextLevel()
    {
        // 重置剧情索引
        currentStoryIndex = 0;

        // 切换到下一关
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
            Debug.LogWarning("背景图片列表为空或索引超出范围！");
        }
    }

    public void NextBackground()
    {
        // 切换到下一张背景图片
        currentBackgroundIndex = (currentBackgroundIndex + 1) % backgrounds.Count;
        UpdateBackground();
    }

    public void SetBackgroundByIndex(int index)
    {
        // 根据索引切换背景图片
        if (index >= 0 && index < backgrounds.Count)
        {
            currentBackgroundIndex = index;
            UpdateBackground();
        }
        else
        {
            Debug.LogWarning("背景索引超出范围！");
        }
    }
}
