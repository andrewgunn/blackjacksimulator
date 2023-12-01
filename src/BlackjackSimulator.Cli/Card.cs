namespace BlackjackSimulator.Cli;

public class Card(Suit suit, Rank rank)
{
    private static readonly Dictionary<Rank, string> _rankShortNameMappings;
    private static readonly Dictionary<Rank, int> _rankValueMappings;
    private static readonly Dictionary<Suit, string> _suitShortNameMappings;

    static Card()
    {
        _rankShortNameMappings = new Dictionary<Rank, string>
        {
            {Rank.Two, "2"},
            {Rank.Three, "3"},
            {Rank.Four, "4"},
            {Rank.Five, "5"},
            {Rank.Six, "6"},
            {Rank.Seven, "7"},
            {Rank.Eight, "8"},
            {Rank.Nine, "9"},
            {Rank.Ten, "10"},
            {Rank.Jack, "J"},
            {Rank.Queen, "Q"},
            {Rank.King, "K"},
            {Rank.Ace, "A"}
        };
        _rankValueMappings = new Dictionary<Rank, int>
        {
            {Rank.Two, 2},
            {Rank.Three, 3},
            {Rank.Four, 4},
            {Rank.Five, 5},
            {Rank.Six, 6},
            {Rank.Seven, 7},
            {Rank.Eight, 8},
            {Rank.Nine, 9},
            {Rank.Ten, 10},
            {Rank.Jack, 10},
            {Rank.Queen, 10},
            {Rank.King, 10},
            {Rank.Ace, 11}
        };
        _suitShortNameMappings = new Dictionary<Suit, string>
        {
            {Suit.Clubs, "C"},
            {Suit.Diamonds, "D"},
            {Suit.Hearts, "H"},
            {Suit.Spades, "S"}
        };
    }

    public Rank Rank { get; } = rank;
    public Suit Suit { get; } = suit;
    public int Value => _rankValueMappings[Rank];

    public override string ToString()
    {
        return $"{_rankShortNameMappings[Rank]}{_suitShortNameMappings[Suit]}";
    }
}