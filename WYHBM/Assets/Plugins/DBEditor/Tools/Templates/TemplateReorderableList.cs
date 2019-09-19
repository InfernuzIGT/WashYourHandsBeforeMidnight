using System.Collections.Generic;
using UnityEngine;

public class TemplateReorderableList : MonoBehaviour
{
    public List<Drink> Drinks;

    [System.Serializable]
    public struct Drink
    {
        public string Name;
        [Range(0, 20)]
        public float Price;
        public Color Color;
    }
}