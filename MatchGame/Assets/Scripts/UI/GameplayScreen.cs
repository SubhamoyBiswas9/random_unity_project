using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayScreen : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text movesText;
    [SerializeField] TMP_Text matchesText;
    [SerializeField] TMP_Text comboText;

    [SerializeField] GameObject levelEndScreen;
    [SerializeField] TMP_Text levelEndText;

    [SerializeField] Button replayButton;
    [SerializeField] Button quitButton;

    ScoreSystem scoreSystem;
    LevelManager levelManager;
    GameInitializer gameInitializer;

    private void OnEnable()
    {
        replayButton.onClick.AddListener(OnClickReplay);
        quitButton.onClick.AddListener(()=> Application.Quit());
    }

    public void Init(GameInitializer gameInitializer, ScoreSystem scoreSystem, LevelManager levelManager)
    {
        this.gameInitializer = gameInitializer;
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

    void OnClickReplay()
    {
        SaveSystem.Clear();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        replayButton.onClick.RemoveListener(OnClickReplay);
        quitButton.onClick.RemoveAllListeners();
    }
}
