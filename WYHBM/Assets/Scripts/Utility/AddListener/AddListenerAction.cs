using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AddListenerAction : MonoBehaviour
{
    public COMBAT_STATE combatState;

    private void Start()
    {
        Button actionButton = GetComponent<Button>();
        actionButton.onClick.AddListener(() => GameManager.Instance.combatManager.DoAction(combatState));
    }

}