using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GamePlayerLogic;

//�����ֻ�����ڿ�����Ϸʱ�������ָʾ�����������ڿ���UI�л�
//���鴴��Canvas������������GamePanel��
public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    //����������mask���Ǵ���һ�����������ڵ�ָʾ��
    //ͨ����Сmask�ĸ߶�����Сָʾֵ��ʾ����mask��ê����Ҫ������Ѫ���ײ�
    //��������ָ���ܲ��ܺ�mask����һ���ƶ�
    public Image blackpaintmask;
    public Image whitepaintmask;

    public float originalHeight;//ָʾ��ԭʼ�߶�
    public GameObject wjiasubiaoshi;//��ɫ�仯���ٱ�ʶ
    public GameObject bjiasubiaoshi;//��ɫ�仯���ٱ�ʶ
    public GamePlayerLogic player;

    void Awake()
    {
        Instance = this;
        originalHeight = blackpaintmask.rectTransform.rect.height;
        wjiasubiaoshi.SetActive(false);
    }

    private void Update()
    {
        Showjiasubiaoshi();//�����Ƿ���ʾ���ٱ�ʶ
    }

    //���úڰ�����ָʾ��ui
    public void WhitePaintValueUI(float Percentofwhitepaintvalue)
    {
        whitepaintmask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Percentofwhitepaintvalue * originalHeight);
    }
    public void BlackPaintValueUI(float Percentofblackpaintvalue)
    {
        blackpaintmask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Percentofblackpaintvalue * originalHeight);
    }
    
    public void Showjiasubiaoshi()
    {
        if (Inventory.Instance.HasItem("BlackPaintBottle") && player.GetCurrentAreaType() == AreaType.Black) 
        {
            bjiasubiaoshi.SetActive(true);
        }
        else if(Inventory.Instance.HasItem("WhitePaintBottle") && player.GetCurrentAreaType() == AreaType.White)
        {
            wjiasubiaoshi.SetActive(true);
        }
        else
        {
            wjiasubiaoshi.SetActive(false);
        }
    }

    public void OnPauseButtonClick()
    {
         
    }
}
