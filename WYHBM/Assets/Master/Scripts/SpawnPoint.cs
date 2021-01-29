using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpawnPoint : MonoBehaviour
{
    [Header("Spawn Point")]
    public PlayerSO player;
    [Space]
    [SerializeField] private SpriteRenderer _spriteRenderer = null;

    private void Start()
    {
        // _spriteRenderer.enabled = false;
    }

    public void SetSprite()
    {
        if (_spriteRenderer == null)_spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = player.Sprite;
    }
    
}