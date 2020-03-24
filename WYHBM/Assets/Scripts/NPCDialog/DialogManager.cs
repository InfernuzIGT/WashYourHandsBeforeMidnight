using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Text;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public GameObject textUI;
    public GameObject continueBtn;
    public Dialog dialog;
    public float typingSpeed;
    
    void Start()
    {
        // string.Join(".", dialog.sentences);
    }

    public void SetText()
    {
        // textDisplay.text = dialog.sentences;
        textUI.SetActive(true);
        textDisplay.DOFade(1, 5);
    }


}
