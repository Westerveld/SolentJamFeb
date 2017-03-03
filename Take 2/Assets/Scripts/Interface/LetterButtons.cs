using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LetterButtons : MonoBehaviour {

    [SerializeField]
    private Text letter;

    [SerializeField]
    private int currentLetter;

    [SerializeField]
    private int minLetter;
    [SerializeField]
    private int maxLetter;

    private char letterChar;

    void Start()
    {
        letterChar = (char)currentLetter;
        letter.text = letterChar.ToString();
    }

    public void PlusLetter()
    {
        if(currentLetter++ >= maxLetter)
        {
            currentLetter = minLetter;
        }
        letterChar = (char)currentLetter;
        letter.text = letterChar.ToString();

    }

    public void MinusLetter()
    {
        if(currentLetter-- <= minLetter)
        {
            currentLetter = maxLetter;
        }
        letterChar = (char)currentLetter;
        letter.text = letterChar.ToString();

    }

}
