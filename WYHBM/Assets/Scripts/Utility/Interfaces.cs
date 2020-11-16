using UnityEngine;

public interface IInteractable
{
    void OnInteractionEnter(Collider other);
    void OnInteractionExit(Collider other);
}

public interface IDialogueable
{
    void DDGiveReward();
    bool DDHaveAmount();
}