using System.Collections.Generic;
using UnityEngine;

public class Carousel : MonoBehaviour
{
    [Header("Carousel")]
    [SerializeField] private List<GameObject> _listCarousel;
    [Space]
    [SerializeField, Range(0, 10)] private float _distanceFromCenter = 2;
    [SerializeField, Range(0, 1)] private float _speedRotation = 0.25f;
    [Space]
    [SerializeField, ReadOnly] private int _selectedIndex = 0;
    [SerializeField, ReadOnly] private GameObject _selectedGameObject = null;

    private float _angle;
    private float _rotationAngle;

    private bool _resetCenterRotation = true;
    private bool _isFirstTime = true;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (_resetCenterRotation && _isFirstTime)transform.rotation = Quaternion.identity;

        _angle = 360 / (float)_listCarousel.Count;
        float tempAngle = _angle;

        for (int i = 0; i < _listCarousel.Count; i++)
        {
            _listCarousel[i].transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            _listCarousel[i].transform.SetParent(transform);
            _listCarousel[i].transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + _distanceFromCenter);
            _listCarousel[i].transform.RotateAround(transform.position, Vector3.up, tempAngle);
            tempAngle += _angle;
        }

        if (_listCarousel.Count % 2 != 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, _angle + _angle / 2, transform.eulerAngles.z);
            _rotationAngle = _angle + _angle / 2;
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, _angle, transform.eulerAngles.z);
            _rotationAngle = _angle;
        }

        for (int i = 0; i < _listCarousel.Count; i++)
        {
            if (_listCarousel[i].name == _listCarousel[0].name)
            {
                float angleMultiplier = _listCarousel.Count == 3 ? 5 : i++; // Fix Carousel of 3 Objects

                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + _angle * angleMultiplier, transform.eulerAngles.z);
                _rotationAngle = transform.eulerAngles.y;

                break;
            }
        }

        _selectedGameObject = _listCarousel[_selectedIndex];
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(_rotationAngle, Vector3.up), _speedRotation);
    }

    [ContextMenu("Rotate Left")]
    public void RotateLeft()
    {
        if (_isFirstTime)
        {
            _rotationAngle = transform.eulerAngles.y;
            _rotationAngle += _angle;
            _isFirstTime = false;
        }
        else
        {
            _rotationAngle += _angle;
        }

        if (_selectedIndex <= 0)
        {
            _selectedIndex = _listCarousel.Count - 1;
        }
        else
        {
            _selectedIndex--;
        }

        _selectedGameObject = _listCarousel[_selectedIndex];
    }

    [ContextMenu("Rotate Right")]
    public void RotateRight()
    {
        if (_isFirstTime)
        {
            _rotationAngle = transform.eulerAngles.y;
            _rotationAngle -= _angle;
            _isFirstTime = false;
        }
        else
        {
            _rotationAngle -= _angle;
        }

        if (_selectedIndex >= _listCarousel.Count - 1)
        {
            _selectedIndex = 0;
        }
        else
        {
            _selectedIndex++;
        }

        _selectedGameObject = _listCarousel[_selectedIndex];
    }
}