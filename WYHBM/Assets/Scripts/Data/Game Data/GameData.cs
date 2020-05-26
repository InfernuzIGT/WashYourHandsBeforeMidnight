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

	#region Load Scene

	public void LoadScene(SCENE_INDEX sceneIndex)
	{
		StartCoroutine(LoadYourAsyncScene(sceneIndex));
	}

	private IEnumerator LoadYourAsyncScene(SCENE_INDEX index)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)index);

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
			if (_data == null)LoadData();
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
	public Dictionary<int, QuestSO> dictionaryQuest = new Dictionary<int, QuestSO>();
	public Dictionary<int, int> dictionaryProgress = new Dictionary<int, int>();

	public SessionData()
	{
		items.Add(GameData.Instance.persistenceItem);
		dictionaryQuest.Add(GameData.Instance.persistenceQuest.GetInstanceID(), GameData.Instance.persistenceQuest);
		dictionaryProgress.Add(GameData.Instance.persistenceQuest.GetInstanceID(), 0);
	}
}