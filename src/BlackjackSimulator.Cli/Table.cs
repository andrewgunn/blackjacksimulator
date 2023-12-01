namespace BlackjackSimulator.Cli;

public class Table
{
    public Table(int deckCount, int minimumBet, int playersWhoCount, int n00bsCount)
    {
        var decks = new List<Deck>();

        for (var i = 0; i < deckCount; i++)
        {
            var deck = new Deck();
            decks.Add(deck);
        }

        Shoe = new Shoe(decks);

        MinimumBet = minimumBet;

        var players = new List<Player>();

        for (var i = 0; i < playersWhoCount; i++)
        {
            var player = new Player($"P{i + 1}", 10000, true);
            players.Add(player);
        }

        for (var i = playersWhoCount; i < playersWhoCount + n00bsCount; i++)
        {
            var player = new Player($"P{i + 1}", 10000, false);
            players.Add(player);
        }

        Players = players;
        Dealer = new Dealer();
    }

    private Dealer Dealer { get; }

    private int MinimumBet { get; }
    private IReadOnlyCollection<Player> Players { get; }
    private Shoe Shoe { get; }

    private void StartNewGame(int gameCount)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"Game #{gameCount} Card Count: {Shoe.Cards.Count} Deck Count: {Shoe.DeckCount} Running Count: {Shoe.RunningCount} True Count: {Shoe.TrueCount}");
        Console.WriteLine("---");
        Console.ResetColor();

        foreach (var player in Players)
        {
            player.PlaceBet(Shoe, MinimumBet);
        }

        Dealer.Deal(Shoe, Players);

        Console.WriteLine($"{Dealer}");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("---");
        Console.ResetColor();

        foreach (var player in Players)
        {
            var handsCount = player.Hands.Count;

            for (var i = 0; i < handsCount; i++)
            {
                var hand = player.Hands.ElementAt(i);

                if (hand.IsBlackjack)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine($"{player.Name}\t{hand}");
                Console.ResetColor();

                while (hand.CanTakeAnotherCard)
                {
                    // Auto
                    var move = BasicStrategy.NextMove(hand, Dealer);

                    // Manual
                    //Console.ForegroundColor = ConsoleColor.DarkGray;
                    //Console.Write("h/d/s... ");

                    //var key = Console.ReadKey();

                    //Move move;

                    //switch (key.Key)
                    //{
                    //     case ConsoleKey.H:
                    //          move = Move.Hit;
                    //          break;
                    //     case ConsoleKey.D:
                    //          move = Move.Double;
                    //          break;
                    //     default:
                    //          move = Move.Stand;
                    //          break;
                    //}

                    //Console.WriteLine();
                    //Console.ResetColor();

                    if (move == Move.Stand)
                    {
                        break;
                    }

                    if (move == Move.Split)
                    {
                        var previousHand = hand;
                        var hands = player.SplitHand(hand, MinimumBet);

                        if (hands.Count == 1)
                        {
                            Dealer.DealToPlayer(Shoe, hand);
                        }
                        else
                        {
                            hand = hands.ElementAt(0);
                            handsCount++;

                            Dealer.DealToPlayer(Shoe, hand);
                            Dealer.DealToPlayer(Shoe, hands.ElementAt(1));

                            Console.SetCursorPosition(0, Console.CursorTop - 1);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"{player.Name}\t{previousHand}");
                            Console.ResetColor();

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("---");
                            Console.ResetColor();
                        }
                    }
                    else if (move == Move.Double)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"{player.Name}\t{hand}");
                        Console.ResetColor();

                        if (!Dealer.DoubleDownToPlayer(player, Shoe, hand))
                        {
                            Dealer.DealToPlayer(Shoe, hand);
                        }
                    }
                    else
                    {
                        Dealer.DealToPlayer(Shoe, hand);
                    }

                    if (hand.IsBust)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.WriteLine($"{player.Name}\t{hand}");
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("---");
                Console.ResetColor();
            }
        }

        Console.WriteLine(Dealer);

        while (Dealer.CanTakeAnotherCard)
        {
            Dealer.DealToSelf(Shoe);

            if (Dealer.Hand.IsBust)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine(Dealer);
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("---");
        Console.ResetColor();

        foreach (var player in Players)
        {
            if (player.Hands.Any(h => h.HasBet))
            {
                var totalWinnings = (from hand in player.Hands.Where(h => h.HasBet) let result = Dealer.CalculateResult(hand) select player.RecordResult(hand, result)).Sum();

                Console.ForegroundColor = totalWinnings switch
                {
                    > 0 => ConsoleColor.Green,
                    0 => ConsoleColor.Yellow,
                    < 0 => ConsoleColor.Red,
                };

                Console.WriteLine($"{player} {totalWinnings:C2}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(player);
            }

            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("---");
        Console.ResetColor();

        Dealer.ClearHands(Shoe, Players);
    }

    public void Start()
    {
        var i = 1;
        ConsoleKeyInfo key;

        do
        {
            Console.Clear();

            StartNewGame(i++);

            Console.Write("Press any to start a new game...");

            key = Console.ReadKey();
        }
        while (key.Key != ConsoleKey.Escape);

        Console.ReadKey();
    }
}