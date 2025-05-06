using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Whiteinput : MonoBehaviour
{
    private TextMeshProUGUI inputText;
    public bool haswhitepaint = false; // 是否有白色颜料瓶

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GamePlayerLogic player = collision.GetComponent<GamePlayerLogic>();
            inputText.text = $"F放入白色颜料，当前有{Inventory.Instance.GetItemCount("WhitePaintBottle")}瓶";
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Inventory.Instance.RemoveItemFromInventory("WhitePaintBottle", 1))// 从玩家物品栏中移除一个白色颜料瓶
                {
                    inputText.text = "成功放入白色颜料瓶！";
                    haswhitepaint = true;
                }
                else
                {
                    inputText.text = "没有白色颜料瓶";
                    return;
                }
            }
            Invoke("Disabletext", 2f);
        }
    }
    private void Disabletext()
    {
        inputText = null;
    }
}
