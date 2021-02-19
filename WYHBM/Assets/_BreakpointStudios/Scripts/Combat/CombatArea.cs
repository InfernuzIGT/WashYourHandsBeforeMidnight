using Cinemachine;
using UnityEngine;

public class CombatArea : MonoBehaviour
{
    [Header("Combat Area")]
    public CinemachineVirtualCamera virtualCamera;
    [Space]
    public Transform[] playerPosition;
    public Transform[] enemyPosition;

    [Header("Debug")]
    public SpriteRenderer[] spriteReferences;

    private void Start()
    {
        for (int i = 0; i < spriteReferences.Length; i++)
        {
            spriteReferences[i].enabled = false;
        }
    }

}