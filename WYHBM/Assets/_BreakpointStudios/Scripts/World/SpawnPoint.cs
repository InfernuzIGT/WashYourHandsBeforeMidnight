using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpawnPoint : MonoBehaviour
{
    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
}