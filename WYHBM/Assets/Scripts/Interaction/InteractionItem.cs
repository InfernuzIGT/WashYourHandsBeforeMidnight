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
            GameManager.Instance.worldUI.InventoryPopUp();
            Debug.Log($"<b> INVENTORY FULL! </b>");
            return;
        }

        Slot newSlot = Instantiate(GameData.Instance.gameConfig.slotPrefab, GameManager.Instance.worldUI.itemParents);
        newSlot.AddItem(item);
        EventController.RemoveListener<InteractionEvent>(OnInteractItem);

        Destroy(gameObject);
    }

    public void AddInfo(ItemSO itemInfo)
    {
        item = itemInfo;
        _spriteRenderer.sprite = itemInfo.Sprite;
    }

    [ContextMenu("Detect Size")]
    public void DetectSize()
    {
        SpriteRenderer tempSpriteRenderer= GetComponent<SpriteRenderer>();
        BoxCollider tempBoxCollider = GetComponent<BoxCollider>();

        float pixelUnit = tempSpriteRenderer.sprite.pixelsPerUnit;
        float width = tempSpriteRenderer.sprite.texture.width /pixelUnit;
        float height = tempSpriteRenderer.sprite.texture.height /pixelUnit;

        if (width < 1) width = 1;
        if (height < .45f) height = .45f;

        tempBoxCollider.size = new Vector3 (width, height, 1.5f);
    }
}