using DG.Tweening;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject textUI;
    public GameObject continueBtn;
    [Space]
    
    [Header("Dialog")]
    public Dialog dialog;
    public TextMeshProUGUI textDisplay;
    public float typingSpeed;
    public bool isPass = false;
    public bool isTriggerArea = false;
    public bool isEndConversation = false;
    
    private Tween _txtAnimation;
    private string _currentSentence;
    private int _dialogIndex;

    //when player enter in conversation he can't move or Ui desactivates?

    public void SetText()
    {
        isEndConversation = false;

        if (_dialogIndex < dialog.sentences.Length && isTriggerArea)
        {
            textDisplay.text = "";
            ExecuteText();
        }
    }

    public void ExecuteText()
    {
        isPass = true;
        _currentSentence = dialog.sentences[_dialogIndex];
        _txtAnimation = textDisplay.DOText(_currentSentence, typingSpeed);
        _dialogIndex++;

        if (_dialogIndex == dialog.sentences.Length)
        {
            isEndConversation = true;
            _dialogIndex = 0;
        }
    }

    public void CompleteText()
    {
        _txtAnimation.Kill();
        textDisplay.text = _currentSentence;
        isPass = false;
    }

}