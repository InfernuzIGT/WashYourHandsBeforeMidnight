using UnityEngine;

public class AddCameraCanvas : MonoBehaviour
{
    private void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

}