using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject objectToShow = null;

    public void OnPointerEnter(PointerEventData eventData)
    {
        objectToShow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        objectToShow.SetActive(false);
    }
}