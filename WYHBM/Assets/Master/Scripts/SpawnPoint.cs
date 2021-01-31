using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpawnPoint : MonoBehaviour
{
    [Header("Spawn Point")]
    public PlayerSO player;
    [Space]
    [SerializeField] private SpriteRenderer _spriteRenderer = null;

    [Header("Combat Areas")]
    [SerializeField] private CombatArea[] _combatAreas;

    private void Start()
    {
        _spriteRenderer.enabled = false;
    }

    public void SetSprite()
    {
        if (_spriteRenderer == null)_spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = player.Sprite;
    }

    public CombatArea GetCombatArea()
    {
        return _combatAreas[Random.Range(0, _combatAreas.Length)];
    }

}