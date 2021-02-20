using Events;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupUtility))]
public class TutorialHint : MonoBehaviour
{
    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] protected FMODConfig _FMODConfig = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasUtility = null;
    [SerializeField, ConditionalHide] private InputInfo _inputInfo = null;

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
            _inputInfo.SetInputAction(evt.actionTutorial);
        }

        _canvasUtility.Show(evt.show);
    }

}