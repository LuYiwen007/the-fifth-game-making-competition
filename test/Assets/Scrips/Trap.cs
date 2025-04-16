using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour//����ű�����Ҫ��ǰ��Ԥ����ŵ���������(bool)ActivateTrapLogic����
{
    public static Trap Instance { get; private set; }
    public GameObject trap; //��ȡ����Ԥ����
    public float trapDuration = 5f; //����ʱ��
    public float trapCooldown = 5f;//��ȴʱ��
    public bool ActivateTrapLogic = false;//�Ƿ�������

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