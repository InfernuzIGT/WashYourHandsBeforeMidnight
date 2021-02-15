using System;
using System.Collections;
using System.Collections.Generic;
using Chronos;
using Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

public enum SESSION_OPTION
{
	Save = 0,
	Load = 1,
	Delete = 2,
}

[RequireComponent(typeof(GlobalClock), typeof(Timekeeper))]
[RequireComponent(typeof(LocalizationUtility), typeof(InputSystemUtility))]
public class GameData : MonoSingleton<GameData>
{
	[Header("Developer")]
	[SerializeField] private bool _devPrintInputInfo = false;
	[SerializeField] private bool _devDontSave = false;
	[SerializeField] private bool _devLoadMainMenu = false;

	[Header("Configs")]
	[SerializeField] private PlayerConfig _playerConfig;
	[SerializeField] private DeviceConfig _deviceConfig;

	[Header("Persistence")]
	public int sessionIndex;
	public SessionData sessionData;
	public SessionSettings sessionSettings;

	[Header("References")]
#pragma warning disable 0414
	[SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
	[SerializeField, ConditionalHide] private LocalizationUtility _localizationUtility;
	[SerializeField, ConditionalHide] private InputSystemUtility _inputSystemUtility;
	[SerializeField, ConditionalHide] private GlobalClock _globalClock;

	private List<AsyncOperation> _listScenes;

	private GlobalController _globalController;
	private LevelManager _levelManager;
	private SpawnPoint _spawnPoint;

	// Scene Managment
	private bool _onlyTeleport;
	private bool _load;
	private bool _isLoadAdditive;
	private SceneSO _sceneData;
	private bool _homeUsed;

	// Input System
	private Gamepad _gamepad;
	private Coroutine _coroutineRumble;
	private RUMBLE_TYPE _rumbleType;
	private RumbleValues _currentRumbleValues;
	private float _rumbleDuration;
	private float _rumbleFrequency;
	private float _currentDuration;

	// Events
	private EnableMovementEvent _enableMovementEvent;
	private ChangePositionEvent _changePositionEvent;
	private CustomFadeEvent _customFadeEvent;
	private SaveAnimationEvent _saveAnimationEvent;
	private LoadAnimationEvent _loadAnimationEvent;

	private bool _devDDLegacyMode;

	private bool _changeSceneUseEvent;

	// Properties
	public PlayerSO PlayerData { get { return _globalController.PlayerData; } }
	public SpawnPoint SpawnPoint { get { return _spawnPoint; } }
	public LevelManager LevelManager { get { return _levelManager; } }

	public bool DevDDLegacyMode { get { return _devDDLegacyMode; } set { _devDDLegacyMode = value; } }
	public bool HomeUsed { get { return _homeUsed; } set { _homeUsed = value; } }

	protected override void Awake()
	{
		InitializeConfigs();

#if UNITY_EDITOR
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		if (_devPrintInputInfo)InputSystemAdapter.printInfo = true;
#else
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		DeleteAll();
#endif

		base.Awake();

	}

	private void InitializeConfigs()
	{
		_deviceConfig.UpdateDictionary();
		_playerConfig.UpdateActionDictionary();
	}

	private void Start()
	{
		_listScenes = new List<AsyncOperation>();

		_enableMovementEvent = new EnableMovementEvent();
		_changePositionEvent = new ChangePositionEvent();

		_customFadeEvent = new CustomFadeEvent();

		_loadAnimationEvent = new LoadAnimationEvent();
		_saveAnimationEvent = new SaveAnimationEvent();

#if UNITY_EDITOR
#else
		if (!_devLoadMainMenu)SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive); // Main Menu
#endif
		if (_devLoadMainMenu)SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive); // Main Menu

	}

	public List<Player> GetListPlayer()
	{
		return _globalController.GetListPlayer();
	}

	public void GetSceneReferences()
	{
		StartCoroutine(FindReferences());
	}

	private IEnumerator FindReferences()
	{
		_globalController = GameObject.FindObjectOfType<GlobalController>();

		while (_globalController == null)
		{
			yield return new WaitForSeconds(3);
			_globalController = GameObject.FindObjectOfType<GlobalController>();
		}

		_levelManager = GameObject.FindObjectOfType<LevelManager>();

		while (_levelManager == null)
		{
			yield return new WaitForSeconds(3);
			_levelManager = GameObject.FindObjectOfType<LevelManager>();
		}

		_spawnPoint = _levelManager.GetSpawnPoint(PlayerData.ID);

		_globalController.Init(_spawnPoint.transform.position);

		if (_loadAnimationEvent != null && _loadAnimationEvent.isLoading)
		{
			_loadAnimationEvent.isLoading = false;
			EventController.TriggerEvent(_loadAnimationEvent);
		}
	}

	private void OnEnable()
	{
		EventController.AddListener<ChangeSceneEvent>(OnChangeScene);
		EventController.AddListener<DeviceChangeEvent>(OnDeviceChange);
		EventController.AddListener<PauseEvent>(OnPause);
		EventController.AddListener<VibrationEvent>(OnVibration);
	}

	private void OnDisable()
	{
		EventController.RemoveListener<ChangeSceneEvent>(OnChangeScene);
		EventController.RemoveListener<DeviceChangeEvent>(OnDeviceChange);
		EventController.RemoveListener<PauseEvent>(OnPause);
		EventController.RemoveListener<VibrationEvent>(OnVibration);
	}

	private void OnVibration(VibrationEvent evt)
	{
		PlayRumble(evt.type);
	}

	public void DetectDevice(InputDevice inputDevice = null)
	{
		_inputSystemUtility.DetectDevice(inputDevice);
	}

	private void OnPause(PauseEvent evt)
	{
		_globalClock.localTimeScale = evt.isPaused ? 0 : 1;
	}

	public Interaction GetPlayerCurrentInteraction()
	{
		return _globalController.GetPlayerCurrentInteraction();
	}

	#region Localization

	public void SelectNextLanguage(bool isNext)
	{
		_localizationUtility.SelectNextLanguage(isNext);
	}

	public string GetCurrentLanguage()
	{
		return _localizationUtility.Language;
	}

	public void ForceLanguage(Locale locale)
	{
		_localizationUtility.ForceSetLocale(locale);
	}

	#endregion

	#region Rumble

	private void OnDeviceChange(DeviceChangeEvent evt)
	{
		_gamepad = evt.gamepad;
	}

	public void PlayRumble(RUMBLE_TYPE rumbleType)
	{
		if (_gamepad == null)return;

		_rumbleType = rumbleType;

		_coroutineRumble = StartCoroutine(ActivateRumble());
	}

	public void StopRumble(bool isStop)
	{
		if (_gamepad == null)return;

		if (isStop)
		{
			if (_coroutineRumble != null)
			{
				StopCoroutine(_coroutineRumble);
				_coroutineRumble = null;
			}

			_gamepad.ResetHaptics();
		}
		else
		{
			_gamepad.ResumeHaptics();
		}
	}

	private IEnumerator ActivateRumble()
	{
		_currentRumbleValues = _playerConfig.GetRumbleValues(_rumbleType);
		_rumbleDuration = _currentRumbleValues.duration;
		_rumbleFrequency = _currentRumbleValues.frequency;

		_currentDuration = 0;

		while (_currentDuration < _rumbleDuration)
		{
			_currentDuration += Time.deltaTime;
			_gamepad.SetMotorSpeeds(_rumbleFrequency, _rumbleFrequency);
			yield return null;
		}

		_gamepad.SetMotorSpeeds(0, 0);
	}

	#endregion

	#region Scene Managment

	private void OnChangeScene(ChangeSceneEvent evt)
	{
		_changeSceneUseEvent = evt.useEnableMovementEvent;

		_onlyTeleport = evt.onlyTeleport;
		_load = evt.load;
		_isLoadAdditive = evt.isLoadAdditive;
		_sceneData = evt.sceneData;
		_changePositionEvent.newPosition = evt.newPlayerPosition;

		_customFadeEvent.instant = evt.instantFade;
		_customFadeEvent.fadeIn = true;
		_customFadeEvent.callbackFadeIn = ChangeScene;
		EventController.TriggerEvent(_customFadeEvent);

		if (_changeSceneUseEvent)
		{
			_enableMovementEvent.canMove = false;
			EventController.TriggerEvent(_enableMovementEvent);
		}

	}

	private void ChangeScene()
	{
		if (_onlyTeleport || _sceneData == null)
		{
			EventController.TriggerEvent(_changePositionEvent);
			StartCoroutine(LoadingProgress());
			return;
		}

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
		_loadAnimationEvent.isLoading = true;
		EventController.TriggerEvent(_loadAnimationEvent);

		// float currentProgress = 0;
		// float totalProgress = 0;

		// for (int i = 0; i < _listScenes.Count; i++)
		// {
		// 	while (!_listScenes[i].isDone)
		// 	{
		// 		totalProgress += _listScenes[i].progress;
		// 		currentProgress = totalProgress / _listScenes.Count;
		// 		yield return null;
		// 	}
		// }

		_listScenes.Clear();

		if (_globalController == null)
		{
			yield return StartCoroutine(FindReferences());;
		}

		yield return new WaitForSeconds(.5f);

		_customFadeEvent.fadeIn = false;
		EventController.TriggerEvent(_customFadeEvent);

		if (_changeSceneUseEvent)
		{
			_enableMovementEvent.canMove = true;
			EventController.TriggerEvent(_enableMovementEvent);
		}

		// GetSceneReferences();
		// TODO Mariano: Search another checkpoint?
	}

	#endregion

	#region Persistence

	public bool SaveSession(SessionData currentSession)
	{
		sessionData = currentSession;
		return Save();
	}

	public SessionData LoadSessionData()
	{
		Load();
		return sessionData;
	}

	public SessionSettings LoadSessionSettings()
	{
		LoadSettings();
		return sessionSettings;
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
			// Save();
		}
	}

	public bool CheckAndWriteID(string id)
	{
		bool containId = _globalController.SessionData.listIds.Contains(id);

		if (!containId)
		{
			_globalController.SessionData.listIds.Add(id);
			// Save();
		}

		return containId;
	}

	public bool CheckQuestCurrentStep(QuestSO questData)
	{
		for (int i = 0; i < _globalController.SessionData.listQuest.Count; i++)
		{
			if (_globalController.SessionData.listQuest[i].data == questData)
			{
				return _globalController.SessionData.listQuest[i].currentStep == questData.steps;
			}
		}

		return false;
	}

	public bool CheckQuestRequiredStep(QuestSO questData, int requiredSteps)
	{
		for (int i = 0; i < _globalController.SessionData.listQuest.Count; i++)
		{
			if (_globalController.SessionData.listQuest[i].data == questData)
			{
				return _globalController.SessionData.listQuest[i].currentStep == requiredSteps;
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
		}

		return valid;
	}

	public bool Save()
	{
		if (_devDontSave)return true;

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

	public bool LoadSettings()
	{
		bool valid = false;

		string data = PlayerPrefs.GetString("settings", "");
		if (data != "")
		{
			bool success = DESEncryption.TryDecrypt(data, out string original);
			if (success)
			{
				sessionSettings = JsonUtility.FromJson<SessionSettings>(original);
				valid = true;
			}

		}

		return valid;
	}

	public bool SaveSettings(SessionSettings lastSessionSettings)
	{
		bool valid = false;

		sessionSettings = lastSessionSettings;

		try
		{
			string result = DESEncryption.Encrypt(JsonUtility.ToJson(sessionSettings));
			PlayerPrefs.SetString("settings", result);
			PlayerPrefs.Save();
			valid = true;
		}
		catch (Exception ex)
		{
			Debug.LogError($"<color=red><b>[ERROR]</b></color> Save Settings: {ex}");
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
public class SessionSettings
{
	public Locale language;
	public Resolution resolution;
	public int quality;
	public bool fullScreen;
	public int vSync;
	public int masterVolume;
	public int soundEffects;
	public int music;
	public bool vibration;
}