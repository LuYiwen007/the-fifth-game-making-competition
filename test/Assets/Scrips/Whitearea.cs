using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whitearea : MonoBehaviour
{
    private AreaTypeComponent areaTypeComponent;

    private void Awake()
    {
        // ��ȡ AreaTypeComponent ���
        areaTypeComponent = GetComponent<AreaTypeComponent>();

        // ȷ���������
        if (areaTypeComponent != null)
        {
            // ����Ϊ��ɫ����
            areaTypeComponent.areaType = GamePlayerLogic.AreaType.White;
        }
        else
        {
            Debug.LogError("AreaTypeComponent δ���ص��� GameObject �ϣ�");
        }
    }
}

