using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
    public List<ActionButton> actionButtons;

    public void Init(List<ItemSO> items)
    {
        if (items.Count != 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                actionButtons[i].Init(items[i]);
                actionButtons[i].gameObject.SetActive(true);
            }
        }
        else
        {
            actionButtons[0].gameObject.SetActive(true);
        }
    }
}