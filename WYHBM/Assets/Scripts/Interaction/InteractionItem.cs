using Events;
using UnityEngine;

public class InteractionItem : Interaction, IInteractable
{
    public ItemSO item;
    private SpriteRenderer _spriteRenderer;

    public override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        AddInfo(item);
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<InteractionEvent>(OnInteractItem);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractItem);
        }
    }

    private void OnInteractItem(InteractionEvent evt)
    {
        if (GameManager.Instance.worldUI.inventorySlots.isFull)
        {
            Debug.Log($"<b> INVENTORY FULL! </b>");
            return;
        }

        Slot newSlot = Instantiate(GameData.Instance.gameConfig.slotPrefab, GameManager.Instance.worldUI.itemParents);
        newSlot.AddItem(item);
        GameManager.Instance.worldUI.inventorySlots.AddItemList(item);
        EventController.RemoveListener<InteractionEvent>(OnInteractItem);

        Destroy(gameObject);
    }

    public void AddInfo(ItemSO itemInfo)
    {
        item = itemInfo;
        _spriteRenderer.sprite = itemInfo.Sprite;
    }

}