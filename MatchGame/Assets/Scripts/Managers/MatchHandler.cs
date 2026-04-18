using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    Queue<CardController> flippedQueue = new();    
    List<CardController> allCards = new();

    bool isProcessing;

    List<CardDataSO> cardPool;

    public System.Action<bool> OnPairEvaluated;

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

                SaveProgress();
            }
            else
            {
                a.Flip(false);
                b.Flip(false);
            }

            OnPairEvaluated?.Invoke(isMatch);
        }

        isProcessing = false;
    }

    void SaveProgress()
    {
        SaveData data = new SaveData();

        data.cardIndices = new List<int>();
        data.matched = new List<bool>();

        foreach (var card in allCards)
        {
            int index = cardPool.IndexOf(card.Data);

            data.cardIndices.Add(index);
            data.matched.Add(card.IsMatched);
        }

        SaveSystem.Save(data);
    }
}