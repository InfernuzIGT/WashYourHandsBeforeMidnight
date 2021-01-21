using UnityEngine;

public interface IInteractable
{
    void OnInteractionEnter(Collider other);
    void OnInteractionExit(Collider other);
}

public interface IDialogueable
{
    void DDQuest(QUEST_STATE state);
    bool DDFirstTime();
    bool DDFinished();
    bool DDCheckQuest();
    bool DDHaveQuest();
    void DDFinish();
}

public interface IHoldeable
{
    void OnStart();
    void OnCancel();
    void OnFinish();
}