using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    public void MouseEnter()
    {
        Debug.Log($"<b> ENTER </b>");
    }

    public void MouseExit()
    {
        Debug.Log($"<b> EXIT </b>");
    }

    public void MouseStay()
    {
        Debug.Log($"<b> STAY </b>");
    }
}