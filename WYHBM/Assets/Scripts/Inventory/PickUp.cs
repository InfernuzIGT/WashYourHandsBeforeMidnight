using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Inventory inventory;
    public GameObject itemButton;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameData.Instance.gameConfig.tagPlayer))
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (!inventory.isFull[i])
                {
                    // ITEM CAN BE ADDED TO _INVENTORY
                    inventory.isFull[i] = true;
                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    // DropIt(item);
                    break;
                    
                }
            }
        }
    }
}
