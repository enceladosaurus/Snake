using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Snake/Score Model")]
public class ScoreModel : ScriptableObject
{
    [SerializeField] private uint score;
    [SerializeField] private uint foodTickAmount = 25;
    [SerializeField] private float scoreTickInterval = 0.33f;
    [SerializeField] private uint scoreTickAmount = 1;
    public uint Score 
    { 
        get => score;
        set 
        {
            score = value;
            ScoreChange?.Invoke();
        }
    }
    public uint FoodTickAmount => foodTickAmount;
    public float ScoreTickInterval => scoreTickInterval;
    public uint ScoreTickAmount => scoreTickAmount; 

    public UnityEvent ScoreChange { get; set; } = new UnityEvent();

    private void OnEnable()
    {
        Score = 0;
    }
}
