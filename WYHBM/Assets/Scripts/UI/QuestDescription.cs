using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestDescription : MonoBehaviour
{
    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI descriptionTxt;
    [Space]
    public Transform container;

    private List<TextMeshProUGUI> _listObjectives;
    private TextMeshProUGUI _lastObjective;
    private QuestSO _quest;
    private int _currentIndex;

    public void Init(QuestSO quest)
    {
        _quest = quest;

        titleTxt.text = quest.title;
        descriptionTxt.text = quest.description;

        _currentIndex = 0;
        _listObjectives = new List<TextMeshProUGUI>();

        for (int i = 0; i < quest.objetives.Length; i++)
        {
            TextMeshProUGUI questObjetive = Instantiate(GameData.Instance.gameConfig.questObjetivePrefab, container);
            questObjetive.text = quest.objetives[i];
            questObjetive.gameObject.SetActive(false);
            _listObjectives.Add(questObjetive);
        }

        _listObjectives[0].gameObject.SetActive(true);
        _lastObjective = _listObjectives[0];
    }

    public void UpdateObjetives()
    {
        _lastObjective.fontStyle = FontStyles.Strikethrough;
        _lastObjective.color = Color.grey;

        _currentIndex++;

        if (_currentIndex < _quest.objetives.Length)
        {
            _listObjectives[_currentIndex].gameObject.SetActive(true);
            _lastObjective = _listObjectives[_currentIndex];
        }
    }

}