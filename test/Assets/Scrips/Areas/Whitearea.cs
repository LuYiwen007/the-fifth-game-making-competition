using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whitearea : MonoBehaviour
{
    private AreaTypeComponent areaTypeComponent;

    private void Awake()
    {
        // 获取 AreaTypeComponent 组件
        areaTypeComponent = GetComponent<AreaTypeComponent>();

        // 确保组件存在
        if (areaTypeComponent != null)
        {
            // 设置为白色区域
            areaTypeComponent.areaType = GamePlayerLogic.AreaType.White;
        }
        else
        {
            Debug.LogError("AreaTypeComponent 未挂载到该 GameObject 上！");
        }
    }
}

