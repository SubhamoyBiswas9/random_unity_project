using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int score { get; private set; }
    public int comboCount { get; private set; } = 0;

    MatchHandler matchHandler;

    public event System.Action<int,int> OnScoreChanged;

    public void Init(MatchHandler matchHandler)
    {
        this.matchHandler = matchHandler;
        matchHandler.OnPairEvaluated += OnPairEvaluated;

        score = 0;
        comboCount = 0;
    }

    void OnPairEvaluated(bool isMatch)
    {
        if (isMatch)
        {
            comboCount++;

            int basePoints = 10;
            int comboBonus = comboCount > 1 ? comboCount * 5 : 0;

            score += basePoints + comboBonus;

            Debug.Log($"Match! Combo: {comboCount}, Score: {score}");
        }
        else
        {
            comboCount = 0;
        }

        OnScoreChanged?.Invoke(score, comboCount);
    }

    public void SetScore(int score)
    {
        this.score = score;
    }

    void OnDisable()
    {
        if (matchHandler != null)
            matchHandler.OnPairEvaluated -= OnPairEvaluated;
    }
}
