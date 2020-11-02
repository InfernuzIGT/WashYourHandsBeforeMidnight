using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ChestItems
{
    public ItemSO item;
    [Range(0f, 1f)] public double probability;
}

public class InteractionChest : Interaction, IInteractable
{
    [Header("Chest")]
    public float holdDuration = 3;
    public List<ChestItems> items;

    private ItemSO _item;
    private ProportionValue<ItemSO>[] _listItems;

    private void Start()
    {
        _listItems = new ProportionValue<ItemSO>[items.Count];

        for (int i = 0; i < items.Count; i++)
        {
            _listItems[i] = ProportionValue.Create(items[i].probability, items[i].item);
        }
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            GameManager.Instance.globalController.playerController.ToggleInputHold(true);
            GameManager.Instance.SetHoldSystem(ref GameManager.Instance.worldUI.progressImg, holdDuration, Success);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            GameManager.Instance.globalController.playerController.ToggleInputHold(false);
        }
    }

    private void Success()
    {
        _item = _listItems.ChooseByRandom();
        Debug.Log($"<b> SUCCESS: </b> {_item.name}");
        // TODO Marcos: Agregar _item al inventario

        GameManager.Instance.globalController.playerController.ToggleInputHold(false);
        Destroy(gameObject);
    }

}