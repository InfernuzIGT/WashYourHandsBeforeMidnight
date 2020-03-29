using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DetectTargetBehind : MonoBehaviour
{
    [SerializeField]
    public Transform _target;
    [SerializeField]
    public LayerMask _layerMask;

    private Camera _camera;
    private TargetBehind _currentTargetBehind;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        DetectTarget();
    }

    private void DetectTarget()
    {
        if (Physics.Linecast(transform.position, _target.position, out RaycastHit hit, _layerMask, QueryTriggerInteraction.UseGlobal))
        {
            if (_currentTargetBehind != hit.transform.gameObject)
            {
                _currentTargetBehind = hit.transform.gameObject.GetComponent<TargetBehind>();
                _currentTargetBehind.Detected(true);
            }
        }
        else
        {
            if (_currentTargetBehind != null)
            {
                _currentTargetBehind.Detected(false);
            }

            _currentTargetBehind = null;
        }
    }

}