using UnityEngine;

public class InteractionCorpse : Interaction, IInteractable
{
    [Header("Corpse")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public void Init(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
        }
    }

}