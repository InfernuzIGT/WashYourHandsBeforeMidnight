using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.Playables;

public class InteractionCutscene : Interaction, IInteractable
{
    private CutsceneEvent _cutsceneEvent;
    // public DialogSO dialog;
    // private EnableDialogEvent _interactionDialogEvent;

    // private void Start()
    // {
    //     _interactionDialogEvent = new EnableDialogEvent();
    //     _interactionDialogEvent.dialog = dialog;
    // }

    // public override void Execute(bool enable, NPCController currentNPC)
    // {
    //     base.Execute();

    //     _interactionDialogEvent.enable = enable;
    //     EventController.TriggerEvent(_interactionDialogEvent);
    // }

    public override void Awake()
    {
        base.Awake();

        _cutsceneEvent = new CutsceneEvent();
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            if (playInCollision)
            {
                PlayCutscene();
                Destroy(this.gameObject);
            }

            EventController.AddListener<InteractionEvent>(OnInteractCutscene);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractCutscene);
        }
    }

    private void OnInteractCutscene(InteractionEvent evt)
    {
        if (!playInCollision)
        {
            PlayCutscene();
            Destroy(this.gameObject);
        }

        {
            if (cutscene == null) return;

            _cutsceneEvent.cutscene = cutscene;

            EventController.TriggerEvent(_cutsceneEvent);

        }
        Destroy(this);

        if (GameManager.Instance.playableDirector.state == PlayState.Playing)
        {
            Debug.Log($"<b> PLAYING </b>");
            Destroy(this);
        }
    }

}