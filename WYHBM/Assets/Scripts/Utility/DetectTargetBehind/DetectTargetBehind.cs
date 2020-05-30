using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DetectTargetBehind : MonoBehaviour
{
    private Camera _camera;
    private Transform _target;
    private TargetBehind _currentTargetBehind;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _target = GameManager.Instance.globalController.player.transform;
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