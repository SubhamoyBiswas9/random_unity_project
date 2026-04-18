[System.Serializable]
public class CardController
{
    public CardDataSO Data { get; private set; }
    public Card Card { get; private set; }

    public bool IsMatched { get; private set; }

    public CardController(CardDataSO data, Card card)
    {
        Data = data;
        Card = card;

        Card.Setup(data.frontSprite, data.baseSprite);
    }

    public void Flip(bool show)
    {
        Card.Flip(show);
    }

    public void SetMatched()
    {
        IsMatched = true;
    }
}