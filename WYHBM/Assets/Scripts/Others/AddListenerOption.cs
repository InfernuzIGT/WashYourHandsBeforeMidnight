using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AddListenerOption : MonoBehaviour
{
    public OPTION_TYPE optionType;

    private void Start()
    {
        Button optionButton = GetComponent<Button>();
        optionButton.onClick.AddListener(() => UIManager.Instance.SelectOption(optionType));
    }

}