using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayScreen : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text movesText;
    [SerializeField] TMP_Text matchesText;
    [SerializeField] TMP_Text comboText;

    [SerializeField] GameObject levelEndScreen;
    [SerializeField] TMP_Text levelEndText;

    ScoreSystem scoreSystem;
    LevelManager levelManager;

    public void Init(ScoreSystem scoreSystem, LevelManager levelManager)
    {
        this.scoreSystem = scoreSystem;
        this.levelManager = levelManager;

        scoreSystem.OnScoreChanged += UpdateScore;
        levelManager.OnMovesChanged += UpdateMoves;
        levelManager.OnMatchProgressChanged += UpdateMatches;
        levelManager.OnGameOver += LevelManager_OnGameOver;

        UpdateUI();
    }

    private void LevelManager_OnGameOver(bool hasWon)
    {
        levelEndScreen.SetActive(true);

        if (hasWon)
            levelEndText.text = "You Win!";
        else
            levelEndText.text = "You Lose!";
    }

    void UpdateUI()
    {
        UpdateScore(scoreSystem.score,scoreSystem.comboCount);
        UpdateMoves(levelManager.moves);
        UpdateMatches(levelManager.matchedPairs, levelManager.totalPairs);
    }

    void UpdateScore(int score, int combo)
    {
        scoreText.text = $"Score: {score}";

        comboText.text = $"Combo: {combo}";
    }

    void UpdateMoves(int moves)
    {
        movesText.text = $"Moves: {moves}/{levelManager.maxMoves}";
    }

    void UpdateMatches(int matched, int total)
    {
        matchesText.text = $"Matches: {matched}/{total}";
    }

    void OnDisable()
    {
        if (scoreSystem != null)
            scoreSystem.OnScoreChanged -= UpdateScore;

        if (levelManager)
        {
            levelManager.OnMovesChanged -= UpdateMoves;
            levelManager.OnMatchProgressChanged -= UpdateMatches;
        }
        
    }
}
