using UnityEngine;

public class Item : MonoBehaviour
{
    private SpriteRenderer _spriteRnd;

    private void Awake()
    {

        _spriteRnd = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        if (_spriteRnd == null)
        {
            

        }

    }

}