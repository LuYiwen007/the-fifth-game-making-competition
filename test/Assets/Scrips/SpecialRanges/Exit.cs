using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Exit : MonoBehaviour
{
    GamePlayerLogic gamePlayerLogic;
    public TrapLogic trapLogic; // 绑定场景中的 TrapLogic 脚本

    //检测玩家触碰到出口、是否有三把钥匙
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))//玩家触碰出口检测
        {
            gamePlayerLogic = collision.gameObject.GetComponent<GamePlayerLogic>();
            if (Inventory.Instance.HasItem("key", 3))//判断玩家是否集齐3把钥匙
            {
                    trapLogic.DeactivateTrap(); // 禁用陷阱逻辑
                    Inventory.Instance.RemoveItemFromInventory("key", 3);
                    //Animator animator = gameObject.GetComponent<Animator>();
                    //animator.SetTrigger("Exit");// 播放动画
                    UIController.Instance.SetGameState(UIController.GameState.Story);
                    GameManager.Instance.hasrespwam = false;
            }
            else
            {
                Debug.Log("钥匙不足，无法离开，当前：" + Inventory.Instance.GetItemCount("key"));
            }
        }
    }
}
