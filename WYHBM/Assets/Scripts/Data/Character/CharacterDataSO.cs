using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character Data", order = 0)]
public class CharacterDataSO : ScriptableObject
{
    [Header("Data")]
    [SerializeField] private CharacterSO[] character = null;

    private Dictionary<string, CharacterSO> characterDictionary;

    public CharacterSO GetCharacterByName(string name)
    {
        if (characterDictionary.ContainsKey(name))
        {
            return characterDictionary[name];
        }
        else
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't find Character {name}");
            return null;
        }
    }

    public void UpdateDictionary()
    {
        characterDictionary = new Dictionary<string, CharacterSO>();

        for (int i = 0; i < character.Length; i++)
        {
            if (!characterDictionary.ContainsKey(character[i].Name))
            {
                characterDictionary.Add(character[i].Name, character[i]);
            }
        }
    }

}