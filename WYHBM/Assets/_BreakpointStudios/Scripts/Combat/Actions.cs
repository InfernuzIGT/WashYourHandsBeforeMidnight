using UnityEngine;

public class Actions : MonoBehaviour
{
    public ActionButton actionButtonA;
    public ActionButton actionButtonB;
    public ActionButton actionButtonItem;

    private int _lastIndex;

    public void Init(Equipment equipment)
    {
        actionButtonA.Init(equipment.actionA, UpdateIndex);
        actionButtonB.Init(equipment.actionB, UpdateIndex);
        actionButtonItem.Init(equipment.actionItem[0], UpdateIndex);

        _lastIndex = 0;
    }

    private void UpdateIndex(int index)
    {
        _lastIndex = index;
    }

    public void SelectButton()
    {
        switch (_lastIndex)
        {
            case 0:
                actionButtonA.SelectButton();
                break;

            case 1:
                actionButtonB.SelectButton();
                break;

            case 2:
                actionButtonItem.SelectButton();
                break;
        }
    }

}