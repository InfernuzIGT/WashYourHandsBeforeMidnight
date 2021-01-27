using Events;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AddListenerMenu : MonoBehaviour
{
    public UI_TYPE menuType;

    private MainMenuEvent _mainMenuEvent;

    private void Start()
    {
        _mainMenuEvent = new MainMenuEvent();
        _mainMenuEvent.menuType = menuType;

        Button optionButton = GetComponent<Button>();
        optionButton.onClick.AddListener(() => GoToMenu());
    }

    private void GoToMenu()
    {
        EventController.TriggerEvent(_mainMenuEvent);
    }

}