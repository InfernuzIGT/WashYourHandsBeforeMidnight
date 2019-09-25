using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionObject : MonoBehaviour
{
    public ActionSO action;

    [Header("Complete")]
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Image actionImg;
    [SerializeField] private TextMeshProUGUI shortcutTxt;

    private void Start()
    {
        SetAction();
    }

    private void Update()
    {
        if (Input.GetKeyDown(action.actionKey))
        {
            SelectAction();
        }
    }
    
    private void SetAction()
    {
        // TODO Mariano: Configure the action
        // actionImg.sprite = action.actionSprite;
        shortcutTxt.text = action.actionKey.ToString().Replace("Alpha", "");
    }

    private void SelectAction()
    {
        _toggle.isOn = true;
        CombatManager.Instance.actionController.ChooseAction(action);
        CombatManager.Instance.uiController.ChooseAction(action);
    }
}