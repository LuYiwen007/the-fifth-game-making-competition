using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour//陷阱脚本，需要提前把预制体放到场景，用(bool)ActivateTrapLogic开启
{
    public static Trap Instance { get; private set; }
    public GameObject trap; //获取陷阱预制体
    public float trapDuration = 5f; //持续时间
    public float trapCooldown = 5f;//冷却时间
    public bool ActivateTrapLogic = false;//是否开启陷阱

    public void TrapManager()
    {
        while (ActivateTrapLogic == true)
        {
            StartCoroutine(TrapCooldown());
            if(ActivateTrapLogic != true)
            {
                StopCoroutine(TrapCooldown());
                trap.SetActive(false);
            }
        }
    }
    IEnumerator TrapCooldown()
    {
        trap.SetActive(true);
        yield return new WaitForSeconds(trapDuration);
        trap.SetActive(false);
        yield return new WaitForSeconds(trapCooldown);
    }
}