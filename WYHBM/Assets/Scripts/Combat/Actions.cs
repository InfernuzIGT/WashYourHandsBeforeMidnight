using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
    public List<ActionButton> actionButtons;

    public void Init(List<ItemSO> items)
    {
        int lastIndex = 0;

        if (items.Count != 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                actionButtons[i].Init(items[i]);
                actionButtons[i].gameObject.SetActive(true);
                lastIndex = i;
            }

            if (lastIndex < 2)
            {
                actionButtons[lastIndex + 1].gameObject.SetActive(true);
            }
        }
        else
        {
            actionButtons[0].gameObject.SetActive(true);
        }
    }
    
    public void SelectFirstButton()
    {
        actionButtons[0].SelectFirstButton();
    }

    // public void SetButtonsEnable(bool isEnable)
    // {
    //     for (int i = 0; i < actionButtons.Count; i++)
    //     {
    //         actionButtons[i].SetButtonEnable(isEnable);
    //     }
    // }
    
}