using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{

    public TextMeshProUGUI textDisplay;
    [TextArea(10, 3)]
    public string[] sentences;
    public float typingSpeed;
    public GameObject continueBtn;
    // public Animator textDisplayAnim;
    public GameObject dialogUI;

    private int _index;

    void Update()
    {
        if (textDisplay.text == sentences[_index])
        {
            continueBtn.SetActive(true);
        }
    }

    public IEnumerator Type ()
    {
        foreach (char letter in sentences[_index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        // textDisplayAnim.SetTrigger("ChangeText");
        continueBtn.SetActive(false);

        if (_index < sentences.Length - 1)
        {
            _index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } 
        else
        {
            textDisplay.text = "";
            continueBtn.SetActive(false);
        }
    }

}
