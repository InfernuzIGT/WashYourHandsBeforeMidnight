﻿using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Character", menuName = "Characters/Combat", order = 10)]
public class CombatCharacterSO : ScriptableObject
{
    [Header("Character")]
    [SerializeField] private string _name = "-";
    [SerializeField, PreviewTexture(64)] private Sprite _sprite = null;
    [SerializeField] private AnimatorOverrideController _animatorController = null;

    [Header("Combat")]
    [SerializeField] private Equipment _equipment = null;
    [Space]
    [SerializeField, Range(0f, 100f)] private int _statsBaseHealth = 100;
    [SerializeField, Range(1f, 10f)] private int _statsBaseDamage = 1;
    [SerializeField, Range(0f, 10f)] private int _statsBaseDefense = 0;

    // Properties
    public string Name { get { return _name; } }
    public Sprite Sprite { get { return _sprite; } }
    public AnimatorOverrideController AnimatorController { get { return _animatorController; } }
    public Equipment Equipment { get { return _equipment; } }
    public int StatsHealthMax { get { return _statsBaseHealth; } }
    public int StatsBaseDamage { get { return _statsBaseDamage; } }
    public int StatsBaseDefense { get { return _statsBaseDefense; } }

}