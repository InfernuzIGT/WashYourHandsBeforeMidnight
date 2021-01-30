using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalizationUtility : MonoBehaviour
{
    [SerializeField, ReadOnly] private Locale _locale;
    [SerializeField, ReadOnly] private string _language = "";

    private AsyncOperationHandle m_InitializeOperation;
    private List<Locale> _listLocales;
    private int _index;
    private bool _isForced;

    public string Language { get { return _language; } }

    private UpdateLanguageEvent _updateLanguageEvent;

    private void Start()
    {
        _updateLanguageEvent = new UpdateLanguageEvent();

        m_InitializeOperation = LocalizationSettings.SelectedLocaleAsync;

        if (m_InitializeOperation.IsDone)
        {
            InitializeCompleted(m_InitializeOperation);
        }
        else
        {
            m_InitializeOperation.Completed += InitializeCompleted;
        }
    }

    private void InitializeCompleted(AsyncOperationHandle obj)
    {
        _listLocales = new List<Locale>();
        _listLocales.AddRange(LocalizationSettings.AvailableLocales.Locales);

        if (_isForced)
        {
            _isForced = false;
            _index = LocalizationSettings.AvailableLocales.Locales.IndexOf(_locale);
        }
        
        _index = _index < 0 ? 0 : _index;

        UpdateLocale(_listLocales[_index]);
    }

    private void UpdateLocale(Locale locale)
    {
        LocalizationSettings.SelectedLocale = locale;

        _language = locale.Identifier.Code;
        _locale = locale;

        _updateLanguageEvent.language = _language;
        _updateLanguageEvent.locale = locale;
        EventController.TriggerEvent(_updateLanguageEvent);
    }

    public void SelectNextLanguage(bool isNext)
    {
        _index = Utils.GetNextIndex(isNext, _index, _listLocales.Count - 1);

        UpdateLocale(_listLocales[_index]);
    }

    public void ForceSetLocale(Locale locale)
    {
        _isForced = true;
        _locale = locale;
    }

}