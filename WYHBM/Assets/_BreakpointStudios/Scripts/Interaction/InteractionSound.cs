using Events;
using UnityEngine;

public class InteractionSound : Interaction, IInteractable, IHoldeable
{
    [Header("Sound")]
    [SerializeField, Range(0, 50)] private float _soundRadius = 10;
    [SerializeField] private bool _doOnce = false;
    [Space]
    [SerializeField] private WorldConfig _worldConfig = null;
    [SerializeField] private InputHoldUtility _holdUtility = null;

    [Header("Debug")]
    [SerializeField] private bool _showDebug = false;
    [SerializeField] private Color _color = new Color(0, 1, 0, 0.5f);

    private bool _used;
    private Collider[] _targetsInSoundRadius;
    private NPCController _currentNPC;

    private void Start()
    {
        if (_holdUtility != null)
        {
            _holdUtility.OnStarted.AddListener(OnStart);
            _holdUtility.OnCanceled.AddListener(OnCancel);
            _holdUtility.OnFinished.AddListener(OnFinish);
        }
    }

    public void OnInteractionEnter(Collider other)
    {
        if (_doOnce && _used)return;

        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<InteractionEvent>(OnInteractSound);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (_doOnce && _used)return;

        if (other.gameObject.CompareTag(Tags.Player))
        {
            _holdUtility.OnCancel();

            EventController.RemoveListener<InteractionEvent>(OnInteractSound);
        }
    }

    private void OnInteractSound(InteractionEvent evt)
    {
        if (evt.isStart)
        {
            _holdUtility.OnStart();
        }
        else
        {
            _holdUtility.OnCancel();
        }
    }

    #region Hold System

    public void OnStart()
    {
        ShowHint(false);
    }

    public void OnCancel()
    {
        ShowHint(true);
    }

    public void OnFinish()
    {
        _used = true;

        _targetsInSoundRadius = Physics.OverlapSphere(transform.position, _soundRadius, _worldConfig.layerNPC);

        for (int i = 0; i < _targetsInSoundRadius.Length; i++)
        {
            _currentNPC = _targetsInSoundRadius[i].GetComponent<NPCController>();

            if (_currentNPC != null)_currentNPC.SetDestination(transform.position);
        }

        ForceCleanInteraction();
    }

    #endregion

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (_showDebug)
        {
            Gizmos.color = _color;
            Gizmos.DrawWireSphere(transform.position, _soundRadius);
        }
    }

#endif
}