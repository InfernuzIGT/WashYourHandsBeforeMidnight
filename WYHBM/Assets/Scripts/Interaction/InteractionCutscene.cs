using Events;
using UnityEngine;

public class InteractionCutscene : Interaction, IInteractable
{
    private CutsceneEvent _cutsceneEvent;
    public DialogSO dialog;

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
        {
            if (cutscene == null) return;

            _cutsceneEvent.cutscene = cutscene;

            EventController.TriggerEvent(_cutsceneEvent);
        }

        if (!playInCollision)
        {
            PlayCutscene();
            Destroy(this.gameObject);
        }

    }

}