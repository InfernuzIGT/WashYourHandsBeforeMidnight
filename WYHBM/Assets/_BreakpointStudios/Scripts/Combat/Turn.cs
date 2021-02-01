using UnityEngine;
using UnityEngine.UI;

public class Turn : MonoBehaviour
{
    private Image _characterImg;

    private void Start()
    {
        _characterImg = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void SetSprite(CombatCharacter character)
    {
        _characterImg = GetComponent<Image>();
        _characterImg.sprite = character.Data.TurnSprite;
    }

}