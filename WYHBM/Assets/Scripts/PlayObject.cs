using UnityEngine;
using UnityEngine.UI;

public class PlayObject : MonoBehaviour
{
    public KeyCode actionKeyPrimary;
    public KeyCode actionKeySecondary;

    private Button _actionBtn;

    private void Start()
    {
        _actionBtn = GetComponent<Button>();
        _actionBtn.onClick.AddListener(() => CombatManager.Instance.actionController.Play());
    }

    private void Update()
    {
        if (Input.GetKeyDown(actionKeyPrimary) || Input.GetKeyDown(actionKeySecondary))
        {
            _actionBtn.onClick.Invoke();
        }
    }
}