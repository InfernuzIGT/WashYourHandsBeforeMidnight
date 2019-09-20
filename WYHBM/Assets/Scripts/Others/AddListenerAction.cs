using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AddListenerAction : MonoBehaviour
{
    public ACTION_TYPE actionType;

    private void Start()
    {
        Button actionButton = GetComponent<Button>();
        actionButton.onClick.AddListener(() => UIManager.Instance.SelectAction(actionType));
    }

}