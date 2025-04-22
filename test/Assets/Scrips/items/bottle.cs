using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bottle : MonoBehaviour, IPickupable
{
    public enum BottleType
    {
        Black,
        White,
        Gray
    }
    public BottleType bottleType;
    private int amount = 1;
    public int GetItemAmount()
    {
        return amount;
    }
    public string GetItemName()
    {
        switch (bottleType)
        {
            case BottleType.Black:
                return "BlackPaintBottle";//��ɫ����ƿ
            case BottleType.White:
                return "WhitePaintBottle";//��ɫ����ƿ
            case BottleType.Gray:
                return "GrayPaintBottle";//��ɫ����ƿ
            default:
                return "UnknownBottle";//δ֪����ƿ
        }
    }
    public void OnPickup(GameObject player)
    {
        GamePlayerLogic playerLogic = player.GetComponent<GamePlayerLogic>();
        if (playerLogic != null)
        {
            if (Inventory.Instance.AddItemToInventory(GetItemName(), GetItemAmount()))
            {
                
                // ʰȡ�ɹ���������Ʒ
                Destroy(gameObject);
                Debug.Log(Inventory.Instance.GetInventory());
            }
            else
            {
                // ʰȡʧ�ܣ���Ʒ������
                Debug.Log("��Ʒ���������޷�ʰȡ " + GetItemName());
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Bottle bottle = GetComponent<Bottle>();
            if (bottle != null)
            {
                bottle.OnPickup(collision.gameObject); // ����ʰȡ�߼�
            }
        }
    }

}