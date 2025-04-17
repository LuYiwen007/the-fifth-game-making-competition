using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteinput : MonoBehaviour
{
    public bool haswhitepaint = false; // 是否有白色颜料瓶
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GamePlayerLogic player = collision.GetComponent<GamePlayerLogic>();
            //展示ui，询问玩家是否加入白色颜料（按F确定）（未添加）
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (player.RemoveItemFromInventory("WhitePaintBottle", 1))// 从玩家物品栏中移除一个白色颜料瓶
                {
                    Debug.Log("成功使用白色颜料瓶");
                    //展示ui:成功放入白色颜料瓶！提示背包中白色颜料瓶减一（未添加）
                    haswhitepaint = true;
                }
                else
                {
                    Debug.Log("没有白色颜料瓶");
                    //展示ui:白色颜料瓶不足（未添加）
                    return;
                }
            }
        }
    }
}
