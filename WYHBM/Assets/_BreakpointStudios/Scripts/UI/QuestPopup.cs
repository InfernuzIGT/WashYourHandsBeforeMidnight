using DG.Tweening;
using Events;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

[RequireComponent(typeof(CanvasGroupUtility))]
public class QuestPopup : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private LocalizedString _stateNone;
    [SerializeField] private LocalizedString _stateNew;
    [SerializeField] private LocalizedString _stateUpdate;
    [SerializeField] private LocalizedString _stateComplete;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] protected FMODConfig _FMODConfig = null;
    [SerializeField, ConditionalHide] private TextMeshProUGUI _titleTxt = null;
    [SerializeField, ConditionalHide] private TextMeshProUGUI _stateTxt = null;
    [SerializeField, ConditionalHide] private LocalizeStringEvent _localizeStringTitle = null;
    [SerializeField, ConditionalHide] private LocalizeStringEvent _localizeStringState = null;

    private CanvasGroupUtility _canvasUtility;
    private RectTransform _rectTransform;
    private bool _isPlaying;
    private Vector2 _originalPosition;
    private Tween _tweenStart;
    private Tween _tweenEnd;

    private void Awake()
    {
        _canvasUtility = GetComponent<CanvasGroupUtility>();

        _rectTransform = GetComponent<RectTransform>();
        _originalPosition = _rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        EventController.AddListener<QuestEvent>(OnQuestEvent);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<QuestEvent>(OnQuestEvent);
    }

    private void OnQuestEvent(QuestEvent evt)
    {
        _localizeStringTitle.StringReference = evt.data.title;
        _localizeStringTitle.OnUpdateString.Invoke(_titleTxt.text);

        _localizeStringState.StringReference = GetLocalizedState(evt.state);
        _localizeStringState.OnUpdateString.Invoke(_stateTxt.text);

        Play();
    }

    private LocalizedString GetLocalizedState(QUEST_STATE state)
    {
        switch (state)
        {
            case QUEST_STATE.New:
                return _stateNew;

            case QUEST_STATE.Update:
                return _stateUpdate;

            case QUEST_STATE.Complete:
                return _stateComplete;

            default:
                return _stateNone;
        }
    }

    private void Play()
    {
        if (_isPlaying)return;

        _isPlaying = true;

        RuntimeManager.PlayOneShot(_FMODConfig.popupDevice);

        _canvasUtility.Show(true);

        _rectTransform.anchoredPosition = _originalPosition;

        _tweenStart = transform
            .DOLocalMoveX(-350, .5f)
            .SetEase(Ease.InOutSine)
            .SetRelative()
            .SetLoops(0);

        _tweenEnd = transform
            .DOLocalMoveX(350, .5f)
            .SetEase(Ease.InOutSine)
            .SetRelative()
            .SetLoops(0)
            .SetDelay(5)
            .OnComplete(Complete);
    }

    public void Complete()
    {
        _canvasUtility.ShowInstant(false);

        _tweenStart.Kill();
        _tweenEnd.Kill();

        _isPlaying = false;
    }

}