using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayPaintBottle : MonoBehaviour
{
    private float maxPaintValue = 100f;
    private float greyPaintValue = 0f; // 消耗中的灰色颜料瓶中的颜料值
    private float greyPaintDecreaseRate = 10f; // 每次生成永久灰色区域消耗的颜料值

    public void InitializeGreyPaint()
    {
        greyPaintValue = maxPaintValue;
    }
    public float GetPaintAmount()
    {
        return greyPaintValue;
    }
    public void ConsumePaint()
    {
        SetGreyPaintValue(greyPaintValue - greyPaintDecreaseRate);
    }

    // 添加获取和设置灰色颜料值的方法
    public void SetGreyPaintValue(float value)
    {
        greyPaintValue = Mathf.Clamp(value, 0, maxPaintValue);
    }
}
