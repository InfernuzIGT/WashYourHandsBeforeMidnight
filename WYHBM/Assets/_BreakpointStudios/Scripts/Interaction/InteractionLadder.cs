using Events;
using UnityEngine;

public enum LADDER_TYPE
{
    Interaction = 0,
    Bot = 1,
    Top = 2,
}

public class InteractionLadder : Interaction, IInteractable
{
    private bool _inLadder;

    private ChangePositionEvent _changePositionEvent;
    private LadderEvent _ladderEvent;

    private void Start()
    {
        _changePositionEvent = new ChangePositionEvent();
        _changePositionEvent.newPosition = transform.position;
        _changePositionEvent.offset = new Vector3(0, 0.5f, 0);
        _changePositionEvent.useY = false;

        _ladderEvent = new LadderEvent();
        _ladderEvent.type = LADDER_TYPE.Interaction;
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<InteractionEvent>(OnInteractionLadder);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            if (_inLadder)
            {
                OnLadder(LADDER_TYPE.Top);
            }
            else
            {
                EventController.RemoveListener<InteractionEvent>(OnInteractionLadder);
            }
        }
    }

    private void OnInteractionLadder(InteractionEvent evt)
    {
        if (!evt.isStart)return;

        _inLadder = !_inLadder;

        if (_inLadder)
        {
            EventController.TriggerEvent(_ladderEvent);
            EventController.TriggerEvent(_changePositionEvent);

            OnLadder(LADDER_TYPE.Interaction);
        }
        else
        {
            OnLadder(LADDER_TYPE.Bot);
        }
    }

    private void OnLadderEvent(LadderEvent evt)
    {
        OnLadder(evt.type);
    }

    private void OnLadder(LADDER_TYPE type)
    {
        switch (type)
        {
            case LADDER_TYPE.Interaction:
                EventController.AddListener<LadderEvent>(OnLadderEvent);
                break;

            case LADDER_TYPE.Bot:
            case LADDER_TYPE.Top:

                _inLadder = false;
                EventController.RemoveListener<InteractionEvent>(OnInteractionLadder);
                EventController.RemoveListener<LadderEvent>(OnLadderEvent);

                _ladderEvent.type = LADDER_TYPE.Interaction;
                EventController.TriggerEvent(_ladderEvent);
                break;

        }
    }
}