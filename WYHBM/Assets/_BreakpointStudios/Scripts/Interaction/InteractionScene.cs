using System.Collections.Generic;
using Events;
using FMODUnity;
using UnityEngine;

public class InteractionScene : Interaction, IInteractable, IHoldeable
{
    [Header("Cutscene")]
    public Vector3 newPlayerPosition;
    [SerializeField] private bool _onlyTeleport = true;
    [SerializeField] private bool _load = false;
    [SerializeField] private bool _isLoadAdditive = true;
    [SerializeField] private bool _instantFade = false;

    [SerializeField] private SceneSO sceneData = null;
    [SerializeField] private InputHoldUtility _holdUtility = null;

    [Header("Debug")]
    [SerializeField] private bool _showDebug = false;
    [SerializeField] private Color _color = new Color(0, 1, 0, 0.5f);

    [Header("FMOD")]
    [SerializeField] private FMODAmbience _ambienceToPlay;
    [SerializeField] private FMODAmbience _ambienceToStop;
    [Space]
    [SerializeField] private StudioEventEmitter holdStart;
    [SerializeField] private StudioEventEmitter holdStop;
    [SerializeField] private StudioEventEmitter doorSound;

    private List<AsyncOperation> _listScenes;
    private ChangeSceneEvent _changeSceneEvent;

    public bool ShowDebug { get { return _showDebug; } }

    private void Start()
    {
        _listScenes = new List<AsyncOperation>();

        _changeSceneEvent = new ChangeSceneEvent();
        _changeSceneEvent.onlyTeleport = _onlyTeleport;
        _changeSceneEvent.load = _load;
        _changeSceneEvent.useEnableMovementEvent = true;
        _changeSceneEvent.isLoadAdditive = _isLoadAdditive;
        _changeSceneEvent.newPlayerPosition = newPlayerPosition;
        _changeSceneEvent.sceneData = sceneData;
        _changeSceneEvent.instantFade = _instantFade;
        _changeSceneEvent.callbackFMODPlay = PlayAmbience;
        _changeSceneEvent.callbackFMODStop = StopAmbience;

        _holdUtility.OnStarted.AddListener(OnStart);
        _holdUtility.OnCanceled.AddListener(OnCancel);
        _holdUtility.OnFinished.AddListener(OnFinish);
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
            _holdUtility.OnCancel();

            EventController.RemoveListener<InteractionEvent>(OnInteractScene);
        }
    }

    private void OnInteractScene(InteractionEvent evt)
    {
        if (evt.isStart)
        {
            _holdUtility.OnStart();

            if (_holdUtility.isDoor)
            {
                holdStart.Play();
            }
        }
        else
        {
            _holdUtility.OnCancel();

            if (_holdUtility.isDoor)
            {
                holdStart.Stop();
                holdStop.Play();
            }
        }
    }

    public void ResetPosition()
    {
        newPlayerPosition = gameObject.transform.position + new Vector3(1, 0, 1);
    }

    private void PlayAmbience()
    {
        if (_ambienceToPlay != null)_ambienceToPlay.Execute(true);
    }

    private void StopAmbience()
    {
        if (_ambienceToStop != null)_ambienceToStop.Execute(false);
    }

    #region Hold System

    public void OnStart()
    {
        ShowHint(false);
    }

    public void OnCancel()
    {
        ShowHint(true);
    }

    public void OnFinish()
    {
        EventController.RemoveListener<InteractionEvent>(OnInteractScene);

        EventController.TriggerEvent(_changeSceneEvent);

        doorSound.Play();

        ForceCleanInteraction();
    }

    #endregion

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