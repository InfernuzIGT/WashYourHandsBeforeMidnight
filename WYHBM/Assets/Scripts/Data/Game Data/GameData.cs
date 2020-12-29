using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SESSION_OPTION
{
	Save = 0,
	Load = 1,
	Delete = 2,
}

/// <summary>
/// Used by:
/// - GlobalController
/// - Interaction
/// - Interaction Cutscene
/// - NPCController
/// </summary>

[RequireComponent(typeof(LocalizationUtility))]
public class GameData : MonoSingleton<GameData>
{
	public SessionData sessionData;

	[Header("Config")]
	public WorldConfig worldConfig;
	public CombatConfig combatConfig;
	public TextConfig textConfig;

	[Header("Input System")]
	public DeviceSO[] deviceData;

	private GlobalController _globalController;
	private LocalizationUtility _localizationUtility;
	private UpdateLanguageEvent _updateLanguageEvent;

	protected override void Awake()
	{
		base.Awake();

		_globalController = GameObject.FindObjectOfType<GlobalController>();

		// Load();
	}

	private void Start()
	{
		_localizationUtility = GetComponent<LocalizationUtility>();

		_updateLanguageEvent = new UpdateLanguageEvent();
	}

	public void SelectNextLanguage()
	{
		_localizationUtility.SelectNextLanguage(true);
	}

	public void UpdateLanguage(string language)
	{
		_updateLanguageEvent.language = language;
		EventController.TriggerEvent(_updateLanguageEvent);
	}

	public Sprite GetInputIcon(DEVICE device, INPUT_ACTION action)
	{
		for (int i = 0; i < deviceData.Length; i++)
		{
			if (deviceData[i].type == device)
			{
				return deviceData[i].GetIcon(action);
			}
		}

		return null;
	}

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

	#region Persistence

	public bool SaveSession(SessionData currentSession)
	{
		sessionData = currentSession;
		return Save();
	}

	public SessionData LoadSession()
	{
		Load();
		return sessionData;
	}

	public bool CheckID(string id)
	{
		return _globalController.sessionData.listIds.Contains(id);
	}

	public void WriteID(string id)
	{
		if (!_globalController.sessionData.listIds.Contains(id))
		{
			_globalController.sessionData.listIds.Add(id);
			// sessionData.listIds.Add(id);
			Save();
		}
	}

	public bool CheckAndWriteID(string id)
	{
		bool containId = _globalController.sessionData.listIds.Contains(id);

		if (!containId)
		{
			_globalController.sessionData.listIds.Add(id);
			// sessionData.listIds.Add(id);
			Save();
		}

		return containId;
	}

	public bool CheckQuest(QuestSO questData)
	{
		for (int i = 0; i < _globalController.sessionData.listQuest.Count; i++)
		{
			if (_globalController.sessionData.listQuest[i].data == questData)
			{
				return _globalController.sessionData.listQuest[i].currentStep >= questData.steps;
			}
		}

		return false;
	}

	public bool HaveQuest(QuestSO questData)
	{
		for (int i = 0; i < _globalController.sessionData.listQuest.Count; i++)
		{
			if (_globalController.sessionData.listQuest[i].data == questData)
			{
				return true;
			}
		}

		return false;
	}

	public bool Load()
	{
		bool valid = false;

		string data = PlayerPrefs.GetString("data", "");
		if (data != "")
		{
			bool success = DESEncryption.TryDecrypt(data, out string original);
			if (success)
			{
				sessionData = JsonUtility.FromJson<SessionData>(original);
				valid = true;
			}
			// else
			// {
			// 	sessionData = new SessionData();
			// }
		}
		// else
		// {
		// 	sessionData = new SessionData();
		// }

		return valid;
	}

	public bool Save()
	{
		bool valid = false;

		try
		{
			string result = DESEncryption.Encrypt(JsonUtility.ToJson(sessionData));
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

	public bool DeleteAll()
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

		sessionData = null;

		return valid;
	}

	#endregion

}

[Serializable]
public class SessionData
{
	public PlayerSO playerData;
	public Transform newSpawnPoint;

	public List<Quest> listQuest;
	public List<string> listIds;

	public SessionData()
	{
		listQuest = new List<Quest>();
		listIds = new List<string>();
	}
}