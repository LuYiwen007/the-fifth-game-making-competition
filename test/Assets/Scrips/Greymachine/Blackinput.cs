using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackinput : MonoBehaviour
{
    public bool hasblackpaint = false; // 是否有黑色颜料瓶
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GamePlayerLogic player = collision.GetComponent<GamePlayerLogic>();
            //展示ui，询问玩家是否加入黑色颜料（按F确定）（未添加）
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Inventory.Instance.RemoveItemFromInventory("BlackPaintBottle", 1))// 从玩家物品栏中移除一个黑色颜料瓶
                {
                    Debug.Log("成功使用黑色颜料瓶");
                    //展示ui:成功放入黑色颜料瓶！提示背包中黑色颜料瓶减一（未添加）
                    hasblackpaint = true;
                }
                else
                {
                    Debug.Log("没有黑色颜料瓶");
                    //展示ui:黑色颜料瓶不足（未添加）
                    return;
                }
            }
        } 
    } 
}
    
