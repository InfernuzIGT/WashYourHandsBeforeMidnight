using UnityEngine;

public class InteractionGrass : Interaction, IInteractable
{
    private Grass[] _grassArray;

    public override void Awake() { }

    private void Start()
    {
        _grassArray = GetComponentsInChildren<Grass>();
    }

    public void OnInteractionEnter(Collider other)
    {
        for (int i = 0; i < _grassArray.Length; i++)
        {
            _grassArray[i].EnableMovement(true);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        for (int i = 0; i < _grassArray.Length; i++)
        {
            _grassArray[i].EnableMovement(true);
        }
    }

}