using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemSO> items = new List<ItemSO>();
    public int maxSlots = 12;
    public bool isFull;
    public bool isAdded;

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
        }
    }

    public void RemoveItemList(ItemSO item)
    {
        items.Remove(item);
        
    }

}