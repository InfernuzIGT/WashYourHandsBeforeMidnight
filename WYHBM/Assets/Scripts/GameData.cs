﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoSingleton<GameData>
{
	public CombatConfig combatConfig;
	public TextConfig textConfig;
}