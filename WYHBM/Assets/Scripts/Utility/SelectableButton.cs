using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableButton : MonoBehaviour
{
    public EventSystem eventSystem;

    public void Select()
    {
        Button button = GetComponent<Button>();
        eventSystem.SetSelectedGameObject(button.gameObject);
    }
}