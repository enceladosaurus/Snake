using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifetimeScoreUpdater : MonoBehaviour
{
    [SerializeField] ScoreModel scoreModel = null;
    private float elapsedTime = 0;
    public void Awake()
    {
        Debug.Assert(scoreModel != null, "Score updater needs a score model set.");
    }
    public void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime; 

        if (elapsedTime >= scoreModel.ScoreTickInterval)
        {
            scoreModel.Score += scoreModel.ScoreTickAmount;
            elapsedTime = 0;
        }
    }
}
