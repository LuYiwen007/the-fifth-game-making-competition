using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    private Dictionary<string, int> inventory = new Dictionary<string, int>(); // �����ֵ䣬��Ϊ��Ʒ���ƣ�ֵΪ����
    public UnityEvent<string, int> OnInventoryChanged; // ��������Ʒ���ƣ���ǰ����
    private int maxInventorySize = 20; // �����������

    private void Awake()//��ʼ��Instance
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // ����ϵͳ��ط���
    public void InitializeInventory()//��ʼ����������
    {
        // ��ӳ�ʼ��Ʒ
        AddItemToInventory("BlackPaintBottle", 0);
        AddItemToInventory("WhitePaintBottle", 0);
        AddItemToInventory("GrayPaintBottle", 0);
    }

    public bool AddItemToInventory(string itemName, int amount = 1)//�����Ʒ������
    {
        // ��鱳���Ƿ�����
        if (GetInventoryItemCount() >= maxInventorySize)
        {
            //Debug.Log("�����������޷������Ʒ");
            return false;
        }

        // �����Ʒ�Ѵ��ڣ���������
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            // �����������Ʒ
            inventory[itemName] = amount;
        }

        // ���������仯�¼�
        OnInventoryChanged?.Invoke(itemName, inventory[itemName]);
        return true;
    }

    public bool RemoveItemFromInventory(string itemName, int amount = 1)//�ӱ����Ƴ���Ʒ
    {
        // �����Ʒ�Ƿ����
        if (!inventory.ContainsKey(itemName))
        {
            //Debug.Log($"������û��{itemName}");
            return false;
        }

        // �����Ʒ�����Ƿ��㹻
        if (inventory[itemName] < amount)
        {
            //Debug.Log($"������{itemName}��������");
            return false;
        }

        // ������Ʒ����
        inventory[itemName] -= amount;

        // �����Ʒ����Ϊ0���ӱ������Ƴ�
        if (inventory[itemName] <= 0)
        {
            inventory.Remove(itemName);
        }

        // ���������仯�¼�
        OnInventoryChanged?.Invoke(itemName, inventory.ContainsKey(itemName) ? inventory[itemName] : 0);
        return true;
    }

    public int GetItemCount(string itemName)//��ȡ��Ʒ����
    {
        if (inventory.ContainsKey(itemName))
        {
            return inventory[itemName];
        }
        return 0;
    }

    public bool HasItem(string itemName, int amount = 1)//����Ƿ����㹻��������Ʒ
    {
        return GetItemCount(itemName) >= amount;
    }

    public int GetInventoryItemCount()//��ȡ��������Ʒ����
    {
        int totalCount = 0;
        foreach (var item in inventory)
        {
            totalCount += item.Value;
        }
        return totalCount;
    }

    public int GetMaxInventorySize()//��ȡ�����������
    {
        return maxInventorySize;
    }

    public void SetMaxInventorySize(int size)//���ñ����������
    {
        maxInventorySize = size;
    }

    public Dictionary<string, int> GetInventory()//��ȡ��������
    {
        return new Dictionary<string, int>(inventory);
    }




}
