using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackinput : MonoBehaviour
{
    public bool hasblackpaint = false; // 是否有黑色颜料瓶
    public TextMeshProUGUI inputText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GamePlayerLogic player = collision.GetComponent<GamePlayerLogic>();
            inputText.text = $"F放入黑色颜料，当前有{Inventory.Instance.GetItemCount("BlackPaintBottle")}瓶";
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Inventory.Instance.RemoveItemFromInventory("BlackPaintBottle", 1))// 从玩家物品栏中移除一个黑色颜料瓶
                {
                    inputText.text = "成功放入黑色颜料瓶";
                    hasblackpaint = true;
                }
                else
                {
                    inputText.text = "没有黑色颜料瓶";
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
    
