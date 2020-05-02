using System.Collections;
using Events;
using UnityEngine;

public class InteractionItem : Interaction, IInteractable
{
    public ItemSO item;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        SetSprite(item);

    }
    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameData.Instance.gameConfig.tagPlayer))
        {
            EventController.AddListener<InteractionEvent>(OnInteractItem);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(GameData.Instance.gameConfig.tagPlayer))
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractItem);
        }
    }

    private void OnInteractItem(InteractionEvent evt)
    {
        if (GameManager.Instance.worldUI.inventorySlots.isAdded)
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractItem);
            Destroy(gameObject);
        }
        else
        {
            if (GameManager.Instance.worldUI.inventorySlots.isFull)
            {

                return;
            }
            else
            {
                Slot newSlot = Instantiate(GameData.Instance.gameConfig.slotPrefab, GameManager.Instance.worldUI.itemParents);
                newSlot.AddItem(item);
                GameManager.Instance.worldUI.inventorySlots.AddItemList(item);

            }
        }

    }

    public void AddInfo(ItemSO itemInfo)
    {
        item = itemInfo;
        _spriteRenderer.sprite = itemInfo.icon;

    }

    public void SetSprite(ItemSO ItemInfo)
    {
        item = ItemInfo;
        if (_spriteRenderer == null)
        {
            ItemInfo.icon = _spriteRenderer.sprite;
            
        }

    }

}