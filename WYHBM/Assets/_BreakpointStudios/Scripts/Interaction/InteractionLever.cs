using System.Collections;
using Cinemachine;
using Events;
using FMODUnity;
using UnityEngine;

public class InteractionLever : Interaction
{
    [Header("Lever")]
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField, Range(0f, 5f)] private float _waitTime = 3;
    [SerializeField, EventRef] private string _sound = "";
    [SerializeField] private bool _useOnlyOnce = true;

    private EnableMovementEvent _enableMovementEvent;
    private CutsceneEvent _cutsceneEvent;

    private void Start()
    {
        _checkCurrentInteraction = false;

        _enableMovementEvent = new EnableMovementEvent();
        _enableMovementEvent.isDetected = false;

        _cutsceneEvent = new CutsceneEvent();

        if (_useOnlyOnce)CheckPersistence(string.Format(DDParameters.Format, DDParameters.SimpleDialog, gameObject.name));
    }

    public override void Used()
    {
        base.Used();

        ExecuteAnimation(true);
        SetCollider(false);
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            Execute(true);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            Execute(false);
        }
    }

    public override void Execute(bool enable)
    {
        base.Execute();

        CanInteractEvent(enable);
    }

    public override void OnInteractEvent()
    {
        base.OnInteractEvent();

        if (_sound != "")RuntimeManager.PlayOneShot(_sound);

        ExecuteAnimation();
        ExecuteCutscene();

        if (_useOnlyOnce && !_used)
        {
            _used = true;
            Execute(false);
            SetCollider(false);

            GameData.Instance.WriteID(_usedId);
        }
    }

    private void ExecuteCutscene()
    {
        if (_camera == null)return;

        StartCoroutine(Cutscene());
    }

    private IEnumerator Cutscene()
    {
        _cutsceneEvent.show = true;
        EventController.TriggerEvent(_cutsceneEvent);

        _camera.gameObject.SetActive(true);

        _enableMovementEvent.canMove = false;
        EventController.TriggerEvent(_enableMovementEvent);

        yield return new WaitForSeconds(_waitTime + 2);

        _camera.gameObject.SetActive(false);

        yield return new WaitForSeconds(2);

        _cutsceneEvent.show = false;
        EventController.TriggerEvent(_cutsceneEvent);

        _enableMovementEvent.canMove = true;
        EventController.TriggerEvent(_enableMovementEvent);
    }

    private QuestSO GetData()
    {
        QuestSO data = GetQuestData();

        if (data == null)return null;

        QUEST_STATE state = GetQuestState();
        string id = string.Format(DDParameters.FormatQuadruple, data.name, state.ToString(), DDParameters.SimpleQuest, gameObject.name);

        switch (state)
        {
            case QUEST_STATE.New:
                if (!GameData.Instance.CheckAndWriteID(id))
                {
                    return data;
                }
                break;

            case QUEST_STATE.Update:
                if (GameData.Instance.CheckQuestRequiredStep(data, GetQuestRequiredStep()) && !GameData.Instance.CheckID(id))
                {
                    GameData.Instance.WriteID(id);
                    return data;
                }
                break;

            case QUEST_STATE.Complete:
                if (GameData.Instance.HaveQuest(data) && !GameData.Instance.CheckID(id))
                {
                    GameData.Instance.WriteID(id);
                    return data;
                }
                break;
        }

        return null;
    }
}