using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
            "欢迎来到第一关！",
            "这里是一个充满挑战的世界。",
            "准备好迎接冒险了吗？"
        });

        levelStories.Add(2, new string[] {
            "第二关开始了！",
            "敌人变得更强了。",
            "小心前方的陷阱！"
        });

        // 显示第一句剧情
        ShowCurrentStory();
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

        // 切换背景（如果有背景）
        if (backgrounds.Count > 0)
        {
            currentBackgroundIndex = (currentBackgroundIndex + 1) % backgrounds.Count;
            backgroundImage.sprite = backgrounds[currentBackgroundIndex];
        }

        ShowCurrentStory();
    }

    private void EnterNextLevel()
    {
        // 重置剧情索引
        currentStoryIndex = 0;

        // 切换到下一关
        currentLevel++;

        if (levelStories.ContainsKey(currentLevel))
        {
            // 显示下一关剧情
            ShowCurrentStory();
        }
        else
        {
            // 如果没有更多关卡，隐藏剧情面板
            Debug.Log("所有关卡剧情已播放完毕！");
            gameObject.SetActive(false);

            // 调用 GameManager 进入下一关
            GameManager.Instance.NextLevel();
        }
    }
}
