using Events;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroupUtility))]
public class TutorialPanel : MonoBehaviour
{
    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] protected FMODConfig _FMODConfig = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasUtility = null;
    [Space]
    [SerializeField, ConditionalHide] private Image _tutorialImg = null;
    [SerializeField, ConditionalHide] private TextMeshProUGUI _tutorialTxt = null;
    [SerializeField, ConditionalHide] private LocalizeStringEvent _localizeStringEvent = null;

    private GameObject _lastGameObject;

    private void OnEnable()
    {
        EventController.AddListener<TutorialEvent>(OnTutorial);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<TutorialEvent>(OnTutorial);
    }

    private void OnTutorial(TutorialEvent evt)
    {
        if (evt.show)
        {
            RuntimeManager.PlayOneShot(_FMODConfig.popupDevice);

            _tutorialImg.sprite = evt.data.image;
            _localizeStringEvent.StringReference = evt.data.description;
            _localizeStringEvent.OnUpdateString.Invoke(_tutorialTxt.text);

            // _lastGameObject = EventSystemUtility.Instance.GetSelectedGameObject();
            // EventSystemUtility.Instance.SetSelectedGameObject(_tutorialImg.gameObject);

            _canvasUtility.Show(true);
        }
        else
        {
            // EventSystemUtility.Instance.SetSelectedGameObject(_lastGameObject);

            _canvasUtility.ShowInstant(false);
        }
    }

}