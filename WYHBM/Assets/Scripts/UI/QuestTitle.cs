using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuestTitle : MonoBehaviour
{
    private QuestSO _quest;
    private TextMeshProUGUI _titleTxt;

    public void Init(QuestSO quest)
    {
        Button optionButton = GetComponent<Button>();
        optionButton.onClick.AddListener(() => GameManager.Instance.worldUI.SelectQuest(_quest));

        _titleTxt = GetComponentInChildren<TextMeshProUGUI>();

        _quest = quest;
        _titleTxt.text = quest.title;
    }

    public void Complete()
    {
        _titleTxt.fontStyle = FontStyles.Strikethrough;
        _titleTxt.color = Color.grey;
    }
}