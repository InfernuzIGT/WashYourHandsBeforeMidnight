using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Events;

public class GameData : MonoSingleton<GameData>
{
	public CombatConfig combatConfig;
	public TextConfig textConfig;

	public void PlayAction()
	{
		SceneManager.LoadScene("Combat");

	}
	public void OptionsAction()
	{
		
	}
	public void ExitAction()
	{
		Application.Quit();
	}
}