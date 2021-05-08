using Events;
using UnityEngine;

public enum ACTION_TYPE
{
    None = 0,
    Attack = 1,
    Items = 2,
    Skills = 3,
    Strategies = 4,
}

public class ActionCommand : MonoBehaviour
{
    [Header("Action Command")]
    [SerializeField] private ACTION_TYPE _type = ACTION_TYPE.None;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private MeshRenderer _mesh;

    private ActionCommandEvent _actionCommandEvent;
    private int hash_IsSelected = Shader.PropertyToID("_ISSELECTED");

    private void Start()
    {
        _actionCommandEvent = new ActionCommandEvent();
        _actionCommandEvent.type = _type;

        _mesh.material.SetFloat(hash_IsSelected, 0);
    }

    public void SetSelection(bool isSelected)
    {
        _mesh.material.SetFloat(hash_IsSelected, isSelected ? 1 : 0);
    }

    public void Execute()
    {
        EventController.TriggerEvent(_actionCommandEvent);
    }

    public ACTION_TYPE GetCurrentType()
    {
        return _type;
    }

}