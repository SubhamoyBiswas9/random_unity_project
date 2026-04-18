using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int maxMoves = 20;

    int totalPairs;
    int matchedPairs;
    int moves;

    MatchHandler matchHandler;

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

        if (isMatch)
        {
            matchedPairs++;

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
        SaveSystem.Clear();
    }

    void OnLevelFail()
    {
        Debug.Log("LEVEL FAIL");
    }

    private void OnDisable()
    {
        if (matchHandler != null)
            matchHandler.OnPairEvaluated -= OnPairEvaluated;
    }
}