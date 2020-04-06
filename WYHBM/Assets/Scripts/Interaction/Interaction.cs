using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Interaction : MonoBehaviour
{
    [System.Serializable]
    public class InteractionEvent : UnityEvent<Collider> { }

    public bool showPopup = true;
    [Space]
    public InteractionEvent onEnter;
    public InteractionEvent onExit;

    private GameObject _popupGO;

    private void Awake()
    {
        _popupGO = transform.GetChild(0).gameObject; // TODO Mariano: Review
        _popupGO.SetActive(false);
    }

    #region Interaction

    private void OnTriggerEnter(Collider other)
    {
        onEnter.Invoke(other);
        ShowPopup(true);
    }

    private void OnTriggerExit(Collider other)
    {
        onExit.Invoke(other);
        ShowPopup(false);
    }

    private void ShowPopup(bool show)
    {
        if (!showPopup)return;

        _popupGO.SetActive(show);
    }

    #endregion

    #region Execution

    public virtual void Execute() { }
    public virtual void Execute(bool enable) { }
    
    #endregion
}