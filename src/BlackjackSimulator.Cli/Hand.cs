namespace BlackjackSimulator.Cli;

public class Hand(int bet) : IEquatable<Hand>
{
    private readonly List<Card> _cards = new();

    public int Bet { get; private set; } = bet;
    public bool CanTakeAnotherCard => HasBet && (!HasDoubledBet || Cards.Count == 2) && !IsBust && !IsBlackjack && (!WasSplit || HasPairs || Cards.First().Rank != Rank.Ace || Cards.Count == 1);
    public IReadOnlyCollection<Card> Cards => _cards;
    public bool HasBet => Bet > 0;
    public bool HasDoubledBet { get; private set; }
    public bool HasPairs => Cards.Count == 2 && Cards.Select(c => c.Rank).Distinct().Count() == 1;
    public bool IsBlackjack => Cards.Count == 2 && Value == 21;
    public bool IsBust => Value > 21;
    public bool IsSoft => Value < 21 && _cards.Any(c => c.Rank == Rank.Ace) && _cards.Where(c => c.Rank != Rank.Ace).Sum(c => c.Value) + _cards.Count(c => c.Rank == Rank.Ace) <= 11;
    public bool WasSplit { get; private set; }

    public int Value
    {
        get
        {
            var value = _cards.Sum(c => c.Value);

            for (var i = 0; i < _cards.Count(c => c.Rank == Rank.Ace) && value > 21; i++)
            {
                value -= 10;
            }

            return value;
        }
    }

    public void AddCard(Card card)
    {
        _cards.Add(card);
    }

    public int CompareTo(Hand b)
    {
        if (Value != b.Value)
        {
            return Value > b.Value ? 1 : -1;
        }

        if (IsBlackjack && !b.IsBlackjack)
        {
            return 1;
        }

        if (!IsBlackjack && b.IsBlackjack)
        {
            return -1;
        }

        return 0;
    }

    public bool DoubleBet()
    {
        if (!HasBet || HasDoubledBet)
        {
            return false;
        }

        HasDoubledBet = true;
        Bet *= 2;

        return true;
    }

    public bool Equals(Hand other)
    {
        return this == other;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((Hand)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Value.GetHashCode();
            hashCode = (hashCode * 397) ^ Cards.Count.GetHashCode();

            return hashCode;
        }
    }

    public IReadOnlyCollection<Hand> Split()
    {
        if (!HasPairs)
        {
            return new Hand[0];
        }

        var a = new Hand(Bet)
        {
            WasSplit = true
        };
        a.AddCard(_cards.ElementAt(0));

        var b = new Hand(Bet)
        {
            WasSplit = true
        };

        b.AddCard(_cards.ElementAt(1));

        return new[]
        {
            a,
            b
        };
    }

    public override string ToString()
    {
        var bet = HasBet ? Bet.ToString("C0") : "-";

        return $"{bet}\t{Value}\t{string.Join(" ", Cards)}";
    }

    public static bool operator ==(Hand a, Hand b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
        {
            return true;
        }

        if (ReferenceEquals(a, null))
        {
            return false;
        }

        if (ReferenceEquals(b, null))
        {
            return false;
        }

        if (ReferenceEquals(a, b))
        {
            return true;
        }

        return a.CompareTo(b) == 0;
    }

    public static bool operator !=(Hand a, Hand b)
    {
        return !(a == b);
    }

    public static bool operator <(Hand a, Hand b)
    {
        return a.CompareTo(b) < 0;
    }

    public static bool operator >(Hand a, Hand b)
    {
        return a.CompareTo(b) > 0;
    }
}