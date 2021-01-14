using Events;
using UnityEngine;
using UnityEngine.Playables;

public class InteractionCutscene : Interaction, IInteractable
{
    [Header("Cutscene")]
    [SerializeField] private PlayableAsset cutscene = null;

    private CutsceneEvent _cutsceneEvent;
    private FadeEvent _fadeEvent;
    private ChangeInputEvent _changeInputEvent;
    private InteractionEvent _interactionEvent;

    public override void Awake()
    {
        base.Awake();

        _cutsceneEvent = new CutsceneEvent();
        _cutsceneEvent.show = true;

        _changeInputEvent = new ChangeInputEvent();
        _changeInputEvent.enable = false;

        _showHint = false;
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            TriggerCutscene();
        }
    }

    public void OnInteractionExit(Collider other) { }

    private void TriggerCutscene()
    {
        if (cutscene == null)return;

        if (CheckAndWriteCutscene())
        {
            Destroy(this.gameObject);
            return;
        }

        _cutsceneEvent.cutscene = cutscene;
        _cutsceneEvent.playerData = GameData.Instance.PlayerData;

        EventController.TriggerEvent(_cutsceneEvent);
        EventController.TriggerEvent(_changeInputEvent);

        Destroy(this.gameObject);
    }

    private bool CheckAndWriteCutscene()
    {
        return GameData.Instance.CheckAndWriteID(string.Format(DDParameters.Format, cutscene.name, DDParameters.Cutscene));
    }

    private bool CheckCutscene()
    {
        return GameData.Instance.CheckID(string.Format(DDParameters.Format, cutscene.name, DDParameters.Cutscene));
    }

}