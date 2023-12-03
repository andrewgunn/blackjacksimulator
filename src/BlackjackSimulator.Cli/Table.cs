namespace BlackjackSimulator.Cli;

public class Table
{
    public int GameCount { get; private set; } = 1;

    public Table(int deckCount,
        decimal minimumBet,
        IReadOnlyCollection<Player> players)
    {
        var decks = new List<Deck>();

        for (var i = 0;
            i < deckCount;
            i++)
        {
            var deck = new Deck();
            decks.Add(deck);
        }

        Shoe = new Shoe(decks);
        MinimumBet = minimumBet;
        Players = players;
        Dealer = new Dealer();
    }

    public Dealer Dealer { get; }
    private decimal MinimumBet { get; }
    public IReadOnlyCollection<Player> Players { get; }
    public Shoe Shoe { get; }

    public void StartNewGame()
    {
        Dealer.ClearHands(Shoe, Players);

        foreach (var player in Players.Where(p => p.Money > MinimumBet))
        {
            player.PlaceBet(Shoe, MinimumBet);
        }

        Dealer.Deal(Shoe, Players);

        foreach (var player in Players)
        {
            var handsCount = player.Hands.Count;

            for (var i = 0;
                i < handsCount;
                i++)
            {
                var hand = player.Hands.ElementAt(i);

                while (hand.CanTakeAnotherCard)
                {
                    var move = GetNextMove(player, hand);

                    if (move == Move.Hit)
                    {
                        Dealer.DealToHand(Shoe, hand);
                    }
                    else if (move == Move.Stand)
                    {
                        break;
                    }
                    else if (move == Move.Double)
                    {
                        player.DoubleDown(hand);

                        Dealer.DealToHand(Shoe, hand);
                    }
                    else if (move == Move.Split)
                    {
                        var hands = player.SplitHand(hand, MinimumBet);
                        handsCount++;

                        hand = hands.ElementAt(0);

                        Dealer.DealToHand(Shoe, hand);
                        Dealer.DealToHand(Shoe, hand: hands.ElementAt(1));
                    }
                }
            }
        }

        while (Dealer.CanTakeAnotherCard)
        {
            Dealer.DealToSelf(Shoe);
        }

        foreach (var player in Players)
        {
            foreach (var hand in player.Hands)
            {
                var result = Dealer.CalculateResult(hand);
                player.RecordResult(hand, result);
            }
        }

        GameCount++;
    }

    private Move GetNextMove(Player player,
        Hand hand)
    {
        var move = BasicStrategy.NextMove(hand, Dealer);

        switch (move)
        {
            case Move.Double:
                {
                    if (player.Money < hand.Bet)
                    {
                        move = Move.Hit;
                    }

                    break;
                }
            case Move.Split:
                {
                    if (player.Money < MinimumBet)
                    {
                        move = Move.Hit;
                    }

                    break;
                }
            case Move.Hit:
            case Move.Stand:
            default:
                break;
        }

        return move;
    }
}