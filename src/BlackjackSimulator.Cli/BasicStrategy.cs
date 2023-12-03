namespace BlackjackSimulator.Cli;

public static class BasicStrategy
{
    private static readonly Dictionary<Tuple<int, bool, int>, Move> s_hardSoftMoveMappings;
    private static readonly Dictionary<Tuple<Rank, int>, Move> s_splitMoveMappings;

    static BasicStrategy()
    {
        s_hardSoftMoveMappings = new Dictionary<Tuple<int, bool, int>, Move>
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

        s_splitMoveMappings = new Dictionary<Tuple<Rank, int>, Move>
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

        if (hand.HasPairs)
        {
            return s_splitMoveMappings[Tuple.Create(hand.Cards.First().Rank, dealer.Hand.Value)];
        }

        var move = s_hardSoftMoveMappings[Tuple.Create(hand.Value, hand.IsSoft, dealer.Hand.Value)];

        if (move == Move.Double && hand.Cards.Count > 2)
        {
            return Move.Hit;
        }

        return move;
    }
}