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
        _toggle.onValueChanged.AddListener(delegate { SelectAction(_toggle); });
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

        if (_toggle.isOn)SelectAction();
    }

    private void SelectAction()
    {
        _toggle.isOn = true;
        CombatManager.Instance.actionController.ChooseAction(action, action.value);
        CombatManager.Instance.uiController.ChooseAction(action);
    }

    private void SelectAction(bool isOn)
    {
        if (isOn)
        {
            CombatManager.Instance.actionController.ChooseAction(action, action.value);
            CombatManager.Instance.uiController.ChooseAction(action);
        }
    }
}