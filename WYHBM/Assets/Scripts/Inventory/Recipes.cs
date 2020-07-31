using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New recipe ", menuName = "Recipe", order = 0)]
public class Recipes : ScriptableObject
{
    public Ingredients[] ingredients;
    // public TextMeshProUGUI titleTxt;
    // public TextMeshProUGUI descriptionTxt;
    // public Image icon;
    // public int maxValue;
    // public int minValue;
    // public int actualValue;

    // public List<>

    void Start()
    {

    }

    void Update()
    {

    }

    public void Craft()
    {
        //check ingredients
    }

    public void CheckIngredients()
    {
        
    }
}

[Serializable]
public struct Ingredients
{
    public ItemSO item;
    public int requiredValue;

}