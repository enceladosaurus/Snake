using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] ScoreModel scoreModel = null;
    [SerializeField] private SevenSegment[] digits = null;

    private void Awake()
    {
        Debug.Assert(scoreModel != null, "Score model must be assigned.");
        scoreModel.ScoreChange.AddListener(DisplayScore);
    }

    private void DisplayScore() 
    {
        bool firstDigit = true;
        var score = scoreModel.Score;

        foreach (SevenSegment sevenSegment in digits)
        {
            uint digit = score % 10;
            if (!firstDigit && score == 0)
            {
                sevenSegment.DisplayDigit(null);
            }
            else
            {
                sevenSegment.DisplayDigit(digit);
            }
            score /= 10;
            firstDigit = false;
        }
    }
}
