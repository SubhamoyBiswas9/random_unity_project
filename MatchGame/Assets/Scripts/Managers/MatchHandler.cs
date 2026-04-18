using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    private Queue<CardController> flippedQueue = new();    
    private List<CardController> allCards = new();

    private bool isProcessing;

    private List<CardDataSO> cardPool;

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
            CardController a = flippedQueue.Dequeue();
            CardController b = flippedQueue.Dequeue();

            yield return new WaitForSeconds(0.4f);

            if (a.Data == b.Data)
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
        }

        isProcessing = false;
    }

    public void SaveProgress()
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