using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class DialogManager : MonoBehaviour
{

[Header ("GameObjects")]
    public GameObject textUI;
    public GameObject continueBtn;
    [Space]
    public TextMeshProUGUI textDisplay;
    public float typingSpeed;
    public Dialog dialog;
    public bool isPass = false;
    public Tween txtAnimation;
    private int _dialogIndex;
    private string _currentSentence;
    public bool isTriggerArea = false;
    public bool isEndConversation = false;

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
        txtAnimation = textDisplay.DOText(_currentSentence, typingSpeed);
        _dialogIndex++;

       if (_dialogIndex == dialog.sentences.Length)
       {
            isEndConversation = true;
            _dialogIndex = 0;
       }
    }

    public void CompleteText()
    {
        txtAnimation.Kill();
        textDisplay.text = _currentSentence;
        isPass = false;
    }

}