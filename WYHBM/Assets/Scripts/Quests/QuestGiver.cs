using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{

    private Quest _quest;

    PlayerController playerController;

    public Text titleText;
    public Text descriptionText;
    public Text experienceText;
    public Text goldText;

    public GameObject questLog;

    private Canvas _questDiary;

    private void Awake() 
    {   
        _quest = GetComponent<Quest>();
        _questDiary = GetComponent<Canvas>();
    }

    public void OpenDiary() //discuss if mockup have window
    {
        _questDiary.enabled = true;
    }
    public void CloseDiary() //discuss if mockup have window
    {
        _questDiary.enabled = true;
    }

    public void AcceptQuest()
    {
        Quest.isActive = true;
        playerController.quest = _quest;
        titleText.text = _quest.title;
        descriptionText.text = _quest.description;
        experienceText.text = _quest.experienceReward.ToString();
        goldText.text = _quest.goldReward.ToString();

    }
}
