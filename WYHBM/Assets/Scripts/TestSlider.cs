using UnityEngine;
using UnityEngine.UI;

public class TestSlider : MonoBehaviour
{
    Scrollbar slider;

    private void Awake()
    {
        slider = GetComponent<Scrollbar>();
        slider.onValueChanged.AddListener(SliderTest);
    }

    public void SliderTest(float value)
    {
        Debug.Log($"<b> Value: {value} </b>");
    }
}