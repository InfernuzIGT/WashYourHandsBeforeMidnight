﻿using Events;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private SessionSettings _sessionSettings = null;
    [Space]
    [SerializeField] private LocalizedString _localizedOn;
    [SerializeField] private LocalizedString _localizedOff;
    [SerializeField] private LocalizedString[] _localizedQuality;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private GameObject _firstSelectOptions = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonLanguage = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonResolution = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonQuality = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonFullscreen = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonVsync = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonMasterVolume = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonSoundEffects = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonMusic = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonVibration = null;
    [SerializeField, ConditionalHide] private ButtonUI _buttonApply = null;
    [SerializeField, ConditionalHide] private ButtonUI _buttonBack = null;

    private int _indexLanguage;
    private int _indexResolution;
    private int _indexQuality;
    private int _indexMasterVolume = 10;
    private int _indexSoundEffects = 10;
    private int _indexMusic = 10;

    public GameObject FirstSelectOptions { get { return _firstSelectOptions; } }

    private void OnEnable()
    {
        EventController.AddListener<UpdateLanguageEvent>(OnUpdateLanguage);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<UpdateLanguageEvent>(OnUpdateLanguage);
    }

    public void AddListeners(UnityAction actionApply, UnityAction actionBack)
    {
        _buttonLanguage.AddListenerHorizontal(() => OptionsLanguage(true), () => OptionsLanguage(false));
        _buttonResolution.AddListenerHorizontal(() => OptionsResolution(true), () => OptionsResolution(false));
        _buttonQuality.AddListenerHorizontal(() => OptionsQuality(true), () => OptionsQuality(false));
        _buttonFullscreen.AddListenerHorizontal(() => OptionsFullscreen(true), () => OptionsFullscreen(false));
        _buttonVsync.AddListenerHorizontal(() => OptionsVsync(true), () => OptionsVsync(false));
        _buttonMasterVolume.AddListenerHorizontal(() => OptionsMasterVolume(true), () => OptionsMasterVolume(false));
        _buttonSoundEffects.AddListenerHorizontal(() => OptionsSoundEffects(true), () => OptionsSoundEffects(false));
        _buttonMusic.AddListenerHorizontal(() => OptionsMusic(true), () => OptionsMusic(false));
        _buttonVibration.AddListenerHorizontal(() => OptionsVibration(true), () => OptionsVibration(false));
        _buttonApply.AddListener(actionApply);
        _buttonBack.AddListener(actionBack);
    }

    public void LoadSettings()
    {
        _indexResolution = System.Array.IndexOf(Screen.resolutions, Screen.currentResolution);
        _buttonResolution.UpdateUI(Screen.currentResolution.ToString(), _indexResolution, Screen.resolutions.Length - 1);

        _indexQuality = QualitySettings.GetQualityLevel();
        _buttonQuality.UpdateUI(_localizedQuality[_indexQuality], _indexQuality, QualitySettings.names.Length - 1);

        _buttonFullscreen.UpdateUI(Screen.fullScreen ? _localizedOn : _localizedOff, !Screen.fullScreen);
        _buttonVsync.UpdateUI(QualitySettings.vSyncCount == 0 ? _localizedOff : _localizedOn, QualitySettings.vSyncCount == 0);

        _buttonMasterVolume.UpdateUI(_indexMasterVolume.ToString(), _indexMasterVolume, 10);
        _buttonSoundEffects.UpdateUI(_indexSoundEffects.ToString(), _indexSoundEffects, 10);
        _buttonMusic.UpdateUI(_indexMusic.ToString(), _indexMusic, 10);

        OptionsVibration(true);

        _buttonLanguage.UpdateUI(GameData.Instance.GetCurrentLanguage());
    }

    private void OptionsLanguage(bool isLeft)
    {
        GameData.Instance.SelectNextLanguage(!isLeft);
    }

    private void OptionsResolution(bool isLeft)
    {
        _indexResolution = Utils.GetNextIndex(!isLeft, _indexResolution, Screen.resolutions.Length - 1, false);
        Screen.SetResolution(Screen.resolutions[_indexResolution].width, Screen.resolutions[_indexResolution].height, Screen.fullScreen);

        _buttonResolution.UpdateUI(Screen.resolutions[_indexResolution].ToString(), _indexResolution, Screen.resolutions.Length - 1);
    }

    private void OptionsQuality(bool isLeft)
    {
        _indexQuality = Utils.GetNextIndex(!isLeft, _indexQuality, QualitySettings.names.Length - 1, false);
        QualitySettings.SetQualityLevel(_indexQuality, true);

        _buttonQuality.UpdateUI(_localizedQuality[_indexQuality], _indexQuality, QualitySettings.names.Length - 1);
    }

    private void OptionsFullscreen(bool isLeft)
    {
        Screen.fullScreen = !isLeft;

        _buttonFullscreen.UpdateUI(isLeft ? _localizedOff : _localizedOn, isLeft);
    }

    private void OptionsVsync(bool isLeft)
    {
        QualitySettings.vSyncCount = isLeft ? 0 : 1;

        _buttonVsync.UpdateUI(isLeft ? _localizedOff : _localizedOn, isLeft);
    }

    private void OptionsMasterVolume(bool isLeft)
    {
        _indexMasterVolume = Utils.GetNextIndex(!isLeft, _indexMasterVolume, 10, false);
        VolumeMaster(_indexMasterVolume);

        _buttonMasterVolume.UpdateUI(_indexMasterVolume.ToString(), _indexMasterVolume, 10);
    }

    private void OptionsSoundEffects(bool isLeft)
    {
        _indexSoundEffects = Utils.GetNextIndex(!isLeft, _indexSoundEffects, 10, false);
        VolumeSound(_indexSoundEffects);

        _buttonSoundEffects.UpdateUI(_indexSoundEffects.ToString(), _indexSoundEffects, 10);
    }

    private void OptionsMusic(bool isLeft)
    {
        _indexMusic = Utils.GetNextIndex(!isLeft, _indexMusic, 10, false);
        VolumeMusic(_indexMusic);

        _buttonMusic.UpdateUI(_indexMusic.ToString(), _indexMusic, 10);
    }

    private void OptionsVibration(bool isLeft)
    {
        if (isLeft)
        {
            GameData.Instance.StopRumble(true);
        }
        else
        {
            GameData.Instance.StopRumble(false);
            GameData.Instance.PlayRumble(RUMBLE_TYPE.Options);
        }

        _buttonVibration.UpdateUI(isLeft ? _localizedOff : _localizedOn, isLeft);
    }

    private void OnUpdateLanguage(UpdateLanguageEvent evt)
    {
        string[] splitLanguage = evt.locale.name.Split('(');

        _buttonLanguage.UpdateUI(splitLanguage[0]);
    }

    #region FMOD

    private void VolumeMaster(float vol)
    {
        RuntimeManager.StudioSystem.setParameterByName(FMODParameters.MasterSlider, vol);
    }

    private void VolumeMusic(float vol)
    {
        RuntimeManager.StudioSystem.setParameterByName(FMODParameters.MusicSlider, vol);
    }

    private void VolumeSound(float vol)
    {
        RuntimeManager.StudioSystem.setParameterByName(FMODParameters.SoundsSlider, vol);
    }

    #endregion

}