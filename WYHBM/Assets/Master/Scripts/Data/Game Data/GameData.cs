using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum SESSION_OPTION
{
	Save = 0,
	Load = 1,
	Delete = 2,
}

[RequireComponent(typeof(LocalizationUtility))]
public class GameData : MonoSingleton<GameData>
{
	[Header("Developer")]
	[SerializeField] private bool _devPrintInputInfo = false;
	[SerializeField] private bool _devDDLegacyMode = false;

	[Header("Session Data")]
	public int sessionIndex;
	public SessionData sessionData;

	private List<AsyncOperation> _listScenes;

	private SpawnPoint _lastSpawnPoint;
	private GlobalController _globalController;
	private LocalizationUtility _localizationUtility;

	// Scene Managment
	private bool _load;
	private bool _isLoadAdditive;
	private SceneSO _sceneData;

	// Input System
	private Gamepad _gamepad;
	private Coroutine _coroutineRumble;
	private float _rumbleFrequency;

	// Events
	private EnableMovementEvent _enableMovementEvent;
	private ChangePositionEvent _changePositionEvent;
	private CustomFadeEvent _customFadeEvent;
	private SaveAnimationEvent _saveAnimationEvent;

	private int _indexQuality;

	// Properties
	public PlayerController Player { get { return _globalController.playerController; } }
	public PlayerSO PlayerData { get { return _globalController.PlayerData; } }

	public bool DevDDLegacyMode { get { return _devDDLegacyMode; } }

	protected override void Awake()
	{
#if UNITY_EDITOR

		if (_devPrintInputInfo)InputUtility.printInfo = true;

#endif

		base.Awake();

		GetSceneReferences();

		// Load();
	}

	private void Start()
	{
		_localizationUtility = GetComponent<LocalizationUtility>();

		_listScenes = new List<AsyncOperation>();

		_enableMovementEvent = new EnableMovementEvent();
		_changePositionEvent = new ChangePositionEvent();

		_customFadeEvent = new CustomFadeEvent();

		_saveAnimationEvent = new SaveAnimationEvent();

		_indexQuality = QualitySettings.GetQualityLevel();
	}

	private void GetSceneReferences()
	{
		_globalController = GameObject.FindObjectOfType<GlobalController>();

		_lastSpawnPoint = GameObject.FindObjectOfType<SpawnPoint>();

		if (_globalController != null && _lastSpawnPoint != null)
		{
			_globalController.Init(_lastSpawnPoint.transform.position);
		}
		// else
		// {
		// 	Debug.LogError($"<color=red><b>[ERROR]</b></color> Missing Scene Reference: GlobalController[{_globalController == null}] / SpawnPoint[{_lastSpawnPoint == null}]");
		// }
	}

	private void OnEnable()
	{
		EventController.AddListener<ChangeSceneEvent>(OnChangeScene);
		EventController.AddListener<DeviceChangeEvent>(OnDeviceChange);
	}

	private void OnDisable()
	{
		EventController.RemoveListener<ChangeSceneEvent>(OnChangeScene);
		EventController.RemoveListener<DeviceChangeEvent>(OnDeviceChange);
	}

	#region Settings

	public void SettingNextLanguage(bool isNext)
	{
		_localizationUtility.SelectNextLanguage(isNext);
	}

	public void SettingsNextQuality(bool isNext)
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

	#endregion

	#region Rumble

	private void OnDeviceChange(DeviceChangeEvent evt)
	{
		_gamepad = evt.gamepad;
	}

	public void Rumble(float frequency)
	{
		if (_gamepad == null)return;

		_rumbleFrequency = frequency;

		_coroutineRumble = StartCoroutine(ActivateRumble());
	}

	private IEnumerator ActivateRumble()
	{
		float currentDuration = 0;

		while (currentDuration < 1)
		{
			currentDuration += Time.deltaTime;
			_gamepad.SetMotorSpeeds(_rumbleFrequency, _rumbleFrequency);
			yield return null;
		}

		_gamepad.SetMotorSpeeds(0, 0);
	}

	#endregion

	#region Scene Managment

	private void OnChangeScene(ChangeSceneEvent evt)
	{
		_load = evt.load;
		_isLoadAdditive = evt.isLoadAdditive;
		_sceneData = evt.sceneData;
		_changePositionEvent.newPosition = evt.newPlayerPosition;

		_customFadeEvent.instant = evt.instantFade;
		_customFadeEvent.fadeIn = true;
		_customFadeEvent.callbackFadeIn = ChangeScene;
		EventController.TriggerEvent(_customFadeEvent);

		_enableMovementEvent.canMove = false;
		EventController.TriggerEvent(_enableMovementEvent);
	}

	private void ChangeScene()
	{
		if (_load)
		{
			if (_isLoadAdditive)
			{
				for (int i = 0; i < _sceneData.scenes.Length; i++)
				{
					_listScenes.Add(SceneManager.LoadSceneAsync(_sceneData.scenes[i], LoadSceneMode.Additive));
				}
			}
			else
			{
				_listScenes.Add(SceneManager.LoadSceneAsync(_sceneData.scenes[0]));

				for (int i = 1; i < _sceneData.scenes.Length; i++)
				{
					_listScenes.Add(SceneManager.LoadSceneAsync(_sceneData.scenes[i], LoadSceneMode.Additive));
				}
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

		// TODO Mariano: Fix
		GetSceneReferences();
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
		return _globalController.SessionData.listIds.Contains(id);
	}

	public void WriteID(string id)
	{
		if (!_globalController.SessionData.listIds.Contains(id))
		{
			_globalController.SessionData.listIds.Add(id);
			// sessionData.listIds.Add(id);
			Save();
		}
	}

	public bool CheckAndWriteID(string id)
	{
		bool containId = _globalController.SessionData.listIds.Contains(id);

		if (!containId)
		{
			_globalController.SessionData.listIds.Add(id);
			// sessionData.listIds.Add(id);
			Save();
		}

		return containId;
	}

	public bool CheckQuest(QuestSO questData)
	{
		for (int i = 0; i < _globalController.SessionData.listQuest.Count; i++)
		{
			if (_globalController.SessionData.listQuest[i].data == questData)
			{
				return _globalController.SessionData.listQuest[i].currentStep >= questData.steps;
			}
		}

		return false;
	}

	public bool HaveQuest(QuestSO questData)
	{
		for (int i = 0; i < _globalController.SessionData.listQuest.Count; i++)
		{
			if (_globalController.SessionData.listQuest[i].data == questData)
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
		EventController.TriggerEvent(_saveAnimationEvent);

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