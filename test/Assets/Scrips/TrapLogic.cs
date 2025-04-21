using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLogic : MonoBehaviour
{
    public GameObject trap; // 获取陷阱对象
    public float trapDuration; // 持续时间
    public float trapCooldown; // 冷却时间
    private Coroutine trapCycleCoroutine; // 用于存储陷阱循环协程的引用

    // 启动陷阱循环
    public void StartTrapCycle()
    {
        if (trapCycleCoroutine == null) // 防止重复启动
        {
            trapCycleCoroutine = StartCoroutine(TrapCycle());
        }
    }

    // 停止陷阱循环
    public void StopTrapCycle()
    {
        trap.SetActive(false); // 确保停止时陷阱处于关闭状态
        if (trapCycleCoroutine != null)
        {
            StopCoroutine(trapCycleCoroutine);
            trapCycleCoroutine = null;
        }
    }

    // 封装的关闭方法
    public void DeactivateTrap()
    {
        StopTrapCycle(); // 停止协程并关闭陷阱
    }

    // 陷阱循环协程
    private IEnumerator TrapCycle()
    {
        while (true)
        {
            trap.SetActive(true); // 激活陷阱
            yield return new WaitForSeconds(trapDuration); // 等待持续时间
            trap.SetActive(false); // 停用陷阱
            yield return new WaitForSeconds(trapCooldown); // 等待冷却时间
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 检测到玩家
        {
            GamePlayerLogic player = collision.GetComponent<GamePlayerLogic>(); // 获取玩家逻辑组件
            
        }
    }
}
