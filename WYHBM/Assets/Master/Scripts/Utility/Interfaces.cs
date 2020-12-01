using UnityEngine;

public interface IInteractable
{
    void OnInteractionEnter(Collider other);
    void OnInteractionExit(Collider other);
}

public interface IDialogueable
{
    void DDNewQuest();
    void DDUpdateQuest();
    void DDCompleteQuest();
    bool DDFirstTime();
    bool DDFinished();
}