using System.Collections.Generic;
using Events;
using UnityEngine;

public class InteractionScene : Interaction, IInteractable
{
    [Header("Cutscene")]
    [SerializeField] private bool _load = false;
    [SerializeField] private SceneSO sceneData = null;
    public Vector3 newPlayerPosition;
    [SerializeField] private bool _instantFade = false;
    [Space]
    [SerializeField] private bool _showDebug = false;
    [SerializeField] private Color _color = new Color(0, 1, 0, 0.5f);

    private List<AsyncOperation> _listScenes;
    private ChangeSceneEvent _changeSceneEvent;

    public bool ShowDebug { get { return _showDebug; } }

    private void Start()
    {
        _listScenes = new List<AsyncOperation>();

        _changeSceneEvent = new ChangeSceneEvent();
        _changeSceneEvent.load = _load;
        _changeSceneEvent.newPlayerPosition = newPlayerPosition;
        _changeSceneEvent.sceneData = sceneData;
        _changeSceneEvent.instantFade = _instantFade;
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<InteractionEvent>(OnInteractScene);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractScene);
        }
    }

    private void OnInteractScene(InteractionEvent evt)
    {
        EventController.RemoveListener<InteractionEvent>(OnInteractScene);
        
        ShowHint(false);

        EventController.TriggerEvent(_changeSceneEvent);
    }

    public void ResetPosition()
    {
        newPlayerPosition = gameObject.transform.position;
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (_showDebug)
        {
            Gizmos.color = _color;
            Gizmos.DrawCube(newPlayerPosition, new Vector3(1, 3.15f, 1));
        }
    }

#endif
}