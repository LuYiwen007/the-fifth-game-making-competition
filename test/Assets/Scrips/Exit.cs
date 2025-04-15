using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Exit : MonoBehaviour
{
    GamePlayerLogic gamePlayerLogic;

    //检测玩家触碰到出口、是否有三把钥匙
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))//玩家触碰出口检测
        {
            gamePlayerLogic = collision.gameObject.GetComponent<GamePlayerLogic>();
            if (gamePlayerLogic.HasItem("key", 3))//判断玩家是否集齐3把钥匙
            {
                gamePlayerLogic.RemoveItemFromInventory("key", 3);
                Animator animator = gameObject.GetComponent<Animator>();
                animator.SetTrigger("Exit");// 播放动画
                GameManager.Instance.NextLevel();//加载下一个关卡
            }
            else
            {
                Debug.Log("钥匙不足，无法离开，当前："+gamePlayerLogic.GetItemCount("key"));
            }
        }
    }

    //动画结束后，加载下一个场景
    public void LoadNextScene()
    {
        GameManager.Instance.NextLevel();//加载下一个关卡
    }
}
