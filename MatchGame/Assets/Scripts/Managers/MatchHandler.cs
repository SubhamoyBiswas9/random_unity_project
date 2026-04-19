using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    Queue<CardController> flippedQueue = new();    
    public List<CardController> allCards { get; private set; } = new();

    bool isProcessing;

    List<CardDataSO> cardPool;

    public event System.Action<bool> OnPairEvaluated;

    public void Initialize(List<CardDataSO> pool)
    {
        cardPool = pool;
    }

    public void RegisterCard(CardController cardController)
    {
        allCards.Add(cardController);
        cardController.Card.OnClicked += OnCardClicked;
    }

    void OnCardClicked(Card card)
    {
        CardController cardController = allCards.Find(c => c.Card == card);

        if (cardController.IsMatched || cardController.Card.IsFront()) return;

        cardController.Flip(true);

        AudioManager.Instance.PlayFlip();

        flippedQueue.Enqueue(cardController);

        if (!isProcessing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    IEnumerator ProcessQueue()
    {
        isProcessing = true;

        while (flippedQueue.Count >= 2)
        {
            var a = flippedQueue.Dequeue();
            var b = flippedQueue.Dequeue();

            yield return new WaitForSeconds(0.4f);

            bool isMatch = (a.Data == b.Data);

            if (isMatch)
            {
                a.SetMatched();
                b.SetMatched();

                a.Card.PlayMatchAnimation();
                b.Card.PlayMatchAnimation();

                //SaveProgress();

                AudioManager.Instance.PlayMatch();
            }
            else
            {
                a.Flip(false);
                b.Flip(false);

                AudioManager.Instance.PlayMismatch();
            }

            OnPairEvaluated?.Invoke(isMatch);
        }

        isProcessing = false;
    }

    public void ClearAllCards()
    {
        allCards.Clear();
    }
}