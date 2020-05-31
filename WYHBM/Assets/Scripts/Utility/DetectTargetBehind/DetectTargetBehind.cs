using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DetectTargetBehind : MonoBehaviour
{
    private Transform _target;
    private TargetBehind _currentTargetBehind;

    private void Start()
    {
        _target = transform; // Placeholder
    }

    private void Update()
    {
        DetectTarget();
    }

    private void DetectTarget()
    {
        if (Physics.Linecast(transform.position, _target.position, out RaycastHit hit, GameData.Instance.worldConfig.layerOcclusionMask, QueryTriggerInteraction.UseGlobal))
        {
            if (_currentTargetBehind != hit.transform.gameObject)
            {
                _currentTargetBehind?.Detected(false);
                _currentTargetBehind = hit.transform.gameObject.GetComponent<TargetBehind>();
                _currentTargetBehind.Detected(true);
            }
        }
        else
        {
            _currentTargetBehind?.Detected(false);
            _currentTargetBehind = null;
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

}