using Events;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventSystem))]
public class EventSystemUtility : MonoBehaviour
{
    private EventSystem _eventSystem;

    private void Awake()
    {
        _eventSystem = GetComponent<EventSystem>();
    }

    private void OnEnable()
    {
        EventController.AddListener<EventSystemEvent>(OnUpdateSelection);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<EventSystemEvent>(OnUpdateSelection);
    }

    private void OnUpdateSelection(EventSystemEvent evt)
    {
        _eventSystem.firstSelectedGameObject = evt.objectSelected;
        _eventSystem.SetSelectedGameObject(evt.objectSelected);
    }

}