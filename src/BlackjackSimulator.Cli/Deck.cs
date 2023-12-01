namespace BlackjackSimulator.Cli;

public class Deck
{
    public Deck()
    {
        var cards = new List<Card>();
        cards.AddRange(Enum.GetValues(typeof(Suit))
            .Cast<Suit>()
            .SelectMany(suit => Enum.GetValues(typeof(Rank)).Cast<Rank>(),
                (suit, rank) => new Card(suit, rank)));

        Cards = cards.Shuffle();
    }

    public IReadOnlyCollection<Card> Cards { get; }
}