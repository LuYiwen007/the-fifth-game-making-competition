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
        // 初始化关卡剧情
        levelStories.Add(1, new string[] {
            "恭喜你通过第一关！",
            "恭喜你通过第一关！",
            "恭喜你通过第一关！",
            "恭喜你通过第一关！"
        });

        levelStories.Add(2, new string[] {
            "恭喜你通过第二关！",
            "恭喜你通过第二关！",
            "恭喜你通过第二关！",
            "恭喜你通过第二关！",
        });

        levelStories.Add(3, new string[] {
            "恭喜你通过第三关！",
            "你已经完成了所有挑战！"
        });
    }

    private void OnEnable()
    {
        currentLevel = GameManager.Instance.CurrentLevel; // 正确赋值
        currentStoryIndex = 0; // 每次激活时重置剧情索引
        ShowCurrentStory();
        SetBackgroundByIndex(currentLevel - 1); // 关卡1对应索引0
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
        // 如果是最后一关，直接胜利
        if (GameManager.Instance.CurrentLevel >= 3)
        {
            UIController.Instance.SetGameState(UIController.GameState.Win);
        }
        else
        {
            // 进入下一关
            GameManager.Instance.NextLevel();
        }
    }

    // 设置背景图片
    public void SetBackgroundByIndex(int index)
    {
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
}
