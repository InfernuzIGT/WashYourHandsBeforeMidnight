using Events;
using UnityEngine;

public class InteractionItem : Interaction, IInteractable
{
    public ItemSO item;
    private SpriteRenderer _spriteRenderer;
    // public bool isPicked;

    public override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Items SO for each prefab

        // if (GameManager.Instance.Items.Contains(item))
        // {
        //     isPicked = true;
        //     DeleteItems();
        // }
    }

    // private void DeleteItems()
    // {
    //     if (!isPicked)
    //     {
    //         return;
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            // if (cutsceneData.playInCollision)PlayCutscene();

            // AddListenerQuest();
            EventController.AddListener<InteractionEvent>(OnInteractItem);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            // RemoveListenerQuest();
            EventController.RemoveListener<InteractionEvent>(OnInteractItem);
        }
    }

    private void OnInteractItem(InteractionEvent evt)
    {
        // if (GameManager.Instance.IsInventoryFull)
        // {
        //     GameManager.Instance.worldUI.ShowPopup(GameData.Instance.textConfig.popupInventoryFull, false);
        //     return;
        // }

        // for (int i = 0; i < GameManager.Instance.CurrentQuestData.progress[item.index]; i++)
        // {
        //     item.index = i;

        //     if (i == GameManager.Instance.CurrentQuestData.progress[i])
        //     {
        //         this.enabled = true;
        //     }
        // }

        // if (!cutsceneData.playInCollision)PlayCutscene();

        // Slot newSlot = Instantiate(GameData.Instance.worldConfig.slotPrefab, GameManager.Instance.worldUI.itemParents);
        // newSlot.AddItem(item);

        // GameManager.Instance.listSlots.Add(newSlot);

        // EventController.RemoveListener<InteractionEvent>(OnInteractItem);
        // Destroy(gameObject);
    }

    public void AddInfo(ItemSO itemInfo)
    {
        item = itemInfo;
        _spriteRenderer.sprite = itemInfo.icon;
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
        spriteRenderer.sprite = item.icon;
        // this.questData.quest = quest;
        // this.questData.progress[0] = progress;
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