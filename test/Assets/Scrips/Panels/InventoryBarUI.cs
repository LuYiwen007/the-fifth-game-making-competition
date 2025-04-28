using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MC风格底部物品栏UI脚本，挂在物品栏Panel上
/// </summary>
public class InventoryBarUI : MonoBehaviour
{
    // 物品栏每个格子的Image（Inspector中拖入，顺序与UI一致）
    public List<Image> slotImages;
    // 每个格子的高光边框Image（Inspector中拖入，顺序与UI一致）
    public List<Image> highlightBorders;
    // 空格子的默认图标
    public Sprite emptySlotSprite;
    // 背包物品对应的图标（需和物品名一一对应，或用字典优化）
    public List<Sprite> itemSprites;
    // 当前选中物品的索引
    public int selectedIndex = 0;
    // 是否处于交互提示激活状态
    private bool interactionActive = false;

    void Update()
    {
        // 交互提示生效时禁用物品栏高光切换
        if (interactionActive) return;

        // 鼠标滚轮/左右键切换选中物品
        if (Input.GetAxis("Mouse ScrollWheel") > 0.01f || Input.GetKeyDown(KeyCode.RightArrow))
            selectedIndex = (selectedIndex + 1) % slotImages.Count;
        if (Input.GetAxis("Mouse ScrollWheel") < -0.01f || Input.GetKeyDown(KeyCode.LeftArrow))
            selectedIndex = (selectedIndex - 1 + slotImages.Count) % slotImages.Count;

        UpdateHighlight();
    }

    /// <summary>
    /// 设置交互提示激活状态（由InteractionItemUI调用）
    /// </summary>
    public void SetInteractionActive(bool active)
    {
        interactionActive = active;
        UpdateHighlight(); // 交互时可取消所有高光
    }

    /// <summary>
    /// 刷新物品栏显示（由背包系统调用，传入当前背包物品名列表）
    /// </summary>
    public void RefreshBar(List<string> itemNames)
    {
        for (int i = 0; i < slotImages.Count; i++)
        {
            if (i < itemNames.Count)
            {
                slotImages[i].sprite = GetSpriteByName(itemNames[i]);
                slotImages[i].color = Color.white;
            }
            else
            {
                slotImages[i].sprite = emptySlotSprite;
                slotImages[i].color = new Color(1,1,1,0.3f);
            }
        }
        UpdateHighlight();
    }

    /// <summary>
    /// 更新高亮边框显示
    /// </summary>
    void UpdateHighlight()
    {
        for (int i = 0; i < highlightBorders.Count; i++)
        {
            // 交互提示激活时全部不高亮，否则只高亮当前选中
            highlightBorders[i].enabled = (!interactionActive && i == selectedIndex);
        }
    }

    /// <summary>
    /// 根据物品名获取对应的Sprite
    /// </summary>
    Sprite GetSpriteByName(string itemName)
    {
        foreach (var sprite in itemSprites)
        {
            if (sprite.name == itemName)
                return sprite;
        }
        return emptySlotSprite;
    }
} 