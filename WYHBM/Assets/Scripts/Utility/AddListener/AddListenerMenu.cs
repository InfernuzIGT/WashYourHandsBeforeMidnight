using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AddListenerMenu : MonoBehaviour
{
    public MENU_TYPE menuType;

    private void Start()
    {
        Button optionButton = GetComponent<Button>();
        optionButton.onClick.AddListener(() => GameManager.Instance.menuController.MainMenu(menuType));
    }

}