using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class CameraDirection : MonoBehaviour
{
    [SerializeField] private bool _active;
    [SerializeField, ReadOnly] private Vector3 _direction;

    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (_active)
        {
            _direction = _camera.transform.forward;
        }

    }
}