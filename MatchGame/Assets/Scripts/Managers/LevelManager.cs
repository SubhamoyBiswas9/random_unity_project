using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [field: SerializeField] public int maxMoves { get; private set; } = 20;

    public int totalPairs { get; private set; }
    public int matchedPairs { get; private set; }
    public int moves { get; private set; }

    MatchHandler matchHandler;

    public event Action<int> OnMovesChanged;
    public event Action<int, int> OnMatchProgressChanged;

    public event Action<bool> OnGameOver;

    public void Initialize(MatchHandler matchHandler, int totalCards)
    {
        this.matchHandler = matchHandler;

        totalPairs = totalCards / 2;
        matchedPairs = 0;
        moves = 0;

        matchHandler.OnPairEvaluated += OnPairEvaluated;
    }

    void OnPairEvaluated(bool isMatch)
    {
        moves++;

        OnMovesChanged?.Invoke(moves);

        if (isMatch)
        {
            matchedPairs++;
            OnMatchProgressChanged?.Invoke(matchedPairs, totalPairs);

            if (matchedPairs >= totalPairs)
            {
                OnLevelComplete();
                return;
            }
        }

        if (moves >= maxMoves && matchedPairs < totalPairs)
        {
            OnLevelFail();
        }
    }

    void OnLevelComplete()
    {
        Debug.Log("LEVEL COMPLETE");
        AudioManager.Instance.PlayGameWin();
        OnGameOver?.Invoke(true);
        SaveSystem.Clear();
    }

    void OnLevelFail()
    {
        Debug.Log("LEVEL FAIL");
        AudioManager.Instance.PlayGameLose();
        OnGameOver?.Invoke(false);
        SaveSystem.Clear();
    }

    private void OnDisable()
    {
        if (matchHandler != null)
            matchHandler.OnPairEvaluated -= OnPairEvaluated;
    }
}