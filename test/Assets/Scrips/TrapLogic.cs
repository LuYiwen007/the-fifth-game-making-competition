using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLogic : MonoBehaviour
{
    public GameObject trap; // ��ȡ�������
    public float trapDuration; // ����ʱ��
    public float trapCooldown; // ��ȴʱ��
    private Coroutine trapCycleCoroutine; // ���ڴ洢����ѭ��Э�̵�����

    // ��������ѭ��
    public void StartTrapCycle()
    {
        if (trapCycleCoroutine == null) // ��ֹ�ظ�����
        {
            trapCycleCoroutine = StartCoroutine(TrapCycle());
        }
    }

    // ֹͣ����ѭ��
    public void StopTrapCycle()
    {
        trap.SetActive(false); // ȷ��ֹͣʱ���崦�ڹر�״̬
        if (trapCycleCoroutine != null)
        {
            StopCoroutine(trapCycleCoroutine);
            trapCycleCoroutine = null;
        }
    }

    // ��װ�Ĺرշ���
    public void DeactivateTrap()
    {
        StopTrapCycle(); // ֹͣЭ�̲��ر�����
    }

    // ����ѭ��Э��
    private IEnumerator TrapCycle()
    {
        while (true)
        {
            trap.SetActive(true); // ��������
            yield return new WaitForSeconds(trapDuration); // �ȴ�����ʱ��
            trap.SetActive(false); // ͣ������
            yield return new WaitForSeconds(trapCooldown); // �ȴ���ȴʱ��
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ��⵽���
        {
            GamePlayerLogic player = collision.GetComponent<GamePlayerLogic>(); // ��ȡ����߼����
            
        }
    }
}
