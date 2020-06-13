using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoSingleton<GameData>
{
	[Header("Config")]
	public WorldConfig worldConfig;
	public CombatConfig combatConfig;
	public TextConfig textConfig;

	[Header("Persistence")]
	public ItemSO persistenceItem;
	public QuestSO persistenceQuest;
	public Transform newSpawnPos;

	#region Load Scene

	public void LoadScene(SCENE_INDEX sceneIndex)
	{
		StartCoroutine(LoadYourAsyncScene(sceneIndex));
	}

	private IEnumerator LoadYourAsyncScene(SCENE_INDEX index)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int) index);

		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}

	#endregion

	#region Persistence

	private static SessionData _data;
	public static SessionData Data
	{
		get
		{
			if (_data == null) LoadData();
			return _data;
		}
	}

	private static bool LoadData()
	{
		bool valid = false;

		string data = PlayerPrefs.GetString("data", "");
		if (data != "")
		{
			bool success = DESEncryption.TryDecrypt(data, out string original);
			if (success)
			{
				_data = JsonUtility.FromJson<SessionData>(original);
				valid = true;
			}
			else
			{
				_data = new SessionData();
			}
		}
		else
		{
			_data = new SessionData();
		}

		return valid;
	}

	public static bool SaveData()
	{
		bool valid = false;

		try
		{
			string result = DESEncryption.Encrypt(JsonUtility.ToJson(_data));
			PlayerPrefs.SetString("data", result);
			PlayerPrefs.Save();
			valid = true;
		}
		catch (Exception ex)
		{
			Debug.LogError($"<color=red><b>[ERROR]</b></color> Save Data: {ex}");
		}

		return valid;
	}

	public static bool DeleteAllData()
	{
		bool valid = false;

		try
		{
			PlayerPrefs.DeleteAll();
			valid = true;
		}
		catch (Exception ex)
		{
			Debug.LogError($"<color=red><b>[ERROR]</b></color> Save Data: {ex}");
		}

		return valid;
	}

	#endregion

}

[Serializable]
public class SessionData
{
	public List<ItemSO> items = new List<ItemSO>();
	public List<QuestSO> listQuest = new List<QuestSO>();
	public List<int> listProgress = new List<int>();
	public bool isDataLoaded;
	public Transform newSpawnPoint;

	public SessionData()
	{
		// items.Add(GameData.Instance.persistenceItem);
		// listQuest.Add(GameData.Instance.persistenceQuest);
		// listProgress.Add(0);

		// isDataLoaded = true;

		// guardar variable de data cargada 
		// crea actualiza el spawn point a la posicion guardada, moviendo el spawnpoint a esa pos

		// GameData.Instance.newSpawnPos = newSpawnPoint;
		// newSpawnPoint = GameManager.Instance.globalController.player.transform;

		// newSpawnPoint.TransformPoint(
		// 	GameManager.Instance.globalController.player.transform.position.x,
		// 	GameManager.Instance.globalController.player.transform.position.y,
		// 	GameManager.Instance.globalController.player.transform.position.z);

	}
}