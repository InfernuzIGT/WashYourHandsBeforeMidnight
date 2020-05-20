using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AddListenerAction : MonoBehaviour
{
    public ACTION_TYPE actionType;

    private void Start()
    {
        Button actionButton = GetComponent<Button>();
        // TODO Mariano: Enable
        // actionButton.onClick.AddListener(() => CombatManager.Instance.uIController.SelectAction(actionType));
    }

}