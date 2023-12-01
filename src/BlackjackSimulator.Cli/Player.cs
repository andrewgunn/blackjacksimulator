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

    public bool IsCardCounting { get; }
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

    public bool DoubleDown(Hand hand)
    {
        if (hand.HasDoubledBet)
        {
            return false;
        }

        Money -= hand.Bet;

        return hand.DoubleBet();
    }

    public void PlaceBet(Shoe shoe, int minimumBet)
    {
        var bet = minimumBet;

        if (IsCardCounting)
        {
            if (shoe.TrueCount < -3)
            {
                return;
            }

            if (shoe.TrueCount > 1)
            {
                bet = (bet * 5) * (shoe.TrueCount - 1);
            }
        }

        Money -= bet;

        _hands.Add(new Hand(bet));
    }

    public int RecordResult(Hand hand, Result result)
    {
        _results.Add(result);
        var bet = hand.Bet;

        switch (result)
        {
            case Result.Blackjack:
                Money += bet * 2 + bet / 2;
                return bet + bet / 2;
            case Result.Win:
                Money += bet * 2;
                return bet;
            case Result.Push:
                Money += bet;
                return 0;
            default:
                return -bet;
        }
    }

    public IReadOnlyCollection<Hand> SplitHand(Hand hand, int minimumBet)
    {
        Money -= minimumBet;

        var hands = hand.Split();

        _hands.Remove(hand);
        _hands.AddRange(hands);

        return hands;
    }

    public override string ToString()
    {
        var winCount = Results.Count(r => r == Result.Win);
        var lossCount = Results.Count(r => r == Result.Loss);

        return $"{Name}\tMoney = {Money:C2} Max: {MaximumMoney:C2} Min: {MinimumMoney:C2} Win/Loss: {winCount}/{lossCount}";
    }
}