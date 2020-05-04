using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionObject : MonoBehaviour
{
    public EquipmentSO equipment;

    [Header("Complete")]
    [SerializeField] private Toggle _toggle = null;
    [SerializeField] private Image actionImg = null;
    [SerializeField] private TextMeshProUGUI shortcutTxt = null;

    private void Start()
    {
        _toggle.onValueChanged.AddListener(delegate { SelectAction(_toggle); });
        // _toggle.group = CombatManager.Instance.uIController.panelActions.toggleGroup;
        SetAction();
    }

    private void Update()
    {
        if (Input.GetKeyDown(equipment.actionKey))
        {
            SelectAction();
        }
    }

    private void SetAction()
    {
        actionImg.sprite = equipment.previewSprite;
        shortcutTxt.text = equipment.actionKey.ToString().Replace("Alpha", "");

        if (_toggle.isOn)SelectAction();
    }

    public void SelectAction()
    {
        _toggle.isOn = true;
        // CombatManager.Instance.actionController.ChooseAction(equipment, equipment.valueMin, equipment.valueMax);
        // CombatManager.Instance.uIController.ChooseAction(equipment);
    }

    private void SelectAction(bool isOn)
    {
        if (isOn)
        {
            // CombatManager.Instance.actionController.ChooseAction(equipment, equipment.valueMin, equipment.valueMax);
            // CombatManager.Instance.uIController.ChooseAction(equipment);
        }
    }
}