using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalizationUtility : MonoBehaviour
{
    [SerializeField, ReadOnly] private string _language = "en";
    public string Language { get { return _language; } }
    [Space]
    [SerializeField] private UnityEvent OnUpdateLanguage = null;
    

    private AsyncOperationHandle m_InitializeOperation;
    private List<Locale> _listLocales;
    private int _index;

    private void Start()
    {
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

        _index = 0;

        UpdateLocale(_listLocales[_index]);
    }

    private void UpdateLocale(Locale locale)
    {
        LocalizationSettings.SelectedLocale = locale;

        // _language = locale.name;
        _language = locale.Identifier.Code;

        // Debug.Log($"Update Locale: {locale.name} [{locale.Identifier.Code}]");

        OnUpdateLanguage.Invoke();
    }

    public void SelectNextLanguage(bool isNext)
    {
        if (isNext)
        {
            _index = _index < _listLocales.Count - 1 ? _index + 1 : 0;
        }
        else
        {
            _index = _index > 0 ? _index - 1 : _listLocales.Count - 1;
        }

        UpdateLocale(_listLocales[_index]);
    }

}