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
	[Header("Session Data")]
	public int sessionIndex;
	public SessionData sessionData;

	[Header("Input System")]
	public DeviceSO[] deviceData;

	private List<AsyncOperation> _listScenes;

	private GlobalController _globalController;
	private LocalizationUtility _localizationUtility;

	// Scene Managment
	private bool _load;
	private SceneSO _sceneData;

	// Events
	private UpdateLanguageEvent _updateLanguageEvent;
	private EnableMovementEvent _enableMovementEvent;
	private ChangePositionEvent _changePositionEvent;
	private CustomFadeEvent _customFadeEvent;

	private int _indexQuality;

	protected override void Awake()
	{
		base.Awake();

		_globalController = GameObject.FindObjectOfType<GlobalController>();

		// Load();
	}

	private void Start()
	{
		_localizationUtility = GetComponent<LocalizationUtility>();

		_listScenes = new List<AsyncOperation>();

		_updateLanguageEvent = new UpdateLanguageEvent();
		_enableMovementEvent = new EnableMovementEvent();
		_changePositionEvent = new ChangePositionEvent();

		_customFadeEvent = new CustomFadeEvent();
		_customFadeEvent.callbackFadeIn = ChangeScene;

		_indexQuality = QualitySettings.GetQualityLevel();
	}

	private void OnEnable()
	{
		EventController.AddListener<ChangeSceneEvent>(OnChangeScene);
	}

	private void OnDisable()
	{
		EventController.RemoveListener<ChangeSceneEvent>(OnChangeScene);
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

	public void SelectNextQuality(bool isNext)
	{
		if (isNext)
		{
			_indexQuality = _indexQuality < QualitySettings.names.Length - 1 ? _indexQuality + 1 : 0;
		}
		else
		{
			_indexQuality = _indexQuality > 0 ? _indexQuality - 1 : QualitySettings.names.Length - 1;
		}

		QualitySettings.SetQualityLevel(_indexQuality, true);

		// TODO Mariano: COMPLETE

		// Debug.Log($"Current Quality: {QualitySettings.names[_indexQuality]}");

		// QualitySettings.vSyncCount = 0;

		// FullScreenMode fullScreenMode = FullScreenMode.FullScreenWindow;
		// Screen.fullScreenMode = fullScreenMode;

		// Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, fullScreenMode);
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

	#region Scene Managment

	private void OnChangeScene(ChangeSceneEvent evt)
	{
		_load = evt.load;
		_sceneData = evt.sceneData;
		_changePositionEvent.newPosition = evt.newPlayerPosition;
		_customFadeEvent.instant = evt.instantFade;

		_customFadeEvent.fadeIn = true;
		EventController.TriggerEvent(_customFadeEvent);

		_enableMovementEvent.canMove = false;
		EventController.TriggerEvent(_enableMovementEvent);
	}

	private void ChangeScene()
	{
		if (_load)
		{
			for (int i = 0; i < _sceneData.scenes.Length; i++)
			{
				_listScenes.Add(SceneManager.LoadSceneAsync(_sceneData.scenes[i], LoadSceneMode.Additive));
			}
		}
		else
		{
			for (int i = 0; i < _sceneData.scenes.Length; i++)
			{
				_listScenes.Add(SceneManager.UnloadSceneAsync(_sceneData.scenes[i]));
			}
		}

		if (_listScenes.Count == 0)
		{
			Debug.LogError($"<color=red><b>[ERROR]</b></color> No scenes found in {_sceneData.name}");
		}
		else
		{
			EventController.TriggerEvent(_changePositionEvent);

			StartCoroutine(LoadingProgress());
		}
	}

	private IEnumerator LoadingProgress()
	{
		float currentProgress = 0;
		float totalProgress = 0;

		for (int i = 0; i < _listScenes.Count; i++)
		{
			while (!_listScenes[i].isDone)
			{
				totalProgress += _listScenes[i].progress;
				currentProgress = totalProgress / _listScenes.Count;
				yield return null;
			}
		}

		_listScenes.Clear();

		yield return new WaitForSeconds(.5f);

		_customFadeEvent.fadeIn = false;
		EventController.TriggerEvent(_customFadeEvent);

		_enableMovementEvent.canMove = true;
		EventController.TriggerEvent(_enableMovementEvent);
	}

	#endregion

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

		string fileName = string.Format("data_{0}", sessionIndex);

		string data = PlayerPrefs.GetString(fileName, "");
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
		
		sessionData.sessionIndex = sessionIndex;

		string fileName = string.Format("data_{0}", sessionIndex);

		try
		{
			string result = DESEncryption.Encrypt(JsonUtility.ToJson(sessionData));
			PlayerPrefs.SetString(fileName, result);
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
	public int sessionIndex;

	public SessionSettings settings;

	public PlayerSO playerData;
	public Transform newSpawnPoint;

	public List<Quest> listQuest;
	public List<string> listIds;

	public SessionData()
	{
		sessionIndex = 0;

		listQuest = new List<Quest>();
		listIds = new List<string>();
	}
}

[Serializable]
public struct SessionSettings
{
	public int qualitySettings;
	// TODO Mariano: Complete
}