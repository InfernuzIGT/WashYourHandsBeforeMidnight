using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AddListenerPause : MonoBehaviour
{
    public BUTTON_TYPE buttonType;

    private void Start()
    {
        Button optionButton = GetComponent<Button>();
        optionButton.onClick.AddListener(() => GameManager.Instance.worldUI.MenuPause(buttonType));
    }
}