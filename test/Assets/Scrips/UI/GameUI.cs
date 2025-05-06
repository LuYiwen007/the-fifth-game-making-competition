using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//�����ֻ�����ڿ�����Ϸʱ�������ָʾ�����������ڿ���UI�л�
//���鴴��Canvas������������GamePanel��
public class GameUI : MonoBehaviour
{

    public static GameUI Instance; 
    public GamePlayerLogic player;

    //���ٱ�ʶ
    public GameObject wjiasubiaoshi;//��ɫ�仯���ٱ�ʶ
    public GameObject bjiasubiaoshi;//��ɫ�仯���ٱ�ʶ

    //����ֵ
    private float originalWidth;//ָʾ��ԭʼ���
    public Image blackpaintmask;
    public Image whitepaintmask; 

    //��Ʒ����Ʒ����
    public TextMeshProUGUI WhiteBottle;
    public TextMeshProUGUI BlackBottle;
    public TextMeshProUGUI GeryBottle;
    public TextMeshProUGUI Key;

    //����ֵ
    private float originalhpwidth;//Ѫ��ԭʼ���
    public Image HPMask;

    public void Awake()
    {
        originalWidth = blackpaintmask.rectTransform.rect.height;
        originalhpwidth = HPMask.rectTransform.rect.width;
    }

    private void Update()
    {
        Showjiasubiaoshi();//�����Ƿ���ʾ���ٱ�ʶ
    }
    public void FixedUpdate()
    {
        ////������Ʒ����Ʒ����
        //if (Inventory.Instance != null)
        //{
        //    int whiteBottleCount = Inventory.Instance.GetItemCount("WhitePaintBottle");
        //    int blackBottleCount = Inventory.Instance.GetItemCount("BlackPaintBottle");
        //    int geryBottleCount = Inventory.Instance.GetItemCount("GrayPaintBottle");
        //    int keyCount = Inventory.Instance.GetItemCount("Key");
        //    WhiteBottle.text = whiteBottleCount.ToString();
        //    BlackBottle.text = blackBottleCount.ToString();
        //    GeryBottle.text = geryBottleCount.ToString();
        //    Key.text = keyCount.ToString();
        //}
    }

    //���úڰ�����ָʾ��ui
    public void WhitePaintValueUI()
    {
        whitepaintmask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, player.GetWhitePaintValue()/ player.GetMaxPaintValue() * originalWidth);
    }
    public void BlackPaintValueUI()
    {
        blackpaintmask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, player.GetBlackPaintValue()/ player.GetMaxPaintValue() * originalWidth);
    }
    
    //����Ѫ��UI
    public void HPUI()
    {
        float percent = (float)player.currenthp / player.maxhp;
        HPMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percent * originalhpwidth);
    }

    //���ٱ�ʶ������
    public void Showjiasubiaoshi()
    {
        if (Inventory.Instance.HasItem("BlackPaintBottle") && player.GetCurrentAreaType() == GamePlayerLogic.AreaType.Black) 
        {
            bjiasubiaoshi.SetActive(true);
            wjiasubiaoshi.SetActive(false);
        }
        else if(Inventory.Instance.HasItem("WhitePaintBottle") && player.GetCurrentAreaType() == GamePlayerLogic.AreaType.White)
        {
            bjiasubiaoshi.SetActive(false);
            wjiasubiaoshi.SetActive(true);
        }
        else if (Inventory.Instance.HasItem("WhitePaintBottle")&& Inventory.Instance.HasItem("BlackPaintBottle")&& player.GetCurrentAreaType() == GamePlayerLogic.AreaType.Black)
        {
            bjiasubiaoshi.SetActive(true);
            wjiasubiaoshi.SetActive(false);
        }
        else if (Inventory.Instance.HasItem("WhitePaintBottle") && Inventory.Instance.HasItem("BlackPaintBottle") && player.GetCurrentAreaType() == GamePlayerLogic.AreaType.White)
        {
            bjiasubiaoshi.SetActive(false);
            wjiasubiaoshi.SetActive(true);
        }
        else
        {
            wjiasubiaoshi.SetActive(false);
            bjiasubiaoshi.SetActive(false);
        }
    }

    public void OnPauseButtonClick()
    {
        UIController.Instance.SetGameState(UIController.GameState.Pause);
    }
}

