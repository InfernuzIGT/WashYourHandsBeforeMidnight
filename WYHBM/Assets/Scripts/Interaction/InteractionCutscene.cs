using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.Playables;

public class InteractionCutscene : Interaction, IInteractable
{
    [Header("Cutscene")]
    [SerializeField] private PlayableAsset cutscene;
    [SerializeField] private bool instantLetterbox;
    // [SerializeField] private bool hasFade;

    private CutsceneEvent _cutsceneEvent;
    private FadeEvent _fadeEvent;
    private ChangeInputEvent _changeInputEvent;

    public override void Awake()
    {
        base.Awake();

        _cutsceneEvent = new CutsceneEvent();
        _cutsceneEvent.cutscene = cutscene;
        _cutsceneEvent.instantLetterbox = instantLetterbox;
        _cutsceneEvent.show = true;

        _changeInputEvent = new ChangeInputEvent();
        _changeInputEvent.enable = false;

        _showHint = false;
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            if (cutscene == null)return;

            StartCoroutine(TriggerCutscene());
        }
    }

    public void OnInteractionExit(Collider other) { }

    private IEnumerator TriggerCutscene()
    {
        // if (hasFade)
        // {

        //     yield return new WaitForSeconds(1);
        // }

        // TODO Mariano: SAVE

        EventController.TriggerEvent(_cutsceneEvent);
        EventController.TriggerEvent(_changeInputEvent);

        Destroy(this.gameObject);

        yield return null;
    }

}