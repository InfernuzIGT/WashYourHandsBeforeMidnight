using UnityEngine;

public class Actions : MonoBehaviour
{
    public ActionButton actionButtonA;
    public ActionButton actionButtonB;
    public ActionButton actionButtonItem;

    public void Init(Equipment equipment)
    {
        actionButtonA.Init(equipment.actionA);
        actionButtonB.Init(equipment.actionB);
        actionButtonItem.Init(equipment.actionItem[0]); // TODO Mariano: Review
    }

    public void SelectFirstButton()
    {
        actionButtonA.SelectFirstButton();
    }

}