using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemSO> items = new List<ItemSO>();
    public List<ItemSO> itemsEquipped = new List<ItemSO>();
    public int maxSlots = 12;
    public int maxSlotsEquipped = 4;
    public bool isFull;
    public bool isFullEquipment;
    public bool isAdded;
    public bool isAddedEquipment;

    public void AddItemList(ItemSO item)
    {
        if (items.Count >= maxSlots - 1)
        {
            isFull = true;
            isAdded = false;

            // Popup

            Debug.Log($"<b> No more space </b>");
        }

        else
        {
            items.Add(item);
            isAdded = true;

            isFull = false;

        }
    }

    public void AddItemEquippedList(ItemSO item)
    {
        if (itemsEquipped.Count >= maxSlotsEquipped - 1)
        {
            isFullEquipment = true;

            // Popup

            Debug.Log($"<b> No more space </b>");
        }

        else
        {

            itemsEquipped.Add(item);
            isAddedEquipment = true;

            isFullEquipment = false;

        }
    }

    public void RemoveItemList(ItemSO item)
    {
        items.Remove(item);

    }
    public void RemoveItemEquippedList(ItemSO item)
    {
        itemsEquipped.Remove(item);

    }

}