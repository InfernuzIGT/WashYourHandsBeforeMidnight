using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpawnPoint : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private CombatArea[] _combatAreas;

    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    public CombatArea GetCombatArea()
    {
        return _combatAreas[Random.Range(0, _combatAreas.Length)];
    }

}