namespace BlackjackSimulator
{
   using System;
   using System.Collections.Generic;
   using System.Linq;

   public static class ListExtensions
   {
      private static readonly Random _random;

      static ListExtensions()
      {
         _random = new Random(DateTime.UtcNow.Millisecond);
      }

      public static IReadOnlyCollection<T> Shuffle<T>(this List<T> extended)
      {
         var n = extended.Count;

         while (n > 1)
         {
            n--;
            var k = _random.Next(n + 1);
            var value = extended[k];
            extended[k] = extended[n];
            extended[n] = value;
         }

         return extended;
      }
   }

   public enum Rank
   {
      Two,
      Three,
      Four,
      Five,
      Six,
      Seven,
      Eight,
      Nine,
      Ten,
      Jack,
      Queen,
      King,
      Ace
   }

   public enum Suit
   {
      Clubs,
      Diamonds,
      Hearts,
      Spades
   }

   public enum Move
   {
      Hit,
      Stand,
      Double,
      Split
   }

   public enum Result
   {
      Blackjack,
      Win,
      Loss,
      Push
   }

   public class Card
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

      public Card(Suit suit, Rank rank)
      {
         Suit = suit;
         Rank = rank;
      }

      public Rank Rank { get; }
      public Suit Suit { get; }
      public int Value => _rankValueMappings[Rank];

      public override string ToString()
      {
         return $"{_rankShortNameMappings[Rank]}{_suitShortNameMappings[Suit]}";
      }
   }

   public class Shoe
   {
      private readonly List<Card> _cards;
      private readonly List<Card> _disposedCards;

      public Shoe(IReadOnlyCollection<Deck> decks)
      {
         var cutCard = (int)Math.Ceiling(decks.SelectMany(c => c.Cards).Count() * .75);

         _cards = decks.SelectMany(d => d.Cards)
             .Take(cutCard)
             .ToList();
         _disposedCards = decks.SelectMany(d => d.Cards)
             .Skip(cutCard)
             .ToList();
      }

      public int Count { get; private set; }
      public int TrueCount => Count == 0 ? 0 : (int)Math.Ceiling(Count / Math.Ceiling(_cards.Count / 52d));
      public IReadOnlyCollection<Card> Cards => _cards;

      public void DisposeHand(Hand hand)
      {
         _disposedCards.AddRange(hand.Cards);
      }

      public void Reshuffle()
      {
         _cards.AddRange(_disposedCards);
         _cards.Shuffle();

         _disposedCards.Clear();

         Count = 0;
      }

      public Card TakeNextCard()
      {
         if (!_cards.Any())
         {
            Reshuffle();
         }

         var card = _cards.First();

         if (card.Value <= 6)
         {
            Count++;
         }
         else if (card.Value >= 10)
         {
            Count--;
         }

         _cards.Remove(card);

         return card;
      }
   }

   public class Hand : IEquatable<Hand>
   {
      private readonly List<Card> _cards;

      public Hand(int bet)
      {
         Bet = bet;

         _cards = new List<Card>();
      }

      public int Bet { get; private set; }
      public bool CanTakeAnotherCard => HasBet && (!HasDoubledDown || Cards.Count == 2) && !IsBust && !IsBlackjack && (!WasSplit || IsPairs || Cards.First().Rank != Rank.Ace || Cards.Count == 1);
      public IReadOnlyCollection<Card> Cards => _cards;
      public bool IsBlackjack => Cards.Count == 2 && Value == 21;
      public bool IsBust => Value > 21;
      public bool IsPairs => Cards.Count == 2 && Cards.Select(c => c.Rank).Distinct().Count() == 1;
      public bool IsSoft => Value < 21 && _cards.Any(c => c.Rank == Rank.Ace) && _cards.Where(c => c.Rank != Rank.Ace).Sum(c => c.Value) + _cards.Count(c => c.Rank == Rank.Ace) <= 11;
      public bool WasSplit { get; private set; }
      public bool HasBet => Bet > 0;
      public bool HasDoubledDown { get; private set; }

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
         if (!HasBet || HasDoubledDown)
         {
            return false;
         }

         HasDoubledDown = true;
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
         if (!IsPairs)
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

   public class Player
   {
      private readonly List<Hand> _hands;
      private decimal _money;
      private readonly List<Result> _results;

      public Player(string name, decimal money, bool countsCards)
      {
         Name = countsCards ? $"{name}*" : name;
         Money = money;
         CountsCards = countsCards;

         _results = new List<Result>();
         _hands = new List<Hand>();
      }

      public bool CountsCards { get; }
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
         if (hand.HasDoubledDown)
         {
            return false;
         }

         Money -= hand.Bet;

         return hand.DoubleBet();
      }

      public void PlaceBet(Shoe shoe, int minimumBet)
      {
         var bet = minimumBet;

         if (CountsCards && shoe.TrueCount > 0)
         {
            bet = minimumBet + minimumBet * shoe.TrueCount;
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
               Money += (bet * 2) + (bet / 2);
               return bet + (bet / 2);
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
         var pushCount = Results.Count(r => r == Result.Push);

         return $"{Name}\tMoney = {Money:C2} Max Money = {MaximumMoney:C2} Min Money = {MinimumMoney:C2} Wins = {winCount} Pushes = {pushCount} Losses = {lossCount}";
      }
   }

   public class Dealer
   {
      public Dealer()
      {
         Hand = new Hand(0);
      }

      public bool CanTakeAnotherCard => !Hand.IsBust && Hand.Value < 17;
      public Hand Hand { get; private set; }
      public string Name => "Dealer";

      public Result CalculateResult(Hand hand)
      {
         if (hand.IsBust)
         {
            return Result.Loss;
         }

         if (Hand.IsBust)
         {
            return Result.Win;
         }

         if (hand.IsBlackjack && !Hand.IsBlackjack)
         {
            return Result.Blackjack;
         }

         if (hand == Hand)
         {
            return Result.Push;
         }

         return hand > Hand ? Result.Win : Result.Loss;
      }

      public void ClearHands(Shoe shoe, IReadOnlyCollection<Player> players)
      {
         shoe.DisposeHand(Hand);
         Hand = new Hand(0);

         foreach (var player in players)
         {
            player.ClearHands(shoe);
         }
      }

      public void Deal(Shoe shoe, IReadOnlyCollection<Player> players)
      {
         foreach (var player in players)
         {
            foreach (var hand in player.Hands.Where(h => h.HasBet))
            {
               hand.AddCard(shoe.TakeNextCard());
            }
         }

         Hand.AddCard(shoe.TakeNextCard());

         foreach (var player in players)
         {
            foreach (var hand in player.Hands.Where(h => h.HasBet))
            {
               hand.AddCard(shoe.TakeNextCard());
            }
         }
      }

      public void DealToPlayer(Shoe shoe, Hand hand)
      {
         if (!hand.CanTakeAnotherCard)
         {
            return;
         }

         hand.AddCard(shoe.TakeNextCard());
      }

      public void DealToSelf(Shoe shoe)
      {
         if (!CanTakeAnotherCard)
         {
            return;
         }

         Hand.AddCard(shoe.TakeNextCard());
      }

      public bool DoubleDownToPlayer(Player player, Shoe shoe, Hand hand)
      {
         if ( !player.DoubleDown( hand ) )
         {
            return false;
         }

         DealToPlayer(shoe, hand);

         return true;
      }

      public override string ToString()
      {
         return $"D\t{Hand}";
      }
   }

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
            var player = new Player($"P{i + 1}", 0, true);
            players.Add(player);
         }

         for (var i = playersWhoCount; i < playersWhoCount + n00bsCount; i++)
         {
            var player = new Player($"P{i + 1}", 0, false);
            players.Add(player);
         }

         Players = players;
         Dealer = new Dealer();
      }

      public Dealer Dealer { get; }
      public int MinimumBet { get; }
      public Shoe Shoe { get; }
      public IReadOnlyCollection<Player> Players { get; }

      private void StartNewGame(int gameCount)
      {
         Console.ForegroundColor = ConsoleColor.DarkGray;
         Console.WriteLine($"Game = {gameCount} Deck = {Shoe.Cards.Count} Count = {Shoe.Count} True Count = {Shoe.TrueCount}");
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
                  //    case ConsoleKey.H:
                  //        move = Move.Hit;
                  //        break;
                  //    case ConsoleKey.D:
                  //        move = Move.Double;
                  //        break;
                  //    default:
                  //        move = Move.Stand;
                  //        break;
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
               var totalWinnings = 0;

               foreach (var hand in player.Hands.Where(h => h.HasBet))
               {
                  var result = Dealer.CalculateResult(hand);
                  totalWinnings += player.RecordResult(hand, result);
               }

               if (totalWinnings > 0)
               {
                  Console.ForegroundColor = ConsoleColor.Green;
               }
               else if (totalWinnings == 0)
               {
                  Console.ForegroundColor = ConsoleColor.Yellow;
               }
               else if (totalWinnings < 0)
               {
                  Console.ForegroundColor = ConsoleColor.Red;
               }

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

   public static class BasicStrategy
   {
      private static readonly Dictionary<Tuple<int, bool, int>, Move> _hardSoftMoveMappings;
      private static readonly Dictionary<Tuple<Rank, int>, Move> _splitMoveMappings;

      static BasicStrategy()
      {
         _hardSoftMoveMappings = new Dictionary<Tuple<int, bool, int>, Move>
            {
                // Player: 4-21 (Hard) Dealer: 2
                {Tuple.Create(4, false, 2), Move.Hit},
                {Tuple.Create(5, false, 2), Move.Hit},
                {Tuple.Create(6, false, 2), Move.Hit},
                {Tuple.Create(7, false, 2), Move.Hit},
                {Tuple.Create(8, false, 2), Move.Hit},
                {Tuple.Create(9, false, 2), Move.Hit},
                {Tuple.Create(10, false, 2), Move.Double},
                {Tuple.Create(11, false, 2), Move.Double},
                {Tuple.Create(12, false, 2), Move.Hit},
                {Tuple.Create(13, false, 2), Move.Stand},
                {Tuple.Create(14, false, 2), Move.Stand},
                {Tuple.Create(15, false, 2), Move.Stand},
                {Tuple.Create(16, false, 2), Move.Stand},
                {Tuple.Create(17, false, 2), Move.Stand},
                {Tuple.Create(18, false, 2), Move.Stand},
                {Tuple.Create(19, false, 2), Move.Stand},
                {Tuple.Create(20, false, 2), Move.Stand},
                {Tuple.Create(21, false, 2), Move.Stand},

                // Player: 4-21 (Hard) Dealer: 3
                {Tuple.Create(4, false, 3), Move.Hit},
                {Tuple.Create(5, false, 3), Move.Hit},
                {Tuple.Create(6, false, 3), Move.Hit},
                {Tuple.Create(7, false, 3), Move.Hit},
                {Tuple.Create(8, false, 3), Move.Hit},
                {Tuple.Create(9, false, 3), Move.Double},
                {Tuple.Create(10, false, 3), Move.Double},
                {Tuple.Create(11, false, 3), Move.Double},
                {Tuple.Create(12, false, 3), Move.Hit},
                {Tuple.Create(13, false, 3), Move.Stand},
                {Tuple.Create(14, false, 3), Move.Stand},
                {Tuple.Create(15, false, 3), Move.Stand},
                {Tuple.Create(16, false, 3), Move.Stand},
                {Tuple.Create(17, false, 3), Move.Stand},
                {Tuple.Create(18, false, 3), Move.Stand},
                {Tuple.Create(19, false, 3), Move.Stand},
                {Tuple.Create(20, false, 3), Move.Stand},
                {Tuple.Create(21, false, 3), Move.Stand},

                // Player: 4-21 (Hard) Dealer: 4
                {Tuple.Create(4, false, 4), Move.Hit},
                {Tuple.Create(5, false, 4), Move.Hit},
                {Tuple.Create(6, false, 4), Move.Hit},
                {Tuple.Create(7, false, 4), Move.Hit},
                {Tuple.Create(8, false, 4), Move.Hit},
                {Tuple.Create(9, false, 4), Move.Double},
                {Tuple.Create(10, false, 4), Move.Double},
                {Tuple.Create(11, false, 4), Move.Double},
                {Tuple.Create(12, false, 4), Move.Stand},
                {Tuple.Create(13, false, 4), Move.Stand},
                {Tuple.Create(14, false, 4), Move.Stand},
                {Tuple.Create(15, false, 4), Move.Stand},
                {Tuple.Create(16, false, 4), Move.Stand},
                {Tuple.Create(17, false, 4), Move.Stand},
                {Tuple.Create(18, false, 4), Move.Stand},
                {Tuple.Create(19, false, 4), Move.Stand},
                {Tuple.Create(20, false, 4), Move.Stand},
                {Tuple.Create(21, false, 4), Move.Stand},

                // Player: 4-21 (Hard) Dealer: 5
                {Tuple.Create(4, false, 5), Move.Hit},
                {Tuple.Create(5, false, 5), Move.Hit},
                {Tuple.Create(6, false, 5), Move.Hit},
                {Tuple.Create(7, false, 5), Move.Hit},
                {Tuple.Create(8, false, 5), Move.Hit},
                {Tuple.Create(9, false, 5), Move.Double},
                {Tuple.Create(10, false, 5), Move.Double},
                {Tuple.Create(11, false, 5), Move.Double},
                {Tuple.Create(12, false, 5), Move.Stand},
                {Tuple.Create(13, false, 5), Move.Stand},
                {Tuple.Create(14, false, 5), Move.Stand},
                {Tuple.Create(15, false, 5), Move.Stand},
                {Tuple.Create(16, false, 5), Move.Stand},
                {Tuple.Create(17, false, 5), Move.Stand},
                {Tuple.Create(18, false, 5), Move.Stand},
                {Tuple.Create(19, false, 5), Move.Stand},
                {Tuple.Create(20, false, 5), Move.Stand},
                {Tuple.Create(21, false, 5), Move.Stand},

                // Player: 4-21 (Hard) Dealer: 6
                {Tuple.Create(4, false, 6), Move.Hit},
                {Tuple.Create(5, false, 6), Move.Hit},
                {Tuple.Create(6, false, 6), Move.Hit},
                {Tuple.Create(7, false, 6), Move.Hit},
                {Tuple.Create(8, false, 6), Move.Hit},
                {Tuple.Create(9, false, 6), Move.Double},
                {Tuple.Create(10, false, 6), Move.Double},
                {Tuple.Create(11, false, 6), Move.Double},
                {Tuple.Create(12, false, 6), Move.Stand},
                {Tuple.Create(13, false, 6), Move.Stand},
                {Tuple.Create(14, false, 6), Move.Stand},
                {Tuple.Create(15, false, 6), Move.Stand},
                {Tuple.Create(16, false, 6), Move.Stand},
                {Tuple.Create(17, false, 6), Move.Stand},
                {Tuple.Create(18, false, 6), Move.Stand},
                {Tuple.Create(19, false, 6), Move.Stand},
                {Tuple.Create(20, false, 6), Move.Stand},
                {Tuple.Create(21, false, 6), Move.Stand},

                // Player: 4-21 (Hard) Dealer: 7
                {Tuple.Create(4, false, 7), Move.Hit},
                {Tuple.Create(5, false, 7), Move.Hit},
                {Tuple.Create(6, false, 7), Move.Hit},
                {Tuple.Create(7, false, 7), Move.Hit},
                {Tuple.Create(8, false, 7), Move.Hit},
                {Tuple.Create(9, false, 7), Move.Hit},
                {Tuple.Create(10, false, 7), Move.Double},
                {Tuple.Create(11, false, 7), Move.Double},
                {Tuple.Create(12, false, 7), Move.Hit},
                {Tuple.Create(13, false, 7), Move.Hit},
                {Tuple.Create(14, false, 7), Move.Hit},
                {Tuple.Create(15, false, 7), Move.Hit},
                {Tuple.Create(16, false, 7), Move.Hit},
                {Tuple.Create(17, false, 7), Move.Stand},
                {Tuple.Create(18, false, 7), Move.Stand},
                {Tuple.Create(19, false, 7), Move.Stand},
                {Tuple.Create(20, false, 7), Move.Stand},
                {Tuple.Create(21, false, 7), Move.Stand},

                // Player: 4-21 (Hard) Dealer: 8
                {Tuple.Create(4, false, 8), Move.Hit},
                {Tuple.Create(5, false, 8), Move.Hit},
                {Tuple.Create(6, false, 8), Move.Hit},
                {Tuple.Create(7, false, 8), Move.Hit},
                {Tuple.Create(8, false, 8), Move.Hit},
                {Tuple.Create(9, false, 8), Move.Hit},
                {Tuple.Create(10, false, 8), Move.Double},
                {Tuple.Create(11, false, 8), Move.Double},
                {Tuple.Create(12, false, 8), Move.Hit},
                {Tuple.Create(13, false, 8), Move.Hit},
                {Tuple.Create(14, false, 8), Move.Hit},
                {Tuple.Create(15, false, 8), Move.Hit},
                {Tuple.Create(16, false, 8), Move.Hit},
                {Tuple.Create(17, false, 8), Move.Stand},
                {Tuple.Create(18, false, 8), Move.Stand},
                {Tuple.Create(19, false, 8), Move.Stand},
                {Tuple.Create(20, false, 8), Move.Stand},
                {Tuple.Create(21, false, 8), Move.Stand},

                // Player: 4-21 (Hard) Dealer: 9
                {Tuple.Create(4, false, 9), Move.Hit},
                {Tuple.Create(5, false, 9), Move.Hit},
                {Tuple.Create(6, false, 9), Move.Hit},
                {Tuple.Create(7, false, 9), Move.Hit},
                {Tuple.Create(8, false, 9), Move.Hit},
                {Tuple.Create(9, false, 9), Move.Hit},
                {Tuple.Create(10, false, 9), Move.Double},
                {Tuple.Create(11, false, 9), Move.Double},
                {Tuple.Create(12, false, 9), Move.Hit},
                {Tuple.Create(13, false, 9), Move.Hit},
                {Tuple.Create(14, false, 9), Move.Hit},
                {Tuple.Create(15, false, 9), Move.Hit},
                {Tuple.Create(16, false, 9), Move.Hit},
                {Tuple.Create(17, false, 9), Move.Stand},
                {Tuple.Create(18, false, 9), Move.Stand},
                {Tuple.Create(19, false, 9), Move.Stand},
                {Tuple.Create(20, false, 9), Move.Stand},
                {Tuple.Create(21, false, 9), Move.Stand},

                // Player: 4-21 (Hard) Dealer: 10
                {Tuple.Create(4, false, 10), Move.Hit},
                {Tuple.Create(5, false, 10), Move.Hit},
                {Tuple.Create(6, false, 10), Move.Hit},
                {Tuple.Create(7, false, 10), Move.Hit},
                {Tuple.Create(8, false, 10), Move.Hit},
                {Tuple.Create(9, false, 10), Move.Hit},
                {Tuple.Create(10, false, 10), Move.Hit},
                {Tuple.Create(11, false, 10), Move.Double},
                {Tuple.Create(12, false, 10), Move.Hit},
                {Tuple.Create(13, false, 10), Move.Hit},
                {Tuple.Create(14, false, 10), Move.Hit},
                {Tuple.Create(15, false, 10), Move.Hit},
                {Tuple.Create(16, false, 10), Move.Hit},
                {Tuple.Create(17, false, 10), Move.Stand},
                {Tuple.Create(18, false, 10), Move.Stand},
                {Tuple.Create(19, false, 10), Move.Stand},
                {Tuple.Create(20, false, 10), Move.Stand},
                {Tuple.Create(21, false, 10), Move.Stand},

                // Player: 4-21 (Hard) Dealer: 11
                {Tuple.Create(4, false, 11), Move.Hit},
                {Tuple.Create(5, false, 11), Move.Hit},
                {Tuple.Create(6, false, 11), Move.Hit},
                {Tuple.Create(7, false, 11), Move.Hit},
                {Tuple.Create(8, false, 11), Move.Hit},
                {Tuple.Create(9, false, 11), Move.Hit},
                {Tuple.Create(10, false, 11), Move.Hit},
                {Tuple.Create(11, false, 11), Move.Double},
                {Tuple.Create(12, false, 11), Move.Hit},
                {Tuple.Create(13, false, 11), Move.Hit},
                {Tuple.Create(14, false, 11), Move.Hit},
                {Tuple.Create(15, false, 11), Move.Hit},
                {Tuple.Create(16, false, 11), Move.Hit},
                {Tuple.Create(17, false, 11), Move.Stand},
                {Tuple.Create(18, false, 11), Move.Stand},
                {Tuple.Create(19, false, 11), Move.Stand},
                {Tuple.Create(20, false, 11), Move.Stand},
                {Tuple.Create(21, false, 11), Move.Stand},

                // Player: 13-21 (Soft) Dealer: 2
                {Tuple.Create(13, true, 2), Move.Hit},
                {Tuple.Create(14, true, 2), Move.Hit},
                {Tuple.Create(15, true, 2), Move.Hit},
                {Tuple.Create(16, true, 2), Move.Hit},
                {Tuple.Create(17, true, 2), Move.Hit},
                {Tuple.Create(18, true, 2), Move.Double},
                {Tuple.Create(19, true, 2), Move.Stand},
                {Tuple.Create(20, true, 2), Move.Stand},
                {Tuple.Create(21, true, 2), Move.Stand},

                // Player: 13-21 (Soft) Dealer: 3
                {Tuple.Create(13, true, 3), Move.Hit},
                {Tuple.Create(14, true, 3), Move.Hit},
                {Tuple.Create(15, true, 3), Move.Hit},
                {Tuple.Create(16, true, 3), Move.Hit},
                {Tuple.Create(17, true, 3), Move.Double},
                {Tuple.Create(18, true, 3), Move.Double},
                {Tuple.Create(19, true, 3), Move.Stand},
                {Tuple.Create(20, true, 3), Move.Stand},
                {Tuple.Create(21, true, 3), Move.Stand},

                // Player: 13-21 (Soft) Dealer: 4
                {Tuple.Create(13, true, 4), Move.Hit},
                {Tuple.Create(14, true, 4), Move.Hit},
                {Tuple.Create(15, true, 4), Move.Double},
                {Tuple.Create(16, true, 4), Move.Double},
                {Tuple.Create(17, true, 4), Move.Double},
                {Tuple.Create(18, true, 4), Move.Double},
                {Tuple.Create(19, true, 4), Move.Stand},
                {Tuple.Create(20, true, 4), Move.Stand},
                {Tuple.Create(21, true, 4), Move.Stand},

                // Player: 13-21 (Soft) Dealer: 5
                {Tuple.Create(13, true, 5), Move.Double},
                {Tuple.Create(14, true, 5), Move.Double},
                {Tuple.Create(15, true, 5), Move.Double},
                {Tuple.Create(16, true, 5), Move.Double},
                {Tuple.Create(17, true, 5), Move.Double},
                {Tuple.Create(18, true, 5), Move.Double},
                {Tuple.Create(19, true, 5), Move.Stand},
                {Tuple.Create(20, true, 5), Move.Stand},
                {Tuple.Create(21, true, 5), Move.Stand},

                // Player: 13-21 (Soft) Dealer: 6
                {Tuple.Create(13, true, 6), Move.Double},
                {Tuple.Create(14, true, 6), Move.Double},
                {Tuple.Create(15, true, 6), Move.Double},
                {Tuple.Create(16, true, 6), Move.Double},
                {Tuple.Create(17, true, 6), Move.Double},
                {Tuple.Create(18, true, 6), Move.Double},
                {Tuple.Create(19, true, 6), Move.Double},
                {Tuple.Create(20, true, 6), Move.Stand},
                {Tuple.Create(21, true, 6), Move.Stand},

                // Player: 13-21 (Soft) Dealer: 7
                {Tuple.Create(13, true, 7), Move.Hit},
                {Tuple.Create(14, true, 7), Move.Hit},
                {Tuple.Create(15, true, 7), Move.Hit},
                {Tuple.Create(16, true, 7), Move.Hit},
                {Tuple.Create(17, true, 7), Move.Hit},
                {Tuple.Create(18, true, 7), Move.Stand},
                {Tuple.Create(19, true, 7), Move.Stand},
                {Tuple.Create(20, true, 7), Move.Stand},
                {Tuple.Create(21, true, 7), Move.Stand},

                // Player: 13-21 (Soft) Dealer: 8
                {Tuple.Create(13, true, 8), Move.Hit},
                {Tuple.Create(14, true, 8), Move.Hit},
                {Tuple.Create(15, true, 8), Move.Hit},
                {Tuple.Create(16, true, 8), Move.Hit},
                {Tuple.Create(17, true, 8), Move.Hit},
                {Tuple.Create(18, true, 8), Move.Stand},
                {Tuple.Create(19, true, 8), Move.Stand},
                {Tuple.Create(20, true, 8), Move.Stand},
                {Tuple.Create(21, true, 8), Move.Stand},

                // Player: 13-21 (Soft) Dealer: 9
                {Tuple.Create(13, true, 9), Move.Hit},
                {Tuple.Create(14, true, 9), Move.Hit},
                {Tuple.Create(15, true, 9), Move.Hit},
                {Tuple.Create(16, true, 9), Move.Hit},
                {Tuple.Create(17, true, 9), Move.Hit},
                {Tuple.Create(18, true, 9), Move.Hit},
                {Tuple.Create(19, true, 9), Move.Stand},
                {Tuple.Create(20, true, 9), Move.Stand},
                {Tuple.Create(21, true, 9), Move.Stand},

                // Player: 13-21 (Soft) Dealer: 10
                {Tuple.Create(13, true, 10), Move.Hit},
                {Tuple.Create(14, true, 10), Move.Hit},
                {Tuple.Create(15, true, 10), Move.Hit},
                {Tuple.Create(16, true, 10), Move.Hit},
                {Tuple.Create(17, true, 10), Move.Hit},
                {Tuple.Create(18, true, 10), Move.Hit},
                {Tuple.Create(19, true, 10), Move.Stand},
                {Tuple.Create(20, true, 10), Move.Stand},
                {Tuple.Create(21, true, 10), Move.Stand},

                // Player: 13-21 (Soft) Dealer: 11
                {Tuple.Create(13, true, 11), Move.Hit},
                {Tuple.Create(14, true, 11), Move.Hit},
                {Tuple.Create(15, true, 11), Move.Hit},
                {Tuple.Create(16, true, 11), Move.Hit},
                {Tuple.Create(17, true, 11), Move.Hit},
                {Tuple.Create(18, true, 11), Move.Hit},
                {Tuple.Create(19, true, 11), Move.Stand},
                {Tuple.Create(20, true, 11), Move.Stand},
                {Tuple.Create(21, true, 11), Move.Stand}
            };

         _splitMoveMappings = new Dictionary<Tuple<Rank, int>, Move>
            {
                // Player: Doubles Dealer: 2
                {Tuple.Create(Rank.Two, 2), Move.Split},
                {Tuple.Create(Rank.Three, 2), Move.Split},
                {Tuple.Create(Rank.Four, 2), Move.Hit},
                {Tuple.Create(Rank.Five, 2), Move.Double},
                {Tuple.Create(Rank.Six, 2), Move.Split},
                {Tuple.Create(Rank.Seven, 2), Move.Split},
                {Tuple.Create(Rank.Eight, 2), Move.Split},
                {Tuple.Create(Rank.Nine, 2), Move.Split},
                {Tuple.Create(Rank.Ten, 2), Move.Stand},
                {Tuple.Create(Rank.Jack, 2), Move.Stand},
                {Tuple.Create(Rank.Queen, 2), Move.Stand},
                {Tuple.Create(Rank.King, 2), Move.Stand},
                {Tuple.Create(Rank.Ace, 2), Move.Split},
                
                // Player: Doubles Dealer: 3
                {Tuple.Create(Rank.Two, 3), Move.Split},
                {Tuple.Create(Rank.Three, 3), Move.Split},
                {Tuple.Create(Rank.Four, 3), Move.Hit},
                {Tuple.Create(Rank.Five, 3), Move.Double},
                {Tuple.Create(Rank.Six, 3), Move.Split},
                {Tuple.Create(Rank.Seven, 3), Move.Split},
                {Tuple.Create(Rank.Eight, 3), Move.Split},
                {Tuple.Create(Rank.Nine, 3), Move.Split},
                {Tuple.Create(Rank.Ten, 3), Move.Stand},
                {Tuple.Create(Rank.Jack, 3), Move.Stand},
                {Tuple.Create(Rank.Queen, 3), Move.Stand},
                {Tuple.Create(Rank.King, 3), Move.Stand},
                {Tuple.Create(Rank.Ace, 3), Move.Split},
                
                // Player: Doubles Dealer: 4
                {Tuple.Create(Rank.Two, 4), Move.Split},
                {Tuple.Create(Rank.Three, 4), Move.Split},
                {Tuple.Create(Rank.Four, 4), Move.Hit},
                {Tuple.Create(Rank.Five, 4), Move.Double},
                {Tuple.Create(Rank.Six, 4), Move.Split},
                {Tuple.Create(Rank.Seven, 4), Move.Split},
                {Tuple.Create(Rank.Eight, 4), Move.Split},
                {Tuple.Create(Rank.Nine, 4), Move.Split},
                {Tuple.Create(Rank.Ten, 4), Move.Stand},
                {Tuple.Create(Rank.Jack, 4), Move.Stand},
                {Tuple.Create(Rank.Queen, 4), Move.Stand},
                {Tuple.Create(Rank.King, 4), Move.Stand},
                {Tuple.Create(Rank.Ace, 4), Move.Split},
                
                // Player: Doubles Dealer: 5
                {Tuple.Create(Rank.Two, 5), Move.Split},
                {Tuple.Create(Rank.Three, 5), Move.Split},
                {Tuple.Create(Rank.Four, 5), Move.Split},
                {Tuple.Create(Rank.Five, 5), Move.Double},
                {Tuple.Create(Rank.Six, 5), Move.Split},
                {Tuple.Create(Rank.Seven, 5), Move.Split},
                {Tuple.Create(Rank.Eight, 5), Move.Split},
                {Tuple.Create(Rank.Nine, 5), Move.Split},
                {Tuple.Create(Rank.Ten, 5), Move.Stand},
                {Tuple.Create(Rank.Jack, 5), Move.Stand},
                {Tuple.Create(Rank.Queen, 5), Move.Stand},
                {Tuple.Create(Rank.King, 5), Move.Stand},
                {Tuple.Create(Rank.Ace, 5), Move.Split},
                
                // Player: Doubles Dealer: 6
                {Tuple.Create(Rank.Two, 6), Move.Split},
                {Tuple.Create(Rank.Three, 6), Move.Split},
                {Tuple.Create(Rank.Four, 6), Move.Split},
                {Tuple.Create(Rank.Five, 6), Move.Double},
                {Tuple.Create(Rank.Six, 6), Move.Split},
                {Tuple.Create(Rank.Seven, 6), Move.Split},
                {Tuple.Create(Rank.Eight, 6), Move.Split},
                {Tuple.Create(Rank.Nine, 6), Move.Split},
                {Tuple.Create(Rank.Ten, 6), Move.Stand},
                {Tuple.Create(Rank.Jack, 6), Move.Stand},
                {Tuple.Create(Rank.Queen, 6), Move.Stand},
                {Tuple.Create(Rank.King, 6), Move.Stand},
                {Tuple.Create(Rank.Ace, 6), Move.Split},
                
                // Player: Doubles Dealer: 7
                {Tuple.Create(Rank.Two, 7), Move.Split},
                {Tuple.Create(Rank.Three, 7), Move.Split},
                {Tuple.Create(Rank.Four, 7), Move.Hit},
                {Tuple.Create(Rank.Five, 7), Move.Double},
                {Tuple.Create(Rank.Six, 7), Move.Hit},
                {Tuple.Create(Rank.Seven, 7), Move.Split},
                {Tuple.Create(Rank.Eight, 7), Move.Split},
                {Tuple.Create(Rank.Nine, 7), Move.Stand},
                {Tuple.Create(Rank.Ten, 7), Move.Stand},
                {Tuple.Create(Rank.Jack, 7), Move.Stand},
                {Tuple.Create(Rank.Queen, 7), Move.Stand},
                {Tuple.Create(Rank.King, 7), Move.Stand},
                {Tuple.Create(Rank.Ace, 7), Move.Split},
                
                // Player: Doubles Dealer: 8
                {Tuple.Create(Rank.Two, 8), Move.Hit},
                {Tuple.Create(Rank.Three, 8), Move.Hit},
                {Tuple.Create(Rank.Four, 8), Move.Hit},
                {Tuple.Create(Rank.Five, 8), Move.Double},
                {Tuple.Create(Rank.Six, 8), Move.Hit},
                {Tuple.Create(Rank.Seven, 8), Move.Hit},
                {Tuple.Create(Rank.Eight, 8), Move.Split},
                {Tuple.Create(Rank.Nine, 8), Move.Split},
                {Tuple.Create(Rank.Ten, 8), Move.Hit},
                {Tuple.Create(Rank.Jack, 8), Move.Hit},
                {Tuple.Create(Rank.Queen, 8), Move.Hit},
                {Tuple.Create(Rank.King, 8), Move.Hit},
                {Tuple.Create(Rank.Ace, 8), Move.Split},
                
                // Player: Doubles Dealer: 9
                {Tuple.Create(Rank.Two, 9), Move.Hit},
                {Tuple.Create(Rank.Three, 9), Move.Hit},
                {Tuple.Create(Rank.Four, 9), Move.Hit},
                {Tuple.Create(Rank.Five, 9), Move.Double},
                {Tuple.Create(Rank.Six, 9), Move.Hit},
                {Tuple.Create(Rank.Seven, 9), Move.Hit},
                {Tuple.Create(Rank.Eight, 9), Move.Split},
                {Tuple.Create(Rank.Nine, 9), Move.Split},
                {Tuple.Create(Rank.Ten, 9), Move.Hit},
                {Tuple.Create(Rank.Jack, 9), Move.Hit},
                {Tuple.Create(Rank.Queen, 9), Move.Hit},
                {Tuple.Create(Rank.King, 9), Move.Hit},
                {Tuple.Create(Rank.Ace, 9), Move.Split},
                
                // Player: Doubles Dealer: 10
                {Tuple.Create(Rank.Two, 10), Move.Hit},
                {Tuple.Create(Rank.Three, 10), Move.Hit},
                {Tuple.Create(Rank.Four, 10), Move.Hit},
                {Tuple.Create(Rank.Five, 10), Move.Hit},
                {Tuple.Create(Rank.Six, 10), Move.Hit},
                {Tuple.Create(Rank.Seven, 10), Move.Hit},
                {Tuple.Create(Rank.Eight, 10), Move.Split},
                {Tuple.Create(Rank.Nine, 10), Move.Stand},
                {Tuple.Create(Rank.Ten, 10), Move.Stand},
                {Tuple.Create(Rank.Jack, 10), Move.Stand},
                {Tuple.Create(Rank.Queen, 10), Move.Stand},
                {Tuple.Create(Rank.King, 10), Move.Stand},
                {Tuple.Create(Rank.Ace, 10), Move.Split},
                
                // Player: Doubles Dealer: 11
                {Tuple.Create(Rank.Two, 11), Move.Hit},
                {Tuple.Create(Rank.Three, 11), Move.Hit},
                {Tuple.Create(Rank.Four, 11), Move.Hit},
                {Tuple.Create(Rank.Five, 11), Move.Hit},
                {Tuple.Create(Rank.Six, 11), Move.Hit},
                {Tuple.Create(Rank.Seven, 11), Move.Hit},
                {Tuple.Create(Rank.Eight, 11), Move.Split},
                {Tuple.Create(Rank.Nine, 11), Move.Stand},
                {Tuple.Create(Rank.Ten, 11), Move.Stand},
                {Tuple.Create(Rank.Jack, 11), Move.Stand},
                {Tuple.Create(Rank.Queen, 11), Move.Stand},
                {Tuple.Create(Rank.King, 11), Move.Stand},
                {Tuple.Create(Rank.Ace, 11), Move.Split}
            };
      }

      public static Move NextMove(Hand hand, Dealer dealer)
      {
         if (hand.Cards.Count == 1)
         {
            return Move.Hit;
         }

         if (hand.IsPairs)
         {
            return _splitMoveMappings[Tuple.Create(hand.Cards.First().Rank, dealer.Hand.Value)];
         }

         var move = _hardSoftMoveMappings[Tuple.Create(hand.Value, hand.IsSoft, dealer.Hand.Value)];

         if (move == Move.Double && hand.Cards.Count > 2)
         {
            return Move.Hit;
         }

         return move;
      }
   }

   internal class Program
   {
      private static void Main(string[] args)
      {
         var table = new Table(deckCount: 4, minimumBet: 2, playersWhoCount: 1, n00bsCount: 5);
         table.Start();
      }
   }
}