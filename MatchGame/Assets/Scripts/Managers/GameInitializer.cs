using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] MainMenuScreen mainMenuScreen;
    [SerializeField] GridSpawner gridSpawner;
    [SerializeField] MatchHandler matchHandler;
    [SerializeField] LevelManager levelManager;
    [SerializeField] ScoreSystem scoreSystem;
    [SerializeField] GameplayScreen gameplayScreen;
    [SerializeField] InputManager inputManager;

    private void OnEnable()
    {
        mainMenuScreen.OnPlayBtnClicked += MainMenuScreen_OnPlayBtnClicked;
        matchHandler.OnPairEvaluated += MatchHandler_OnPairEvaluated;
    }

    private void OnDisable()
    {
        matchHandler.OnPairEvaluated -= MatchHandler_OnPairEvaluated;
        mainMenuScreen.OnPlayBtnClicked -= MainMenuScreen_OnPlayBtnClicked;
    }

    public void StartGame()
    {
        SaveData save = SaveSystem.Load();

        matchHandler.ClearAllCards();
        gridSpawner.Spawn(matchHandler, save);
        matchHandler.Initialize(gridSpawner.cardPool);

        var gridConfig = gridSpawner.config;
        int totalCards = gridConfig.rows * gridConfig.cols;
        levelManager.Initialize(matchHandler, totalCards);

        scoreSystem.Init(matchHandler);

        if (save != null)
        {
            scoreSystem.SetScore(save.score);
            levelManager.SetMatchedPairs(save.matchedPairs);
            levelManager.SetMoves(save.moves);
        }
        else
        {
            StartCoroutine(PreviewCards(2f));
        }

        gameplayScreen.Init(this, scoreSystem, levelManager);
    }

    IEnumerator PreviewCards(float previewTime)
    {
        // disable input
        inputManager.SetInputEnabled(false);

        // show all cards
        foreach (var card in matchHandler.allCards)
        {
            if (!card.IsMatched)
                card.Card.ShowFrontInstant();
        }

        yield return new WaitForSeconds(previewTime);

        // flip back
        foreach (var card in matchHandler.allCards)
        {
            if (!card.IsMatched)
                card.Card.Flip(false); // animated flip
        }

        // small delay to let animation finish
        yield return new WaitForSeconds(0.3f);

        // enable input
        inputManager.SetInputEnabled(true);
    }

    void SaveProgress()
    {
        SaveData data = new SaveData();

        data.cardIndices = new List<int>();
        data.matched = new List<bool>();

        foreach (var card in matchHandler.allCards)
        {
            int index = gridSpawner.cardPool.IndexOf(card.Data);

            data.cardIndices.Add(index);
            data.matched.Add(card.IsMatched);
        }

        data.score = scoreSystem.score;
        data.matchedPairs = levelManager.matchedPairs;
        data.moves = levelManager.moves;

        SaveSystem.Save(data);
    }

    private void MatchHandler_OnPairEvaluated(bool isMatch)
    {
        if(isMatch)
            SaveProgress();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause && levelManager.moves > 0 && !levelManager.gameFinished)
            SaveProgress();
    }

    private void OnApplicationQuit()
    {
        if(levelManager.moves > 0 && !levelManager.gameFinished)
        SaveProgress();
    }

    private void MainMenuScreen_OnPlayBtnClicked()
    {
        StartGame();
    }
}