using System;
using System.Collections.Generic;

namespace BlackjackSimulator.Cli;

public class Player
{
    private readonly List<Hand> _hands;
    private decimal _money;
    private readonly List<Result> _results;

    public Player(string name, decimal money, bool isCardCounting)
    {
        Name = isCardCounting ? $"{name}*" : name;
        Money = MaximumMoney = MinimumMoney = money;
        IsCardCounting = isCardCounting;

        _results = new List<Result>();
        _hands = new List<Hand>();
    }
    
    private bool IsCardCounting { get; }
    public IReadOnlyCollection<Hand> Hands => _hands;
    public decimal MaximumMoney { get; private set; }
    public decimal MinimumMoney { get; private set; }
    public decimal Money
    {
        get { return _money; }
        set
        {
            if (value < MinimumMoney)
            {
                MinimumMoney = value;
            }
            else if (value > MaximumMoney)
            {
                MaximumMoney = value;
            }

            _money = value;
        }
    }
    public string Name { get; }
    public IReadOnlyCollection<Result> Results => _results;

    public void ClearHands(Shoe shoe)
    {
        foreach (var hand in Hands)
        {
            shoe.DisposeHand(hand);
        }

        _hands.Clear();
    }

    public void DoubleDown(Hand hand)
    {
        if (Money < hand.Bet)
        {
            throw new Exception("Player doesn't have enough money to double down.");
        }
        
        hand.DoubleBet();

        Money -= hand.Bet;
    }

    public void PlaceBet(Shoe shoe, decimal minimumBet)
    {
        if (Money < minimumBet)
        {
            throw new Exception("Player doesn't have enough money to place minimum bet.");
        }
        
        var bet = minimumBet;

        if (IsCardCounting)
        {
            switch (shoe.TrueCount)
            {
                case < -3:
                    return;
                case > 1:
                    bet = bet * 5 * (shoe.TrueCount - 1);
                    break;
            }
        }

        _hands.Add(new Hand(bet));

        Money -= bet;
    }

    public void RecordResult(Hand hand, Result result)
    {
        hand.RecordResult(result);

        if (result != Result.Loss)
        {
            Money += hand.Bet + hand.Winnings;
        }

        _results.Add(result);
    }

    public IReadOnlyCollection<Hand> SplitHand(Hand hand, 
        decimal minimumBet)
    {
        if (Money < minimumBet)
        {
            throw new Exception("Player doesn't have enough money to split the hand.");
        }
        
        var hands = hand.Split();

        _hands.Remove(hand);
        _hands.AddRange(hands);

        Money -= minimumBet;

        return hands;
    }
}