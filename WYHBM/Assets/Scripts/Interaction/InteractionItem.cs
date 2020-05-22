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

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            AddListenerQuest();
            EventController.AddListener<InteractionEvent>(OnInteractItem);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            RemoveListenerQuest();
            EventController.RemoveListener<InteractionEvent>(OnInteractItem);
        }
    }

    private void OnInteractItem(InteractionEvent evt)
    {
        if (GameManager.Instance.IsInventoryFull)
        {
            GameManager.Instance.worldUI.ShowPopup(GameData.Instance.textConfig.popupInventoryFull, false);
            return;
        }

        Slot newSlot = Instantiate(GameData.Instance.gameConfig.slotPrefab, GameManager.Instance.worldUI.itemParents);
        newSlot.AddItem(item);

        GameManager.Instance.dictionarySlot.Add(item.GetInstanceID(), newSlot);

        EventController.RemoveListener<InteractionEvent>(OnInteractItem);
        Destroy(gameObject);
    }

    public void AddInfo(ItemSO itemInfo)
    {
        item = itemInfo;
        _spriteRenderer.sprite = itemInfo.sprite;
    }

    public void DetectSize()
    {
        SpriteRenderer tempSpriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider tempBoxCollider = GetComponent<BoxCollider>();

        float pixelUnit = tempSpriteRenderer.sprite.pixelsPerUnit;
        float width = tempSpriteRenderer.sprite.texture.width / pixelUnit;
        float height = tempSpriteRenderer.sprite.texture.height / pixelUnit;

        if (width < 1)width = 1;
        if (height < .45f)height = .45f;

        tempBoxCollider.size = new Vector3(width, height, 1.5f);
    }

    #region Item Creator Tool

    /// <summary>
    /// Used in Item Creator Tool
    /// </summary>
    public void InstantiatePrefab(ItemSO item, QuestSO quest, int progress)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        // Set Values
        spriteRenderer.sprite = item.sprite;
        this.quest = quest;
        this.progress = progress;
        this.item = item;

        // Detect Size
        float pixelUnit = spriteRenderer.sprite.pixelsPerUnit;
        float width = spriteRenderer.sprite.rect.width / pixelUnit;
        float height = spriteRenderer.sprite.rect.height / pixelUnit;

        if (width < 1)width = 1;
        if (height < .45f)height = .45f;

        boxCollider.size = new Vector3(width, height, 1.5f);
    }

    #endregion
}