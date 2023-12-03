using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackSimulator.Cli;

public class Hand(decimal bet) : IEquatable<Hand>
{
    private readonly List<Card> _cards = new();

    public decimal Bet => bet;
    public bool CanTakeAnotherCard => HasBet && (!HasDoubledBet || Cards.Count == 2) && !IsBust && !IsBlackjack && (!WasSplit || HasPairs || Cards.First().Rank != Rank.Ace || Cards.Count == 1);
    public IReadOnlyCollection<Card> Cards => _cards;
    public bool HasBet => Bet > 0;
    private bool HasDoubledBet { get; set; }
    public bool HasPairs => Cards.Count == 2 && Cards.Select(c => c.Rank).Distinct().Count() == 1;
    public bool IsBlackjack => Cards.Count == 2 && Value == 21;
    public bool IsBust => Value > 21;
    public bool IsSoft => Value < 21 && _cards.Any(c => c.Rank == Rank.Ace) && _cards.Where(c => c.Rank != Rank.Ace).Sum(c => c.Value) + _cards.Count(c => c.Rank == Rank.Ace) <= 11;
    public Result Result { get; private set; }
    public decimal Winnings { get; private set; }
    private bool WasSplit { get; init; }

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

    private int CompareTo(Hand b)
    {
        if (Value != b.Value)
        {
            return Value > b.Value ? 1 : -1;
        }

        return IsBlackjack switch
        {
            true when !b.IsBlackjack => 1,
            false when b.IsBlackjack => -1,
            _ => 0
        };
    }

    public bool DoubleBet()
    {
        if (!HasBet)
        {            
            throw new Exception("Bet can't be doubled without an original bet.");
        }

        if (HasDoubledBet)
        {
            throw new Exception("Bet has already been doubled.");
        }

        HasDoubledBet = true;
        bet *= 2;

        return true;
    }

    public bool Equals(Hand? other)
    {
        return other is not null && this == other;
    }

    public override bool Equals(object? obj)
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

    public void RecordResult(Result result)
    {
        Result = result;
        
        switch (result)
        {
            case Result.Blackjack:
                Winnings = bet * 1.5M;
                break;
            case Result.Win:
                Winnings = bet;
                break;
            case Result.Push:
                Winnings = 0;
                break;
            case Result.Loss:
            default:
                Winnings = -bet;
                break;
        }
    }

    public IReadOnlyCollection<Hand> Split()
    {
        if (!HasPairs)
        {
            throw new Exception("Hand cannot be split.");
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

    public static bool operator ==(Hand a, Hand b)
    {
        switch (a)
        {
            case null when ReferenceEquals(b, null):
                return true;
            case null:
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