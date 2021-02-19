using Events;
using UnityEngine;

public class InteractionHint : MonoBehaviour
{
    private CanvasGroupUtility _canvasGroupUtility;
    private InputInfo _inputInfo;

    private void Awake()
    {
        _canvasGroupUtility = GetComponent<CanvasGroupUtility>();
        _inputInfo = GetComponentInChildren<InputInfo>();
    }

    // private void OnEnable()
    // {
    //     EventController.AddListener<ShowInteractionHintEvent>(OnShowInteractionHint);
    // }

    // private void OnDisable()
    // {
    //     EventController.RemoveListener<ShowInteractionHintEvent>(OnShowInteractionHint);
    // }
    
    // private void OnShowInteractionHint(ShowInteractionHintEvent evt)
    // {
    //     _canvasGroupUtility.Show(evt.show);
    // }

}