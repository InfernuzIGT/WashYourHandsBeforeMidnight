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

    public GameObject questWindow;

    // private Canvas _questUI;

    private void Awake() 
    {   
        _quest = GetComponent<Quest>();
        // _questUI = GetComponent<Canvas>();
    }

    public void OpenQuestWindow() //discuss if mockup have window
    {
        // _questUI.enabled = true;
        questWindow.SetActive(true);
        titleText.text = _quest.title;
        descriptionText.text = _quest.description;
        experienceText.text = _quest.experienceReward.ToString();
        goldText.text = _quest.goldReward.ToString();
    }

    public void AcceptQuest()
    {
        // _questUI.enabled = true;
        questWindow.SetActive(false);
        Quest.isActive = true;
        playerController.quest = _quest;

    }
}
