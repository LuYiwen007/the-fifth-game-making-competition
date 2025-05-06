using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayPaintBottle : MonoBehaviour
{
    private float maxPaintValue = 100f;
    private float greyPaintValue = 0f; // �����еĻ�ɫ����ƿ�е�����ֵ
    private float greyPaintDecreaseRate = 10f; // ÿ���������û�ɫ�������ĵ�����ֵ

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

    // ��ӻ�ȡ�����û�ɫ����ֵ�ķ���
    public void SetGreyPaintValue(float value)
    {
        greyPaintValue = Mathf.Clamp(value, 0, maxPaintValue);
    }
}
