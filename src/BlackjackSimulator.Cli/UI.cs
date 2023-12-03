using ConsoleTables;

namespace BlackjackSimulator.Cli;

public static class UI
{
    public static void WriteGameResult(Table table)
    {
        WritePlayerStats(table);
        WriteHands(table);
    }
    
    private static void WriteHands(Table table)
    {
        var consoleTable = new ConsoleTable("Name", "Bet", "Value", "Cards", "Result", "Winnings");
        consoleTable.AddRow("Dealer", "", table.Dealer.Hand.Value, string.Join(" ", table.Dealer.Hand.Cards), "", "");
        
        foreach (var player in table.Players)
        {
            if (player.Hands.Count == 0)
            {
                consoleTable.AddRow(player.Name, "", "", "", "", "");
            }
            else
            {
                foreach (var hand in player.Hands)
                {
                    consoleTable.AddRow(player.Name, hand.Bet, hand.Value, string.Join(" ", hand.Cards), hand.Result, hand.Winnings);
                }
            }
        }

        Console.WriteLine("Hands");
        Console.WriteLine();
        consoleTable.Write(Format.Minimal);
    }

    private static void WritePlayerStats(Table table)
    {
        var consoleTable = new ConsoleTable("Name", "Money", "Min", "Max", "Win", "Loss");

        foreach (var player in table.Players)
        {
            consoleTable.AddRow(player.Name,
                player.Money,
                player.MinimumMoney,
                player.MaximumMoney,
                player.Results.Count(r => r != Result.Loss),
                player.Results.Count(r => r == Result.Loss));
        }

        Console.WriteLine("Players");
        Console.WriteLine();
        consoleTable.Write(Format.Minimal);
    }

    public static void WriteTableStats(Table table)
    {
        var consoleTable = new ConsoleTable("#", "Card Count", "Deck Count", "Running Count", "True Count");
        consoleTable.AddRow(table.GameCount,
            table.Shoe.Cards.Count,
            table.Shoe.DeckCount,
            table.Shoe.RunningCount,
            table.Shoe.TrueCount);
        
        Console.WriteLine("Game");
        Console.WriteLine();
        consoleTable.Write(Format.Minimal);
    }
}